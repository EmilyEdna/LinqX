﻿using System;
using System.Collections.Generic;
using System.Text;
using XExten.TracingClient.OpenTracing.Spans.Interface;

namespace XExten.TracingClient.OpenTracing.Noop
{
    public class NoopSpan : ISpan
    {
        public ISpanContext SpanContext { get; } = new NoopSpanContext();

        public Baggage Baggage => SpanContext.Baggage;

        public TagCollection Tags { get; } = new TagCollection();

        public LogCollection Logs { get; } = new LogCollection();

        public void Finish(DateTimeOffset finishTimestamp)
        {
        }

        public DateTimeOffset StartTimestamp { get; set; }

        public DateTimeOffset FinishTimestamp { get; set; }

        public string OperationName => string.Empty;

        public void Dispose()
        {
            Finish(DateTimeOffset.UtcNow);
        }
    }
}
