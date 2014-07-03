using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net;
using unirest_net.http;

namespace unirest_net.request
{
    public class HttpRequest
    {
        private bool hasFields;

        private bool hasExplicitBody;

        public NetworkCredential NetworkCredentials { get; protected set; }

        public Func<HttpRequestMessage, bool> Filter { get; protected set; }

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

        public HttpRequest header(string name, object value)
        {
            if (value != null)
                Headers.Add(name, value.ToString());

            return this;
        }
        
        public HttpRequest headers(Dictionary<string, object> headers)
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    if(header.Value != null)
                        Headers.Add(header.Key, header.Value.ToString());
                }
            }

            return this;
        }

        public HttpRequest field(string name, object value)
        {
            if ((HttpMethod == HttpMethod.Get) || (HttpMethod == HttpMethod.Head) || (HttpMethod == HttpMethod.Trace))
            {
                throw new InvalidOperationException(string.Format("Can't add body to {0} request.", HttpMethod));
            }

            if (hasExplicitBody)
            {
                throw new InvalidOperationException("Can't add fields to a request with an explicit body");
            }

            if (value == null)
                return this;

            Body.Add(new StringContent(value.ToString()), name);

            hasFields = true;           

            return this;
        }

        public HttpRequest field(string name, byte[] data)
        {
            if ((HttpMethod == HttpMethod.Get) || (HttpMethod == HttpMethod.Head) || (HttpMethod == HttpMethod.Trace))
            {
                throw new InvalidOperationException(string.Format("Can't add body to {0} request.", HttpMethod));
            }

            if (hasExplicitBody)
            {
                throw new InvalidOperationException("Can't add fields to a request with an explicit body");
            }

            if (data == null)
                return this;

            //    here you can specify boundary if you need---^
            var imageContent = new ByteArrayContent(data);
            imageContent.Headers.ContentType =
                MediaTypeHeaderValue.Parse("image/jpeg");

            Body.Add(imageContent, name, "image.jpg");

            hasFields = true;
            return this;
        }

        public HttpRequest basicAuth(string userName, string passWord)
        {
            if (this.NetworkCredentials != null)
            {
                throw new InvalidOperationException("Basic authentication credentials are already set.");
            }

            this.NetworkCredentials = new NetworkCredential(userName, passWord);
            return this;
        }

        /// <summary>
        /// Set a delegate to a message filter. This is particularly useful for using external
        /// authentication libraries such as uniauth (https://github.com/zeeshanejaz/uniauth-net)
        /// </summary>
        /// <param name="handler">Filter accepting HttpRequestMessage and returning bool</param>
        /// <returns>updated reference</returns>
        public HttpRequest filter(Func<HttpRequestMessage, bool> filter)
        {            
            if (this.Filter != null)
            {
                throw new InvalidOperationException("Processing filter is already set.");
            }

            this.Filter = filter;
            return this;
        }

        public HttpRequest field(Stream value)
        {
            if ((HttpMethod == HttpMethod.Get) || (HttpMethod == HttpMethod.Head) || (HttpMethod == HttpMethod.Trace))
            {
                throw new InvalidOperationException(string.Format("Can't add body to {0} request.", HttpMethod));
            }

            if (hasExplicitBody)
            {
                throw new InvalidOperationException("Can't add fields to a request with an explicit body");
            }

            if (value == null)
                return this;

            Body.Add(new StreamContent(value));
            hasFields = true;
            return this;
        }

        public HttpRequest fields(Dictionary<string, object> parameters)
        {
            if (parameters == null)
                return this;

            if ((HttpMethod == HttpMethod.Get) || (HttpMethod == HttpMethod.Head) || (HttpMethod == HttpMethod.Trace))
            {
                throw new InvalidOperationException(string.Format("Can't add body to {0} request.", HttpMethod));
            }

            if (hasExplicitBody)
            {
                throw new InvalidOperationException("Can't add fields to a request with an explicit body");
            }

            Body.Add(new FormUrlEncodedContent(parameters.Where(kv => isPrimitiveType(kv.Value)).Select(kv => new KeyValuePair<string, string>(kv.Key, kv.Value.ToString()))));

            foreach (var stream in parameters.Where(kv => kv.Value is Stream).Select(kv => kv.Value))
            {
                if (stream == null)
                    continue;

                Body.Add(new StreamContent(stream as Stream));
            }

            hasFields = true;
            return this;
        }

        private bool isPrimitiveType(object obj)
        {
            if (obj == null)
                return false;

            return obj.GetType().IsPrimitive;
        }

        public HttpRequest body(string body)
        {
            if ((HttpMethod == HttpMethod.Get) || (HttpMethod == HttpMethod.Head) || (HttpMethod == HttpMethod.Trace))
            {
                throw new InvalidOperationException(string.Format("Can't add body to {0} request.", HttpMethod));
            }

            if (hasFields)
            {
                throw new InvalidOperationException("Can't add explicit body to request with fields");
            }

            if (body == null)
                return this;

            Body = new MultipartFormDataContent { new StringContent(body) };
            hasExplicitBody = true;
            return this;
        }

        public HttpRequest body<T>(T body)
        {
            if ((HttpMethod == HttpMethod.Get) || (HttpMethod == HttpMethod.Head) || (HttpMethod == HttpMethod.Trace))
            {
                throw new InvalidOperationException(string.Format("Can't add body to {0} request.", HttpMethod));
            }

            if (hasFields)
            {
                throw new InvalidOperationException("Can't add explicit body to request with fields");
            }

            if (body == null)
                return this;

            if (body is Stream)
            {
                Stream inputStream = (body as Stream);
                if (!inputStream.CanRead)
                    throw new ArgumentException("Excepting a readable stream");

                StreamReader reader = new StreamReader(inputStream);
                string fileContent = reader.ReadToEnd();
                Body = new MultipartFormDataContent { new StringContent(fileContent) };
            }
            else
            {
                Body = new MultipartFormDataContent { new StringContent(JsonConvert.SerializeObject(body)) };
            }

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
