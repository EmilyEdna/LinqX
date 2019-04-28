﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XExten.APM.Entities;
using XExten.CacheFactory;

namespace XExten.APM
{
    /// <summary>
    /// 
    /// </summary>
    public class DataOperation
    {
        const string CACHE_NAMESPACE = "XExten_APM";
        private string _domain;
        private string _domainKey;
        //TODO：需要考虑分布式的情况，最好储存在缓存中
        private static Dictionary<string, Dictionary<string, DateTimeOffset>> KindNameStore { get; set; } = new Dictionary<string, Dictionary<string, DateTimeOffset>>();
        private string BuildFinalKey(string kindName)
        {
            return $"{_domainKey}:{kindName}";
        }
        /// <summary>
        /// 注册 Key
        /// </summary>
        /// <param name="kindName"></param>
        private void RegisterFinalKey(string kindName)
        {
            if (KindNameStore[_domain].ContainsKey(kindName))
            {
                return;
            }
            var kindNameKey = $"{_domainKey}:_KindNameStore";
            var keyList = Caches.RunTimeCacheGet<List<string>>(kindNameKey) ?? new List<string>();
            if (!keyList.Contains(kindName))
            {
                keyList.Add(kindName);
                Caches.RunTimeCacheSet(kindNameKey, keyList, 7200);//储存5天
            }
            KindNameStore[_domain][kindName] = DateTimeOffset.Now;
        }
        /// <summary>
        /// DataOperation 构造函数
        /// </summary>
        /// <param name="domain">域，统计的最小单位，可以是一个网站，也可以是一个模块</param>
        public DataOperation(string domain)
        {
            _domain = domain ?? "GLOBAL";//如果未提供，则统一为 GLOBAL，全局共享
            _domainKey = $"{CACHE_NAMESPACE}:{_domain}";

            if (!KindNameStore.ContainsKey(_domain))
            {
                KindNameStore[_domain] = new Dictionary<string, DateTimeOffset>();
            }
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="kindName">统计类别名称</param>
        /// <param name="value">统计值</param>
        /// <param name="data">复杂类型数据</param>
        /// <param name="tempStorage">临时储存信息</param>
        /// <param name="dateTime">发生时间，默认为当前系统时间</param>
        /// <returns></returns>
        public DataItem Set(string kindName, double value, object data = null, object tempStorage = null, DateTimeOffset? dateTime = null)
        {
            try
            {
                var dt1 = DateTimeOffset.Now;
                var finalKey = BuildFinalKey(kindName);
                var dataItem = new DataItem()
                {
                    KindName = kindName,
                    Value = value,
                    Data = data,
                    TempStorage = tempStorage,
                    DateTime = dateTime ?? DateTimeOffset.Now
                };
                var list = GetDataItemList(kindName);
                list.Add(dataItem);
                Caches.RunTimeCacheSet(finalKey, list, 7200);
                RegisterFinalKey(kindName);//注册Key
                Console.WriteLine($"APM 性能记录 - DataOperation.Set - {_domain}:{kindName}", (DateTimeOffset.Now - dt1).TotalMilliseconds + " ms");
                return dataItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="kindName"></param>
        /// <returns></returns>
        public List<DataItem> GetDataItemList(string kindName)
        {
            try
            {
                var finalKey = BuildFinalKey(kindName);
                var list = Caches.RunTimeCacheGet<List<DataItem>>(finalKey);
                return list ?? new List<DataItem>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取并清空该 Domain 下的所有数据
        /// </summary>
        /// <returns></returns>
        /// <param name="removeReadItems">是否移除已读取的项目，默认为 true</param>
        /// <param name="keepTodayData">当 removeReadItems = true 时有效，在清理的时候是否保留当天的数据</param>
        public List<MinuteDataPack> ReadAndCleanDataItems(bool removeReadItems = true, bool keepTodayData = true)
        {
            try
            {
                Dictionary<string, List<DataItem>> tempDataItems = new Dictionary<string, List<DataItem>>();
                var systemNow = DateTimeOffset.Now.UtcDateTime;//统一UTC时间
                var nowMinuteTime = DateTimeOffset.Now.AddSeconds(-DateTimeOffset.Now.Second).AddMilliseconds(-DateTimeOffset.Now.Millisecond);// new DateTimeOffset(systemNow.Year, systemNow.Month, systemNow.Day, systemNow.Hour, systemNow.Minute, 0, TimeSpan.Zero);
                //快速获取并清理数据
                foreach (var item in KindNameStore[_domain])
                {
                    var kindName = item.Key;
                    var finalKey = BuildFinalKey(kindName);
                    var list = GetDataItemList(item.Key);//获取列表
                    var completedStatData = list.Where(z => z.DateTime < nowMinuteTime).ToList();//统计范围内的所有数据

                    tempDataItems[kindName] = completedStatData;//添加到列表

                    if (removeReadItems)
                    {
                        //筛选需要删除的数据
                        var tobeRemove = completedStatData.Where(z => keepTodayData ? z.DateTime < DateTimeOffset.Now.Date : true);

                        //移除已读取的项目
                        if (tobeRemove.Count() == list.Count())
                        {
                            //已经全部删除
                            Caches.RunTimeCacheRemove(finalKey);
                        }
                        else
                        {
                            //部分删除
                            var newList = list.Except(tobeRemove).ToList();
                            Caches.RunTimeCacheSet(finalKey, newList, 7200);
                        }
                    }
                }
                //开始处理数据（分两步是为了减少同步锁的时间）
                var result = new List<MinuteDataPack>();
                foreach (var kv in tempDataItems)
                {
                    var kindName = kv.Key;
                    var domainData = kv.Value;

                    var lastDataItemTime = DateTimeOffset.MinValue;

                    MinuteDataPack minuteDataPack = new MinuteDataPack();
                    minuteDataPack.KindName = kindName;
                    result.Add(minuteDataPack);//添加一个指标

                    MinuteData minuteData = null;//某一分钟的指标
                    foreach (var dataItem in domainData)
                    {
                        if (DataHelper.IsLaterMinute(lastDataItemTime, dataItem.DateTime))
                        {
                            //新的一分钟
                            minuteData = new MinuteData();
                            minuteDataPack.MinuteDataList.Add(minuteData);

                            minuteData.KindName = dataItem.KindName;
                            minuteData.Time = new DateTimeOffset(dataItem.DateTime.Year, dataItem.DateTime.Month, dataItem.DateTime.Day, dataItem.DateTime.Hour, dataItem.DateTime.Minute, 0, TimeSpan.Zero);
                            minuteData.StartValue = dataItem.Value;
                            minuteData.HighestValue = dataItem.Value;
                            minuteData.LowestValue = dataItem.Value;
                        }
                        minuteData.EndValue = dataItem.Value;
                        minuteData.SumValue += dataItem.Value;
                        if (dataItem.Value > minuteData.HighestValue)
                        {
                            minuteData.HighestValue = dataItem.Value;
                        }
                        if (dataItem.Value < minuteData.LowestValue)
                        {
                            minuteData.LowestValue = dataItem.Value;
                        }
                        minuteData.SampleSize++;
                        lastDataItemTime = dataItem.DateTime;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
