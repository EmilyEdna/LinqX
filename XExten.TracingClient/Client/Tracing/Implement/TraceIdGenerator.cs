﻿using XExten.TracingClient.Client.Tracing.Interface;

namespace XExten.TracingClient.Client.Tracing.Implement
{
    public class TraceIdGenerator : ITraceIdGenerator
    {
        public string Next()
        {
            return RandomUtils.NextLong().ToString();
        }
    }
}
