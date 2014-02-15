using FluentAssertions;
using System;
using System.IO;
using System.Net.Http;
using unirest_net.http;

#if NETFX_CORE || WINDOWS_PHONE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace unirest_net_tests.http
{
    [TestClass]
    public class HttpResponseTests
    {
        [TestMethod]
        public void HttpResponse_Should_Construct()
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
