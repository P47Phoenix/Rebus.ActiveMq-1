﻿#pragma warning disable 1998

namespace Rebus.ActiveMq.Tests
{
#if NET45 || NETSTANDARD2_0
    [TopicName(null)]
    public class NullTopic
    {
        public string Message { get; set; }
    }
#endif
}
