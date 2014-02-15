using FluentAssertions;
using System;
using System.Net.Http;
using unirest_net.http;
using unirest_net.request;

#if NETFX_CORE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace unirest_net_tests.http
{
    [TestClass]
    public class HttpClientHelperTests
    {
        [TestMethod]
        public void HttpClientHelper_Should_Request()
        {
            Action request = () => HttpClientHelper.Request<string>(new HttpRequest(HttpMethod.Get, "http://www.google.com"));
            request.ShouldNotThrow();
        }

        [TestMethod]
        public void HttpClientHelper_Should_Request_Async()
        {
            Action request = () => HttpClientHelper.RequestAsync<string>(new HttpRequest(HttpMethod.Get, "http://www.google.com"));
            request.ShouldNotThrow();
        }

        [TestMethod]
        public void HttpClientHelper_Should_Request_With_Fields()
        {
            Action request = () => HttpClientHelper.Request<string>(new HttpRequest(HttpMethod.Post, "http://www.google.com").field("test","value"));
            request.ShouldNotThrow();
        }

        [TestMethod]
        public void HttpClientHelper_Should_Request_Async_With_Fields()
        {
            Action request = () => HttpClientHelper.RequestAsync<string>(new HttpRequest(HttpMethod.Post, "http://www.google.com").field("test", "value"));
            request.ShouldNotThrow();
        }
    }
}
