﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XExten.HttpFactory.MultiImplement;
using XExten.HttpFactory.MultiInterface;

namespace XExten.HttpFactory
{
    /// <summary>
    /// 负载请求
    /// </summary>
    public class HttpMultiClient
    {
        /// <summary>
        /// Instance
        /// </summary>
        public static HttpMultiClient HttpMulti => new HttpMultiClient();
        /// <summary>
        /// Constructor
        /// </summary>
        public HttpMultiClient()
        {
            HttpMultiClientWare.FactoryClient = new HttpClient();
            HttpMultiClientWare.Container = new CookieContainer();
        }

        #region Header
        /// <summary>
        /// Add Header
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IHeaders Headers(string key, string value)
        {
            HttpMultiClientWare.FactoryClient.DefaultRequestHeaders.Add(key, value);
            return new Headers();
        }
        /// <summary>
        /// Add Header
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public IHeaders Headers(Dictionary<string, string> headers)
        {
            foreach (var item in headers)
            {
                HttpMultiClientWare.FactoryClient.DefaultRequestHeaders.Add(item.Key, item.Value);
            }
            return new Headers();
        }
        #endregion Header

        #region Cookie
        /// <summary>
        /// Add Cookie
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ICookies Cookies(string name, string value)
        {
            Cookie Cookie = new Cookie(name, value);
            HttpMultiClientWare.Container.Add(Cookie);
            return new Cookies();
        }
        /// <summary>
        /// Add Cookie
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public ICookies Cookies(string name, string value, string path)
        {
            Cookie Cookie = new Cookie(name, value, path);
            HttpMultiClientWare.Container.Add(Cookie);
            return new Cookies();
        }
        /// <summary>
        /// Add Cookie
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="path"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        public ICookies Cookies(string name, string value, string path, string domain)
        {
            Cookie Cookie = new Cookie(name, value, path, domain);
            HttpMultiClientWare.Container.Add(Cookie);
            return new Cookies();
        }
        #endregion Cookie

        #region URL
        /// <summary>
        /// Add Path
        /// </summary>
        /// <param name="Path">请求地址</param>
        /// <param name="Weight">1~100区间</param>
        /// <returns></returns>
        public INode AddNode(string Path, int Weight = 50)
        {
            WeightURL WeightUri = new WeightURL
            {
                Weight = Weight,
                URL = new Uri(Path)
            };
            HttpMultiClientWare.WeightPath.Add(WeightUri);
            return new Node();
        }
        /// <summary>
        /// Add Path
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Param"></param>
        /// <param name="Weight"></param>
        /// <returns></returns>
        public INode AddNode(string Path, string Param, int Weight = 50)
        {
            WeightURL WeightUri = new WeightURL
            {
                Weight = Weight,
                URL = new Uri(Path),
                StringContents = new StringContent(Param)
            };
            HttpMultiClientWare.WeightPath.Add(WeightUri);
            return new Node();
        }
        /// <summary>
        /// Add Path
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Path"></param>
        /// <param name="Param">实体模型</param>
        /// <param name="MapFied">映射字段</param>
        /// <param name="Weight"></param>
        /// <returns></returns>
        public INode AddNode<T>(string Path, T Param, IDictionary<string, string> MapFied = null, int Weight = 50) where T : class, new()
        {
            try
            {
                WeightURL WeightUri = new WeightURL
                {
                    Weight = Weight,
                    URL = new Uri(Path),
                    FormContent = new FormUrlEncodedContent(HttpKeyPairs.KeyValuePairs(Param, MapFied))
                };
                HttpMultiClientWare.WeightPath.Add(WeightUri);
                return new Node();
            }
            catch (Exception)
            {
                throw new Exception("参数类型不正确，参数只能是实体模型。");
            }
        }
        #endregion
    }
}
