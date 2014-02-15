using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using unirest_net.http;

namespace unirest_net.request
{
    public class HttpRequest
    {
        private bool hasFields;

        private bool hasExplicitBody;

        public Uri URL { get; protected set; }

        public HttpMethod HttpMethod { get; protected set; }

        public Dictionary<String, String> Headers { get; protected set; }

        public MultipartFormDataContent Body { get; private set; }

        // Should add overload that takes URL object
        public HttpRequest(HttpMethod method, string url)
        {
            Uri locurl;

            if (Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out locurl))
            {
                if (
                    !(locurl.IsAbsoluteUri &&
                      (locurl.Scheme == "http" || locurl.Scheme == "https")) ||
                    !locurl.IsAbsoluteUri)
                {
                    throw new ArgumentException("The url passed to the HttpMethod constructor is not a valid HTTP/S URL");
                }
            }
            else
            {
                throw new ArgumentException("The url passed to the HttpMethod constructor is not a valid HTTP/S URL");
            }

            URL = locurl;
            HttpMethod = method;
            Headers = new Dictionary<string, string>();
            Body = new MultipartFormDataContent();

        }

        public HttpRequest header(string name, string value)
        {
            Headers.Add(name, value);
            return this;
        }

        public HttpRequest headers(Dictionary<string, string> headers)
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    Headers.Add(header.Key, header.Value);
                }
            }

            return this;
        }

        public HttpRequest field(string name, string value)
        {
            if (HttpMethod == HttpMethod.Get)
            {
                throw new InvalidOperationException("Can't add body to Get request.");
            }

            if (hasExplicitBody)
            {
                throw new InvalidOperationException("Can't add fields to a request with an explicit body");
            }
            
            Body.Add(new StringContent(value), name);

            hasFields = true;
            return this;
        }

        public HttpRequest field(string name, byte[] data)
        {
            if (HttpMethod == HttpMethod.Get)
            {
                throw new InvalidOperationException("Can't add body to Get request.");
            }

            if (hasExplicitBody)
            {
                throw new InvalidOperationException("Can't add fields to a request with an explicit body");
            }
            //    here you can specify boundary if you need---^
            var imageContent = new ByteArrayContent(data);
            imageContent.Headers.ContentType =
                MediaTypeHeaderValue.Parse("image/jpeg");

            Body.Add(imageContent, name, "image.jpg");

            hasFields = true;
            return this;
        }

        public HttpRequest field(Stream value)
        {
            if (HttpMethod == HttpMethod.Get)
            {
                throw new InvalidOperationException("Can't add body to Get request.");
            }

            if (hasExplicitBody)
            {
                throw new InvalidOperationException("Can't add fields to a request with an explicit body");
            }

            Body.Add(new StreamContent(value));
            hasFields = true;
            return this;
        }

        public HttpRequest fields(Dictionary<string, object> parameters)
        {
            if (HttpMethod == HttpMethod.Get)
            {
                throw new InvalidOperationException("Can't add body to Get request.");
            }

            if (hasExplicitBody)
            {
                throw new InvalidOperationException("Can't add fields to a request with an explicit body");
            }

            Body.Add(new FormUrlEncodedContent(parameters.Where(kv => kv.Value is String).Select(kv => new KeyValuePair<string, string>(kv.Key, kv.Value as String))));

            foreach (var stream in parameters.Where(kv => kv.Value is Stream).Select(kv => kv.Value))
            {
                Body.Add(new StreamContent(stream as Stream));
            }

            hasFields = true;
            return this;
        }

        public HttpRequest body(string body)
        {
            if (HttpMethod == HttpMethod.Get)
            {
                throw new InvalidOperationException("Can't add body to Get request.");
            }

            if (hasFields)
            {
                throw new InvalidOperationException("Can't add explicit body to request with fields");
            }

            Body = new MultipartFormDataContent { new StringContent(body) };
            hasExplicitBody = true;
            return this;
        }

        public HttpRequest body<T>(T body)
        {
            if (HttpMethod == HttpMethod.Get)
            {
                throw new InvalidOperationException("Can't add body to Get request.");
            }

            if (hasFields)
            {
                throw new InvalidOperationException("Can't add explicit body to request with fields");
            }

            Body = new MultipartFormDataContent { new StringContent(JsonConvert.SerializeObject(body)) };
            hasExplicitBody = true;
            return this;
        }

        public HttpResponse<String> asString()
        {
            return HttpClientHelper.Request<String>(this);
        }

        public Task<HttpResponse<String>> asStringAsync()
        {
            return HttpClientHelper.RequestAsync<String>(this);
        }

        public HttpResponse<Stream> asBinary()
        {
            return HttpClientHelper.Request<Stream>(this);
        }

        public Task<HttpResponse<Stream>> asBinaryAsync()
        {
            return HttpClientHelper.RequestAsync<Stream>(this);
        }

        public HttpResponse<T> asJson<T>()
        {
            return HttpClientHelper.Request<T>(this);
        }

        public Task<HttpResponse<T>> asJsonAsync<T>()
        {
            return HttpClientHelper.RequestAsync<T>(this);
        }
    }
}
