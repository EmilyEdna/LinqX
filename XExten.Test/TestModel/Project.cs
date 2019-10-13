﻿using ProtoBuf;
using System.ComponentModel;

namespace XExten.Test.TestModel
{
    public class TestA
    {
        [Description("标识")]
        public int Id { get; set; }

        [Description("姓名")]
        public string Name { get; set; }

        [Description("密码")]
        public string PassWord { get; set; }
    }

    [ProtoContract]
    public class TestB
    {
        [Description("Test")]
        [ProtoMember(1)]
        public int Id { get; set; }

        [Description("Test")]
        [ProtoMember(2)]
        public string Name { get; set; }

        [Description("Test")]
        [ProtoMember(3)]
        public string Account { get; set; }
    }
    public enum TestC
    {
        [Description("A")]
        A,
        [Description("B")]
        B
    }
}