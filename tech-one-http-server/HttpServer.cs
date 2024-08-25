using System;
using System.Web;
using System.Net;
using System.IO;
using System.Text.Json;
using Newtonsoft.Json;
using tech_one_http_server;
using Microsoft.AspNetCore.Http;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;
using System.Globalization;

public class HttpServer
{
    public int Port = 3001;

    private HttpListener _listener;

    public void Start()
    {
        _listener = new HttpListener();
        _listener.Prefixes.Add("http://localhost:" + Port.ToString() + "/");
        _listener.Start();
        Receive();
    }

    public void Stop()
    {
        _listener.Stop();
    }

    private void Receive()
    {
        _listener.BeginGetContext(new AsyncCallback(ListenerCallback), _listener);
    }

    private void ListenerCallback(IAsyncResult result)
    {
        if (_listener.IsListening)
        {
            var context = _listener.EndGetContext(result);
            var request = context.Request;
            var response = context.Response;
            JObject inJson;

            //return with CORS allowed. for security this wouldnt be * as origin depending on requirements
            if (request.HttpMethod == "OPTIONS")
            {
                Console.WriteLine("Setting CORS Headers");
                response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept, X-Requested-With");
                response.AddHeader("Access-Control-Allow-Methods", "GET, POST");
                response.AddHeader("Access-Control-Max-Age", "1728000");
            }
            response.AppendHeader("Access-Control-Allow-Origin", "*");

            // do something with the request
            Console.WriteLine($"{request.Url}");

            Console.WriteLine("URL");

            Console.WriteLine($"{request.HttpMethod} {request.Url}");

            Console.WriteLine("method url");

            Console.WriteLine(request);
            Console.WriteLine(request.HasEntityBody);

            if (request.ContentType != null)
            {
                Console.WriteLine("Client data content type {0}", request.ContentType);
            }
            Console.WriteLine("Client data content length {0}", request.ContentLength64);

            if (!request.HasEntityBody)
            {
                Console.WriteLine("Request sent with no body");
                response.StatusCode = (int)HttpStatusCode.OK;
                response.ContentType = "text/plain";
                response.OutputStream.Write(new byte[] { }, 0, 0);
                response.OutputStream.Close();
            }

            if (request.HasEntityBody)
            {
                Console.WriteLine("Has Entity Body");
                var body = request.InputStream;
                var encoding = request.ContentEncoding;
                var reader = new StreamReader(body, encoding);
                if (request.ContentType != null)
                {
                    Console.WriteLine("Client data content type {0}", request.ContentType);
                }
                Console.WriteLine("Client data content length {0}", request.ContentLength64);

                Console.WriteLine("Start of data:");
                string s = reader.ReadToEnd();
                inJson = JObject.Parse(s);

                Console.WriteLine(s);
                Console.WriteLine("End of data:");
                reader.Close();
                body.Close();

                Console.WriteLine(inJson);

                if (inJson.TryGetValue("NUMBER", out JToken jNumber))
                {
                    CurrencyToText resp = new CurrencyToText(jNumber.ToString());
                    Console.WriteLine("class: " + resp);
                    Console.WriteLine("class: " + resp.Value);
                    string json = JsonConvert.SerializeObject(resp);

                    Console.WriteLine("json: " + json);


                    //Write it to the response stream
                    var buffer = Encoding.UTF8.GetBytes(json);
                    response.ContentLength64 = buffer.Length;
                    response.OutputStream.Write(buffer, 0, buffer.Length);


                    response.StatusCode = (int)HttpStatusCode.OK;
                    response.ContentType = "application/json";
                    response.OutputStream.Write(new byte[] { }, 0, 0);
                    response.OutputStream.Close();

                    Console.WriteLine(response.Headers);
                    Console.WriteLine("response");
                }
            }
            Receive();
        }
    }

}