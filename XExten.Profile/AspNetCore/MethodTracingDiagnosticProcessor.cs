﻿using System;
using System.Collections.Generic;
using System.Text;
using XExten.Profile.Abstractions;
using XExten.Profile.AspNetCore.DiagnosticProcessorName;
using XExten.Profile.Attributes;

namespace XExten.Profile.AspNetCore
{
    public class MethodTracingDiagnosticProcessor : ITracingDiagnosticProcessor
    {
        public string ListenerName { get; } = ProcessorName.MethodClient;

        [DiagnosticName(ProcessorName.MethodBegin)]
        public void MethodBeginInvoke([Object]Object data)
        {

        }

        [DiagnosticName(ProcessorName.MethodEnd)]
        public void MethodEndInvoke([Object]object data)
        {

        }

        [DiagnosticName(ProcessorName.MethodException)]
        public void MethodExceptionInvoke([Object]Exception exception)
        {

        }
    }
}
