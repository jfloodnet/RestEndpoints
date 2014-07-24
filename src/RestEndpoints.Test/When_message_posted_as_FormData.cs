using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Hosting;
using Ploeh.AutoFixture.Xunit;
using Ploeh.SemanticComparison.Fluent;
using RestEndpoints.Core;
using RestEndpoints.Core.Formatters;
using Should;
using Xunit;
using Xunit.Extensions;

namespace RestEndpoints.Test
{
    public class When_requesting_contracts_for_enpdoint
    {
        private EndpointController sut;

        public When_requesting_contracts_for_enpdoint()
        {
            sut = new EndpointController(new Endpoints()
            .ForEndpoint("TestEnpoint")
            .LookIn(Assembly.GetExecutingAssembly()).For(type => type == typeof(TestContract)));

            sut.Request = new HttpRequestMessage();
            sut.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
        }

        [Fact]
        public void Should_return_404_when_endpoint_not_found()
        {
            var result = sut.Get("No Endpoint");
            result.StatusCode.ShouldEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public void Should_return_404_when_contract_not_found()
        {
            var result = sut.Get("TestEndpoint", "No Contract");
            result.StatusCode.ShouldEqual(HttpStatusCode.NotFound);
        }
    }

    public class When_message_posted_as_FormData
    {
        private readonly ContractInstanceFormDataFormatter formatter;

        public When_message_posted_as_FormData()
        {
            formatter = new ContractInstanceFormDataFormatter();
        }

        [Fact]
        public void Should_support_application_form_data()
        {
            formatter.SupportedMediaTypes.ShouldContain(new MediaTypeHeaderValue("application/x-www-form-urlencoded"));
        }

        [Theory, AutoData]
        public void ContractInstance_should_be_capable_of_creating_instance_of_message(TestContract contract)
        {
            var sut = formatter.CreateContractInsanceFrom(ToFormData(contract));

            var expected = contract.AsSource().OfLikeness<TestContract>();
            var actual = (TestContract)sut.CreateMessage(typeof(TestContract));

            expected.ShouldEqual(actual);
        }

        private NameValueCollection ToFormData(TestContract contract)
        {
            var formData = new NameValueCollection();
            formData.Add("AString", contract.AString);
            formData.Add("AGuid", contract.AGuid.ToString());
            formData.Add("AnInt", contract.AnInt.ToString());
            return formData;
        }
    }
}