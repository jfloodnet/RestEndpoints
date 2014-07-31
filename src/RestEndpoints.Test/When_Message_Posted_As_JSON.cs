using System.Net.Http.Headers;
using Newtonsoft.Json;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit;
using Ploeh.SemanticComparison.Fluent;
using RestEndpoints.Core.Formatters;
using RestEndpoints.Core.Models;
using Should;
using Xunit;
using Xunit.Extensions;

namespace RestEndpoints.Test
{
    public class When_message_posted_as_Json
    {
        private readonly JsonContractInstance sut;
        private readonly TestContract contract;
            
        public When_message_posted_as_Json()
        {
            contract = new Fixture().Create<TestContract>();
            sut = new JsonContractInstance(JsonConvert.SerializeObject(contract));
        }

        [Fact]
        public void ContractInstance_should_be_capable_of_creating_instance_of_message()
        {
            var expected = contract.AsSource().OfLikeness<TestContract>();
            var actual = (TestContract)sut.CreateMessage(typeof (TestContract));
             
            expected.ShouldEqual(actual);
        }
    }
}
