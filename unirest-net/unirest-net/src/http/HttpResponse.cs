using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace unirest_net.http
{
    public class HttpResponse<T>
    {
        public int Code { get; private set; }
        public Dictionary<String, String> Headers { get; private set; }
        public T Body { get; set; }
        public Stream Raw { get; private set; }

        public HttpResponse(System.Net.Http.HttpResponseMessage response)
        {
            Headers = new Dictionary<string, string>();
            Code = (int) response.StatusCode;

            if (response.Content != null)
            {
                var streamTask = response.Content.ReadAsStreamAsync();
                Task.WaitAll(streamTask);
                Raw = streamTask.Result;

                if (typeof(T) == typeof(String))
                {
                    var stringTask = response.Content.ReadAsStringAsync();
                    Task.WaitAll(stringTask);
                    Body = (T)(object)stringTask.Result;
                }
                else if (typeof(Stream).IsAssignableFrom(typeof(T)))
                {
                    Body = (T)(object)Raw;
                }
                else
                {
                    var serializer = new JavaScriptSerializer();
                    var stringTask = response.Content.ReadAsStringAsync();
                    Task.WaitAll(stringTask);
                    Body = serializer.Deserialize<T>(stringTask.Result);
                }
            }

            foreach (var header in response.Headers)
            {
                Headers.Add(header.Key, header.Value.First());
            }
        }
    }
}
