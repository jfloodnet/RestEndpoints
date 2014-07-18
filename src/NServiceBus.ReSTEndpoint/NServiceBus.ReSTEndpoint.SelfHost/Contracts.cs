using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NServiceBus.ReSTEndpoint.SelfHost
{
    public abstract class Endpoint1
    {
    }

    public abstract class Endpoint2
    {
    }

    public class TestCommand1 : Endpoint1
    {
        public string TestName { get; set; }
        public string StringProperty { get; set; }
        public int IntegerProperty { get; set; }
    }

    public class TestCommand2 : Endpoint1
    {
        public string TestName { get; set; }
        public string StringProperty { get; set; }
        public int IntegerProperty { get; set; }
    }

    public class TestCommand3 : Endpoint2
    {
        public string TestName { get; set; }
        public string StringProperty { get; set; }
        public int IntegerProperty { get; set; }
    }

    public class TestCommand4 : Endpoint2
    {
        public string TestName { get; set; }
        public string StringProperty { get; set; }
        public int IntegerProperty { get; set; }
    }
}
