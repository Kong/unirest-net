using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;

namespace unirest_net.http
{
    [TestClass]
    public class UnicornTest
    {
        [TestMethod]
        public void Unicorn_Should_Return_Correct_Verb()
        {
            Unirest.get("http://localhost").HttpMethod.Should().Be(HttpMethod.Get);
            Unirest.post("http://localhost").HttpMethod.Should().Be(HttpMethod.Post);
            Unirest.delete("http://localhost").HttpMethod.Should().Be(HttpMethod.Delete);
            Unirest.patch("http://localhost").HttpMethod.Should().Be(new HttpMethod("PATCH"));
            Unirest.put("http://localhost").HttpMethod.Should().Be(HttpMethod.Put);
        }

        [TestMethod]
        public void Unicorn_Should_Return_Correct_URL()
        {
            Unirest.get("http://localhost").URL.OriginalString.Should().Be("http://localhost");
            Unirest.post("http://localhost").URL.OriginalString.Should().Be("http://localhost");
            Unirest.delete("http://localhost").URL.OriginalString.Should().Be("http://localhost");
            Unirest.patch("http://localhost").URL.OriginalString.Should().Be("http://localhost");
            Unirest.put("http://localhost").URL.OriginalString.Should().Be("http://localhost");
        }
    }
}
