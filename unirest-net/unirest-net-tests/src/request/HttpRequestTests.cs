using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;
using FluentAssertions;

using unirest_net;
using unirest_net.http;
using unirest_net.request;

using System.Net.Http;

namespace unicorn_net_tests.request
{
    [TestFixture]
    class HttpRequestTests
    {
        [Test]
        public static void HttpRequest_Should_Construct()
        {
            Action Get = () => new HttpRequest(HttpMethod.Get, "http://localhost");
            Action Post = () => new HttpRequest(HttpMethod.Post, "http://localhost");
            Action Delete = () => new HttpRequest(HttpMethod.Delete, "http://localhost");
            Action Patch = () => new HttpRequest(new HttpMethod("PATCH"), "http://localhost");
            Action Put = () => new HttpRequest(HttpMethod.Put, "http://localhost");

            Get.ShouldNotThrow();
            Post.ShouldNotThrow();
            Delete.ShouldNotThrow();
            Patch.ShouldNotThrow();
            Put.ShouldNotThrow();
        }

        [Test]
        public static void HttpRequest_Should_Not_Construct_With_Invalid_URL()
        {
            Action Get = () => new HttpRequest(HttpMethod.Get, "http:///invalid");
            Action Post = () => new HttpRequest(HttpMethod.Post, "http:///invalid");
            Action Delete = () => new HttpRequest(HttpMethod.Delete, "http:///invalid");
            Action Patch = () => new HttpRequest(new HttpMethod("PATCH"), "http:///invalid");
            Action Put = () => new HttpRequest(HttpMethod.Put, "http:///invalid");

            Get.ShouldThrow<ArgumentException>();
            Post.ShouldThrow<ArgumentException>();
            Delete.ShouldThrow<ArgumentException>();
            Patch.ShouldThrow<ArgumentException>();
            Put.ShouldThrow<ArgumentException>();
        }

        [Test]
        public static void HttpRequest_Should_Not_Construct_With_None_HTTP_URL()
        {
            Action Get = () => new HttpRequest(HttpMethod.Get, "ftp://localhost");
            Action Post = () => new HttpRequest(HttpMethod.Post, "mailto:localhost");
            Action Delete = () => new HttpRequest(HttpMethod.Delete, "news://localhost");
            Action Patch = () => new HttpRequest(new HttpMethod("PATCH"), "about:blank");
            Action Put = () => new HttpRequest(HttpMethod.Put, "about:settings");

            Get.ShouldThrow<ArgumentException>();
            Post.ShouldThrow<ArgumentException>();
            Delete.ShouldThrow<ArgumentException>();
            Patch.ShouldThrow<ArgumentException>();
            Put.ShouldThrow<ArgumentException>();
        }

        [Test]
        public static void HttpRequest_Should_Construct_With_Correct_Verb()
        {
            var Get = new HttpRequest(HttpMethod.Get, "http://localhost");
            var Post = new HttpRequest(HttpMethod.Post, "http://localhost");
            var Delete = new HttpRequest(HttpMethod.Delete, "http://localhost");
            var Patch = new HttpRequest(new HttpMethod("PATCH"), "http://localhost");
            var Put = new HttpRequest(HttpMethod.Put, "http://localhost");

            Get.HttpMethod.Should().Be(HttpMethod.Get);
            Post.HttpMethod.Should().Be(HttpMethod.Post);
            Delete.HttpMethod.Should().Be(HttpMethod.Delete);
            Patch.HttpMethod.Should().Be(new HttpMethod("PATCH"));
            Put.HttpMethod.Should().Be(HttpMethod.Put);
        }

        [Test]
        public static void HttpRequest_Should_Construct_With_Correct_URL()
        {
            var Get = new HttpRequest(HttpMethod.Get, "http://localhost");
            var Post = new HttpRequest(HttpMethod.Post, "http://localhost");
            var Delete = new HttpRequest(HttpMethod.Delete, "http://localhost");
            var Patch = new HttpRequest(new HttpMethod("PATCH"), "http://localhost");
            var Put = new HttpRequest(HttpMethod.Put, "http://localhost");

            Get.URL.OriginalString.Should().Be("http://localhost");
            Post.URL.OriginalString.Should().Be("http://localhost");
            Delete.URL.OriginalString.Should().Be("http://localhost");
            Patch.URL.OriginalString.Should().Be("http://localhost");
            Put.URL.OriginalString.Should().Be("http://localhost");
        }

        [Test]
        public static void HttpRequest_Should_Construct_With_Headers()
        {
            var Get = new HttpRequest(HttpMethod.Get, "http://localhost");
            var Post = new HttpRequest(HttpMethod.Post, "http://localhost");
            var Delete = new HttpRequest(HttpMethod.Delete, "http://localhost");
            var Patch = new HttpRequest(new HttpMethod("PATCH"), "http://localhost");
            var Put = new HttpRequest(HttpMethod.Put, "http://localhost");

            Get.Headers.Should().NotBeNull();
            Post.URL.OriginalString.Should().NotBeNull();
            Delete.URL.OriginalString.Should().NotBeNull();
            Patch.URL.OriginalString.Should().NotBeNull();
            Put.URL.OriginalString.Should().NotBeNull();
        }

        [Test]
        public static void HttpRequest_Should_Add_Headers()
        {
            var Get = new HttpRequest(HttpMethod.Get, "http://localhost");
            var Post = new HttpRequest(HttpMethod.Post, "http://localhost");
            var Delete = new HttpRequest(HttpMethod.Delete, "http://localhost");
            var Patch = new HttpRequest(new HttpMethod("PATCH"), "http://localhost");
            var Put = new HttpRequest(HttpMethod.Put, "http://localhost");

            Get.header("User-Agent", "unirest-net/1.0");
            Post.header("User-Agent", "unirest-net/1.0");
            Delete.header("User-Agent", "unirest-net/1.0");
            Patch.header("User-Agent", "unirest-net/1.0");
            Put.header("User-Agent", "unirest-net/1.0");

            Get.Headers.Should().Contain("User-Agent", "unirest-net/1.0");
            Post.Headers.Should().Contain("User-Agent", "unirest-net/1.0");
            Delete.Headers.Should().Contain("User-Agent", "unirest-net/1.0");
            Patch.Headers.Should().Contain("User-Agent", "unirest-net/1.0");
            Put.Headers.Should().Contain("User-Agent", "unirest-net/1.0");
        }

        [Test]
        public static void HttpRequest_Should_Add_Headers_Dictionary()
        {
            var Get = new HttpRequest(HttpMethod.Get, "http://localhost");
            var Post = new HttpRequest(HttpMethod.Post, "http://localhost");
            var Delete = new HttpRequest(HttpMethod.Delete, "http://localhost");
            var Patch = new HttpRequest(new HttpMethod("PATCH"), "http://localhost");
            var Put = new HttpRequest(HttpMethod.Put, "http://localhost");

            Get.headers(new Dictionary<string, string> { { "User-Agent", "unirest-net/1.0" } });
            Post.headers(new Dictionary<string, string> { { "User-Agent", "unirest-net/1.0" } });
            Delete.headers(new Dictionary<string, string> { { "User-Agent", "unirest-net/1.0" } });
            Patch.headers(new Dictionary<string, string> { { "User-Agent", "unirest-net/1.0" } });
            Put.headers(new Dictionary<string, string> { { "User-Agent", "unirest-net/1.0" } });

            Get.Headers.Should().Contain("User-Agent", "unirest-net/1.0");
            Post.Headers.Should().Contain("User-Agent", "unirest-net/1.0");
            Delete.Headers.Should().Contain("User-Agent", "unirest-net/1.0");
            Patch.Headers.Should().Contain("User-Agent", "unirest-net/1.0");
            Put.Headers.Should().Contain("User-Agent", "unirest-net/1.0");
        }

        [Test]
        public static void HttpRequest_Should_Return_String()
        {
            var Get = new HttpRequest(HttpMethod.Get, "http://www.google.com");
            var Post = new HttpRequest(HttpMethod.Post, "http://www.google.com");
            var Delete = new HttpRequest(HttpMethod.Delete, "http://www.google.com");
            var Patch = new HttpRequest(new HttpMethod("PATCH"), "http://www.google.com");
            var Put = new HttpRequest(HttpMethod.Put, "http://www.google.com");

            Get.asString().Body.Should().NotBeBlank();
            Post.asString().Body.Should().NotBeBlank();
            Delete.asString().Body.Should().NotBeBlank();
            Patch.asString().Body.Should().NotBeBlank();
            Put.asString().Body.Should().NotBeBlank();
        }

        [Test]
        public static void HttpRequest_Should_Return_Stream()
        {
            var Get = new HttpRequest(HttpMethod.Get, "http://www.google.com");
            var Post = new HttpRequest(HttpMethod.Post, "http://www.google.com");
            var Delete = new HttpRequest(HttpMethod.Delete, "http://www.google.com");
            var Patch = new HttpRequest(new HttpMethod("PATCH"), "http://www.google.com");
            var Put = new HttpRequest(HttpMethod.Put, "http://www.google.com");

            Get.asBinary().Body.Should().NotBeNull();
            Post.asBinary().Body.Should().NotBeNull();
            Delete.asBinary().Body.Should().NotBeNull();
            Patch.asBinary().Body.Should().NotBeNull();
            Put.asBinary().Body.Should().NotBeNull();
        }

        [Test]
        public static void HttpRequest_Should_Return_Parsed_JSON()
        {
            var Get = new HttpRequest(HttpMethod.Get, "http://www.google.com");
            var Post = new HttpRequest(HttpMethod.Post, "http://www.google.com");
            var Delete = new HttpRequest(HttpMethod.Delete, "http://www.google.com");
            var Patch = new HttpRequest(new HttpMethod("PATCH"), "http://www.google.com");
            var Put = new HttpRequest(HttpMethod.Put, "http://www.google.com");

            Get.asJson<String>().Body.Should().NotBeBlank();
            Post.asJson<String>().Body.Should().NotBeBlank();
            Delete.asJson<String>().Body.Should().NotBeBlank();
            Patch.asJson<String>().Body.Should().NotBeBlank();
            Put.asJson<String>().Body.Should().NotBeBlank();
        }

        [Test]
        public static void HttpRequest_Should_Return_String_Async()
        {
            var Get = new HttpRequest(HttpMethod.Get, "http://www.google.com").asStringAsync();
            var Post = new HttpRequest(HttpMethod.Post, "http://www.google.com").asStringAsync();
            var Delete = new HttpRequest(HttpMethod.Delete, "http://www.google.com").asStringAsync();
            var Patch = new HttpRequest(new HttpMethod("PATCH"), "http://www.google.com").asStringAsync();
            var Put = new HttpRequest(HttpMethod.Put, "http://www.google.com").asStringAsync();

            Task.WaitAll(Get, Post, Delete, Patch, Put);

            Get.Result.Body.Should().NotBeBlank();
            Post.Result.Body.Should().NotBeBlank();
            Delete.Result.Body.Should().NotBeBlank();
            Patch.Result.Body.Should().NotBeBlank();
            Put.Result.Body.Should().NotBeBlank();
        }

        [Test]
        public static void HttpRequest_Should_Return_Stream_Async()
        {
            var Get = new HttpRequest(HttpMethod.Get, "http://www.google.com").asBinaryAsync();
            var Post = new HttpRequest(HttpMethod.Post, "http://www.google.com").asBinaryAsync();
            var Delete = new HttpRequest(HttpMethod.Delete, "http://www.google.com").asBinaryAsync();
            var Patch = new HttpRequest(new HttpMethod("PATCH"), "http://www.google.com").asBinaryAsync();
            var Put = new HttpRequest(HttpMethod.Put, "http://www.google.com").asBinaryAsync();

            Task.WaitAll(Get, Post, Delete, Patch, Put);

            Get.Result.Body.Should().NotBeNull();
            Post.Result.Body.Should().NotBeNull();
            Delete.Result.Body.Should().NotBeNull();
            Patch.Result.Body.Should().NotBeNull();
            Put.Result.Body.Should().NotBeNull();
        }

        [Test]
        public static void HttpRequest_Should_Return_Parsed_JSON_Async()
        {
            var Get = new HttpRequest(HttpMethod.Get, "http://www.google.com").asJsonAsync<String>();
            var Post = new HttpRequest(HttpMethod.Post, "http://www.google.com").asJsonAsync<String>();
            var Delete = new HttpRequest(HttpMethod.Delete, "http://www.google.com").asJsonAsync<String>();
            var Patch = new HttpRequest(new HttpMethod("PATCH"), "http://www.google.com").asJsonAsync<String>();
            var Put = new HttpRequest(HttpMethod.Put, "http://www.google.com").asJsonAsync<String>();

            Task.WaitAll(Get, Post, Delete, Patch, Put);

            Get.Result.Body.Should().NotBeBlank();
            Post.Result.Body.Should().NotBeBlank();
            Delete.Result.Body.Should().NotBeBlank();
            Patch.Result.Body.Should().NotBeBlank();
            Put.Result.Body.Should().NotBeBlank();
        }

        [Test]
        public static void HttpRequest_With_Body_Should_Construct()
        {
            Action Post = () => new HttpRequest(HttpMethod.Post, "http://localhost");
            Action Delete = () => new HttpRequest(HttpMethod.Delete, "http://localhost");
            Action Patch = () => new HttpRequest(new HttpMethod("PATCH"), "http://localhost");
            Action Put = () => new HttpRequest(HttpMethod.Put, "http://localhost");

            Post.ShouldNotThrow();
            Delete.ShouldNotThrow();
            Patch.ShouldNotThrow();
            Put.ShouldNotThrow();
        }

        [Test]
        public static void HttpRequest_With_Body_Should_Not_Construct_With_Invalid_URL()
        {
            Action Post = () => new HttpRequest(HttpMethod.Post, "http:///invalid");
            Action delete = () => new HttpRequest(HttpMethod.Delete, "http:///invalid");
            Action patch = () => new HttpRequest(new HttpMethod("PATCH"), "http:///invalid");
            Action put = () => new HttpRequest(HttpMethod.Put, "http:///invalid");

            Post.ShouldThrow<ArgumentException>();
            delete.ShouldThrow<ArgumentException>();
            patch.ShouldThrow<ArgumentException>();
            put.ShouldThrow<ArgumentException>();
        }

        [Test]
        public static void HttpRequest_With_Body_Should_Not_Construct_With_None_HTTP_URL()
        {
            Action Post = () => new HttpRequest(HttpMethod.Post, "mailto:localhost");
            Action delete = () => new HttpRequest(HttpMethod.Delete, "news://localhost");
            Action patch = () => new HttpRequest(new HttpMethod("PATCH"), "about:blank");
            Action put = () => new HttpRequest(HttpMethod.Put, "about:settings");

            Post.ShouldThrow<ArgumentException>();
            delete.ShouldThrow<ArgumentException>();
            patch.ShouldThrow<ArgumentException>();
            put.ShouldThrow<ArgumentException>();
        }

        [Test]
        public static void HttpRequest_With_Body_Should_Construct_With_Correct_Verb()
        {
            var Post = new HttpRequest(HttpMethod.Post, "http://localhost");
            var Delete = new HttpRequest(HttpMethod.Delete, "http://localhost");
            var Patch = new HttpRequest(new HttpMethod("PATCH"), "http://localhost");
            var Put = new HttpRequest(HttpMethod.Put, "http://localhost");

            Post.HttpMethod.Should().Be(HttpMethod.Post);
            Delete.HttpMethod.Should().Be(HttpMethod.Delete);
            Patch.HttpMethod.Should().Be(new HttpMethod("PATCH"));
            Put.HttpMethod.Should().Be(HttpMethod.Put);
        }

        [Test]
        public static void HttpRequest_With_Body_Should_Construct_With_Correct_URL()
        {
            var Post = new HttpRequest(HttpMethod.Post, "http://localhost");
            var Delete = new HttpRequest(HttpMethod.Delete, "http://localhost");
            var Patch = new HttpRequest(new HttpMethod("PATCH"), "http://localhost");
            var Put = new HttpRequest(HttpMethod.Put, "http://localhost");

            Post.URL.OriginalString.Should().Be("http://localhost");
            Delete.URL.OriginalString.Should().Be("http://localhost");
            Patch.URL.OriginalString.Should().Be("http://localhost");
            Put.URL.OriginalString.Should().Be("http://localhost");
        }

        [Test]
        public static void HttpRequest_With_Body_Should_Construct_With_Headers()
        {
            var Post = new HttpRequest(HttpMethod.Post, "http://localhost");
            var Delete = new HttpRequest(HttpMethod.Delete, "http://localhost");
            var Patch = new HttpRequest(new HttpMethod("PATCH"), "http://localhost");
            var Put = new HttpRequest(HttpMethod.Put, "http://localhost");

            Post.URL.OriginalString.Should().NotBeNull();
            Delete.URL.OriginalString.Should().NotBeNull();
            Patch.URL.OriginalString.Should().NotBeNull();
            Put.URL.OriginalString.Should().NotBeNull();
        }

        [Test]
        public static void HttpRequest_With_Body_Should_Add_Headers()
        {
            var Post = new HttpRequest(HttpMethod.Post, "http://localhost");
            var Delete = new HttpRequest(HttpMethod.Delete, "http://localhost");
            var Patch = new HttpRequest(new HttpMethod("PATCH"), "http://localhost");
            var Put = new HttpRequest(HttpMethod.Put, "http://localhost");

            Post.header("User-Agent", "unirest-net/1.0");
            Delete.header("User-Agent", "unirest-net/1.0");
            Patch.header("User-Agent", "unirest-net/1.0");
            Put.header("User-Agent", "unirest-net/1.0");

            Post.Headers.Should().Contain("User-Agent", "unirest-net/1.0");
            Delete.Headers.Should().Contain("User-Agent", "unirest-net/1.0");
            Patch.Headers.Should().Contain("User-Agent", "unirest-net/1.0");
            Put.Headers.Should().Contain("User-Agent", "unirest-net/1.0");
        }

        [Test]
        public static void HttpRequest_With_Body_Should_Add_Headers_Dictionary()
        {
            var Post = new HttpRequest(HttpMethod.Post, "http://localhost");
            var Delete = new HttpRequest(HttpMethod.Delete, "http://localhost");
            var Patch = new HttpRequest(new HttpMethod("PATCH"), "http://localhost");
            var Put = new HttpRequest(HttpMethod.Put, "http://localhost");

            Post.headers(new Dictionary<string, string> { { "User-Agent", "unirest-net/1.0" } });
            Delete.headers(new Dictionary<string, string> { { "User-Agent", "unirest-net/1.0" } });
            Patch.headers(new Dictionary<string, string> { { "User-Agent", "unirest-net/1.0" } });
            Put.headers(new Dictionary<string, string> { { "User-Agent", "unirest-net/1.0" } });

            Post.Headers.Should().Contain("User-Agent", "unirest-net/1.0");
            Delete.Headers.Should().Contain("User-Agent", "unirest-net/1.0");
            Patch.Headers.Should().Contain("User-Agent", "unirest-net/1.0");
            Put.Headers.Should().Contain("User-Agent", "unirest-net/1.0");
        }

        [Test]
        public static void HttpRequest_With_Body_Should_Encode_Fields()
        {
            var Post = new HttpRequest(HttpMethod.Post, "http://localhost");
            var Delete = new HttpRequest(HttpMethod.Delete, "http://localhost");
            var Patch = new HttpRequest(new HttpMethod("PATCH"), "http://localhost");
            var Put = new HttpRequest(HttpMethod.Put, "http://localhost");

            Post.field("key", "value");
            Delete.field("key", "value");
            Patch.field("key", "value");
            Put.field("key", "value");

            Post.Body.Should().NotBeEmpty();
            Delete.Body.Should().NotBeEmpty();
            Patch.Body.Should().NotBeEmpty();
            Put.Body.Should().NotBeEmpty();
        }

        [Test]
        public static void HttpRequest_With_Body_Should_Encode_File()
        {
            var Post = new HttpRequest(HttpMethod.Post, "http://localhost");
            var Delete = new HttpRequest(HttpMethod.Delete, "http://localhost");
            var Patch = new HttpRequest(new HttpMethod("PATCH"), "http://localhost");
            var Put = new HttpRequest(HttpMethod.Put, "http://localhost");

            var stream = new MemoryStream();

            Post.field(stream);
            Delete.field(stream);
            Patch.field(stream);
            Put.field(stream);

            Post.Body.Should().NotBeEmpty();
            Delete.Body.Should().NotBeEmpty();
            Patch.Body.Should().NotBeEmpty();
            Put.Body.Should().NotBeEmpty();
        }

        [Test]
        public static void HttpRequestWithBody_Should_Encode_Multiple_Fields()
        {
            var Post = new HttpRequest(HttpMethod.Post, "http://localhost");
            var Delete = new HttpRequest(HttpMethod.Delete, "http://localhost");
            var Patch = new HttpRequest(new HttpMethod("PATCH"), "http://localhost");
            var Put = new HttpRequest(HttpMethod.Put, "http://localhost");

            var dict = new Dictionary<string, object>
                {
                    {"key", "value"},
                    {"key2", "value2"},
                    {"key3", new MemoryStream()}
                };

            Post.fields(dict);
            Delete.fields(dict);
            Patch.fields(dict);
            Put.fields(dict);

            Post.Body.Should().NotBeEmpty();
            Delete.Body.Should().NotBeEmpty();
            Patch.Body.Should().NotBeEmpty();
            Put.Body.Should().NotBeEmpty();
        }

        [Test]
        public static void HttpRequestWithBody_Should_Add_String_Body()
        {
            var Post = new HttpRequest(HttpMethod.Post, "http://localhost");
            var Delete = new HttpRequest(HttpMethod.Delete, "http://localhost");
            var Patch = new HttpRequest(new HttpMethod("PATCH"), "http://localhost");
            var Put = new HttpRequest(HttpMethod.Put, "http://localhost");

            Post.body("test");
            Delete.body("test");
            Patch.body("test");
            Put.body("test");

            Post.Body.Should().NotBeEmpty();
            Delete.Body.Should().NotBeEmpty();
            Patch.Body.Should().NotBeEmpty();
            Put.Body.Should().NotBeEmpty();
        }

        [Test]
        public static void HttpRequestWithBody_Should_Add_JSON_Body()
        {
            var Post = new HttpRequest(HttpMethod.Post, "http://localhost");
            var Delete = new HttpRequest(HttpMethod.Delete, "http://localhost");
            var Patch = new HttpRequest(new HttpMethod("PATCH"), "http://localhost");
            var Put = new HttpRequest(HttpMethod.Put, "http://localhost");

            Post.body(new List<int> { 1, 2, 3 });
            Delete.body(new List<int> { 1, 2, 3 });
            Patch.body(new List<int> { 1, 2, 3 });
            Put.body(new List<int> { 1, 2, 3 });

            Post.Body.Should().NotBeEmpty();
            Delete.Body.Should().NotBeEmpty();
            Patch.Body.Should().NotBeEmpty();
            Put.Body.Should().NotBeEmpty();
        }

        [Test]
        public static void Http_Request_Shouldnt_Add_Fields_To_Get()
        {
            var Get = new HttpRequest(HttpMethod.Get, "http://localhost");
            Action addStringField = () => Get.field("name", "value");
            Action addKeyField = () => Get.field(new MemoryStream());
            Action addStringFields = () => Get.fields(new Dictionary<string, object> {{"name", "value"}});
            Action addKeyFields = () => Get.fields(new Dictionary<string, object> {{"key", new MemoryStream()}});

            addStringField.ShouldThrow<InvalidOperationException>();
            addKeyField.ShouldThrow<InvalidOperationException>();
            addStringFields.ShouldThrow<InvalidOperationException>();
            addKeyFields.ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public static void Http_Request_Shouldnt_Add_Body_To_Get()
        {
            var Get = new HttpRequest(HttpMethod.Get, "http://localhost");
            Action addStringBody = () => Get.body("string");
            Action addJSONBody = () => Get.body(new List<int> {1,2,3});

            addStringBody.ShouldThrow<InvalidOperationException>();
            addJSONBody.ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public static void HttpRequestWithBody_Should_Not_Allow_Body_For_Request_With_Field()
        {
            var Post = new HttpRequest(HttpMethod.Post, "http://localhost");
            var Delete = new HttpRequest(HttpMethod.Delete, "http://localhost");
            var Patch = new HttpRequest(new HttpMethod("PATCH"), "http://localhost");
            var Put = new HttpRequest(HttpMethod.Put, "http://localhost");

            var stream = new MemoryStream();

            Post.field(stream);
            Delete.field(stream);
            Patch.field(stream);
            Put.field(stream);

            Action addBodyPost = () => Post.body("test");
            Action addBodyDelete = () => Delete.body("test");
            Action addBodyPatch = () => Patch.body("test");
            Action addBodyPut = () => Put.body("test");
            Action addObjectBodyPost = () => Post.body(1);
            Action addObjectBodyDelete = () => Delete.body(1);
            Action addObjectBodyPatch = () => Patch.body(1);
            Action addObjectBodyPut = () => Put.body(1);

            addBodyPost.ShouldThrow<InvalidOperationException>();
            addBodyDelete.ShouldThrow<InvalidOperationException>();
            addBodyPatch.ShouldThrow<InvalidOperationException>();
            addBodyPut.ShouldThrow<InvalidOperationException>();
            addObjectBodyPost.ShouldThrow<InvalidOperationException>();
            addObjectBodyDelete.ShouldThrow<InvalidOperationException>();
            addObjectBodyPatch.ShouldThrow<InvalidOperationException>();
            addObjectBodyPut.ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public static void HttpRequestWithBody_Should_Not_Allow_Fields_For_Request_With_Body()
        {
            var Post = new HttpRequest(HttpMethod.Post, "http://localhost");
            var Delete = new HttpRequest(HttpMethod.Delete, "http://localhost");
            var Patch = new HttpRequest(new HttpMethod("PATCH"), "http://localhost");
            var Put = new HttpRequest(HttpMethod.Put, "http://localhost");

            var stream = new MemoryStream();

            Post.body("test");
            Delete.body("test");
            Patch.body("test");
            Put.body("lalala");

            Action addFieldPost = () => Post.field("key", "value");
            Action addFieldDelete = () => Delete.field("key", "value");
            Action addFieldPatch = () => Patch.field("key", "value");
            Action addFieldPut = () => Put.field("key", "value");
            Action addStreamFieldPost = () => Post.field(stream);
            Action addStreamFieldDelete = () => Delete.field(stream);
            Action addStreamFieldPatch = () => Patch.field(stream);
            Action addStreamFieldPut = () => Put.field(stream);
            Action addFieldsPost = () => Post.fields(new Dictionary<string, object> {{"test", "test"}});
            Action addFieldsDelete = () => Delete.fields(new Dictionary<string, object> {{"test", "test"}});
            Action addFieldsPatch = () => Patch.fields(new Dictionary<string, object> {{"test", "test"}});
            Action addFieldsPut = () => Put.fields(new Dictionary<string, object> {{"test", "test"}});

            addFieldPost.ShouldThrow<InvalidOperationException>();
            addFieldDelete.ShouldThrow<InvalidOperationException>();
            addFieldPatch.ShouldThrow<InvalidOperationException>();
            addFieldPut.ShouldThrow<InvalidOperationException>();
            addStreamFieldPost.ShouldThrow<InvalidOperationException>();
            addStreamFieldDelete.ShouldThrow<InvalidOperationException>();
            addStreamFieldPatch.ShouldThrow<InvalidOperationException>();
            addStreamFieldPut.ShouldThrow<InvalidOperationException>();
            addFieldsPost.ShouldThrow<InvalidOperationException>();
            addFieldsDelete.ShouldThrow<InvalidOperationException>();
            addFieldsPatch.ShouldThrow<InvalidOperationException>();
            addFieldsPut.ShouldThrow<InvalidOperationException>();
        }
    }
}
