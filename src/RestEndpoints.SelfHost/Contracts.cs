using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NServiceBus.ReSTEndpoint.SelfHost
{
    public abstract class Billing
    {
    }

    public abstract class Sales
    {
    }

    public abstract class Clients
    {
    }

    public class ExecuteBillRunCommand : Billing
    {
        public Guid TargetAccountID { get; set; }
        public DateTime TargetDate { get; set; }
        public string Comment { get; set; }
    }

    public class PostInvoiceCommand : Billing
    {
        public string TestName { get; set; }
        public string StringProperty { get; set; }
        public int IntegerProperty { get; set; }
        public ExecuteBillRunCommand NestedBill { get; set; }
    }

    public class WinSaleCommand : Sales
    {
        public string TestName { get; set; }
        public string StringProperty { get; set; }
        public int IntegerProperty { get; set; }
        public PostInvoiceCommand Nested { get; set; }
    }

    public class QualifyLeadCommand : Sales
    {
        public string TestName { get; set; }
        public string StringProperty { get; set; }
        public int IntegerProperty { get; set; }
    }

    public class CreateClientCommand : Clients
    {
        public string TestName { get; set; }
        public string StringProperty { get; set; }
        public int IntegerProperty { get; set; }
    }

    public class ChangeClientDetailsCommand : Clients
    {
        public string TestName { get; set; }
        public string StringProperty { get; set; }
        public int IntegerProperty { get; set; }
    }
}
