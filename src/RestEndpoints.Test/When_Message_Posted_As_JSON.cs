using System.Net.Http.Headers;
using Newtonsoft.Json;
using Ploeh.AutoFixture.Xunit;
using Ploeh.SemanticComparison.Fluent;
using RestEndpoints.Core.Formatters;
using Should;
using Xunit;
using Xunit.Extensions;

namespace RestEndpoints.Test
{
    public class When_message_posted_as_Json
    {
        private readonly ContractInstanceJsonFormatter formatter;

        public When_message_posted_as_Json()
        {
            formatter = new ContractInstanceJsonFormatter();
        }

        [Fact]
        public void Should_support_application_json()
        {
            formatter.SupportedMediaTypes.ShouldContain(new MediaTypeHeaderValue("application/json"));
        }

        [Theory, AutoData]
        public void ContractInstance_should_be_capable_of_creating_instance_of_message(TestContract contract)
        {
            var sut = formatter.CreateContractInstanceFrom(JsonConvert.SerializeObject(contract));

            var expected = contract.AsSource().OfLikeness<TestContract>();
            var actual = (TestContract)sut.CreateMessage(typeof (TestContract));
             
            expected.ShouldEqual(actual);
        }
    }
}
