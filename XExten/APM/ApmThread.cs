﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XExten.APM
{
    /// <summary>
    /// APM 线程
    /// </summary>
    public class ApmThread
    {
        private DateTimeOffset LastRecordTime = DateTime.MinValue;
        /// <summary>
        /// 打开 CPU 状态监控
        /// </summary>
        public bool OpenCpuWatch { get; set; } = false;
        /// <summary>
        /// 打开内存状态监控
        /// </summary>
        public bool OpenMemoryWatch { get; set; } = false;
        /// <summary>
        /// 执行
        /// </summary>
        public void Run()
        {
            while (true)
            {
                if (DataHelper.IsLaterMinute(LastRecordTime, DateTimeOffset.Now))
                {
                    //进行统计并清理多余数据
                    //进行数据清理
                    LastRecordTime = DateTimeOffset.Now;
                }
                Thread.Sleep(1000 * 10);//间隔1分钟以内
            }
        }
    }
}
