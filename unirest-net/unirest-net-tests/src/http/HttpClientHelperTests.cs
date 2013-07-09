using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;

using NUnit;
using NUnit.Framework;
using FluentAssertions;

using unirest_net.http;
using unirest_net.request;

namespace unirest_net_tests.http
{
    [TestFixture]
    class HttpClientHelperTests
    {
        [Test]
        public static void HttpClientHelper_Should_Reqeust()
        {
            Action request = () => HttpClientHelper.Request<string>(new HttpRequest(HttpMethod.Get, "http://www.google.com"));
            request.ShouldNotThrow();
        }

        [Test]
        public static void HttpClientHelper_Should_Reqeust_Async()
        {
            Action request = () => HttpClientHelper.RequestAsync<string>(new HttpRequest(HttpMethod.Get, "http://www.google.com"));
            request.ShouldNotThrow();
        }

        [Test]
        public static void HttpClientHelper_Should_Reqeust_With_Fields()
        {
            Action request = () => HttpClientHelper.Request<string>(new HttpRequest(HttpMethod.Post, "http://www.google.com").field("test","value"));
            request.ShouldNotThrow();
        }

        [Test]
        public static void HttpClientHelper_Should_Reqeust_Async_With_Fields()
        {
            Action request = () => HttpClientHelper.RequestAsync<string>(new HttpRequest(HttpMethod.Post, "http://www.google.com").field("test", "value"));
            request.ShouldNotThrow();
        }
    }
}
