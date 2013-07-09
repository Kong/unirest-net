Unirest-Net
============================================

Unirest is a set of lightweight HTTP libraries available in PHP, Ruby, Python, Java, Objective-C.
This is a port of the Java library to .NET, and is done independently and without affiliation to the original Unirest project.

Documentation
-------------------

### Installing
Is easy as pie. Kidding. It's as easy as downloading from [NuGet](https://nuget.org/packages/Unirest-Net/1.0.0-beta).

### Creating Request
So you're probably wondering how using Unirest makes creating requests in .NET easier, here is a basic POST request that will explain everything:

```C#
HttpResponse<MyClass> jsonResponse = Unirest.post("http://httpbin.org/post")
  .header("accept", "application/json")
  .field("parameter", "value")
  .field("foo", "bar")
  .asJson<MyClass>();
```

Requests are made when `as[Type]()` is invoked, possible types include `Json`, `Binary`, `String`. If the request supports this, a body  can be passed along with `.body(String)` or `body<T>(T)` to serialize an arbitary object to JSON. If you already have a dictionary of parameters or do not wish to use seperate field methods for each one there is a `.fields(Dictionary<string, object> parameters)` method that will serialize each key - value to form parameters on your request.

`.headers(Dictionary<string, string> headers)` is also supported in replacement of multiple header methods.

### Asynchronous Requests
Sometimes, well most of the time, you want your application to be asynchronous and not block, Unirest supports this in .NET with the TPL pattern and async/await:

```C#
Task<HttpResponse<MyClass>> myClassTask = Unirest.post("http://httpbin.org/post")
  .header("accept", "application/json")
  .field("param1", "value1")
  .field("param2", "value2")
  .asJsonAsync<MyClass>();
```

### File Uploads
Creating `multipart` requests with .NET is trivial, simply pass along a `Stream` Object as a field:

```C#
HttpResponse<MyClass> myClass = Unirest.post("http://httpbin.org/post")
  .header("accept", "application/json")
  .field("parameter", "value")
  .field("file", new MemoryStream("/tmp/file"))
  .asJson<MyClass>();
```

### Custom Entity Body

```C#
HttpResponse<MyClass> myClass = Unirest.post("http://httpbin.org/post")
  .header("accept", "application/json")
  .body("{\"parameter\":\"value\", \"foo\":\"bar\"}")
  .asJson<MyClass>();
```

### Request Reference

The .NET Unirest library follows the builder style conventions. You start building your request by creating a `HttpRequest` object using one of the following:

```C#
HttpRequest request = Unirest.get(String url);
HttpRequest request = Unirest.post(String url);
HttpRequest request = Unirest.put(String url);
HttpRequest request = Unirest.patch(String url);
HttpRequest request = Unirest.delete(String url);
```

### Response Reference

Upon recieving a response Unirest returns the result in the form of an Object, this object should always have the same keys for each language regarding to the response details.

`.Code`  
HTTP Response Status Code (Example 200)

`.Headers`  
HTTP Response Headers

`.Body`  
Parsed response body where applicable, for example JSON responses are parsed to Objects / Associative Arrays.

`.Raw`  
Un-parsed response body



License
---------------

The MIT License

Copyright (c) 2013 Mashape (http://mashape.com)

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
