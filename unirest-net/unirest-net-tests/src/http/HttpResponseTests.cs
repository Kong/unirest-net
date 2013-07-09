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
    class HttpResponseTests
    {
        [Test]
        public static void HttpResponse_Should_Construct()
        {
            Action stringResponse = () => new HttpResponse<string>(new HttpResponseMessage { Content = new StringContent("test") });
            Action streamResponse = () => new HttpResponse<Stream>(new HttpResponseMessage { Content = new StreamContent(new MemoryStream())});
            Action objectResponse = () => new HttpResponse<int>(new HttpResponseMessage { Content = new StringContent("1")});

            stringResponse.ShouldNotThrow();
            streamResponse.ShouldNotThrow();
            objectResponse.ShouldNotThrow();
        }
    }
}
