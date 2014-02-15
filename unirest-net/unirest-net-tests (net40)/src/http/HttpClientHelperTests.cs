using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;
using unirest_net.http;
using unirest_net.request;

namespace unirest_net_tests.http
{
    [TestClass]
    public class HttpClientHelperTests
    {
        [TestMethod]
        public void HttpClientHelper_Should_Reqeust()
        {
            Action request = () => HttpClientHelper.Request<string>(new HttpRequest(HttpMethod.Get, "http://www.google.com"));
            request.ShouldNotThrow();
        }

        [TestMethod]
        public void HttpClientHelper_Should_Reqeust_Async()
        {
            Action request = () => HttpClientHelper.RequestAsync<string>(new HttpRequest(HttpMethod.Get, "http://www.google.com"));
            request.ShouldNotThrow();
        }

        [TestMethod]
        public void HttpClientHelper_Should_Reqeust_With_Fields()
        {
            Action request = () => HttpClientHelper.Request<string>(new HttpRequest(HttpMethod.Post, "http://www.google.com").field("test","value"));
            request.ShouldNotThrow();
        }

        [TestMethod]
        public void HttpClientHelper_Should_Reqeust_Async_With_Fields()
        {
            Action request = () => HttpClientHelper.RequestAsync<string>(new HttpRequest(HttpMethod.Post, "http://www.google.com").field("test", "value"));
            request.ShouldNotThrow();
        }
    }
}
