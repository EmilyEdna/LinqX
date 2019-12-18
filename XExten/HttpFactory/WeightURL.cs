﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace XExten.HttpFactory
{
    /// <summary>
    /// 负载路由
    /// </summary>
    public struct WeightURL
    {
        /// <summary>
        /// 请求类型
        /// </summary>
        public RequestType Request { get; set; }
        /// <summary>
        /// 负载比
        /// </summary>
        public int Weight { get; set; }
        /// <summary>
        /// 路由
        /// </summary>
        public Uri URL { get; set; }
        /// <summary>
        /// Json
        /// </summary>
        public HttpContent Contents { get; set; }
    }

}
