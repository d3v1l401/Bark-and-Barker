﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BarkAndBarker
{
    public class CentralServer
    {
        private HttpListener m_listener = null;
        private static readonly string ClientUserAgent = "DungeonCrawler/++UE5+Release-5.0-CL-0 Windows/10.0.22621.1.256.64bit";

        private static async Task<HttpListenerResponse> indexHandler(HttpListenerRequest request)
        {
            return null;
        }

        private static async Task<HttpListenerResponse> clientEntrypointHandler(HttpListenerRequest request)
        {
            return await Endpoints.ClientEntrypoint();
        }

        private static readonly Dictionary<string, Func<HttpListenerRequest, Task<HttpListenerResponse>>> m_methodsMap = new Dictionary<string, HttpListenerResponse>()
        {
            { "/", indexHandler },
            { "/dc/helloWorld", clientEntrypointHandler }
        };

        // https://stackoverflow.com/questions/4019466/httplistener-access-denied
        public CentralServer(string bindAddress = "*", UInt16 port = 80)
        {
            this.m_listener = new HttpListener();
            this.m_listener.Prefixes.Add(string.Format("http://{0}:{1}/", bindAddress, port.ToString()));
        }

        public void Start()
        {
            if (this.m_listener != null) { 
                this.m_listener.Start();
                this.awaitRequest();
            }
        }

        public void Stop()
        {
            if (this.m_listener != null)
                this.m_listener.Stop();
        }

        private void awaitRequest()
        {
            if (m_listener != null)
                this.m_listener.BeginGetContext(new AsyncCallback(callback), this.m_listener);
        }

        private async void callback(IAsyncResult result)
        {
            if (this.m_listener != null && this.m_listener.IsListening)
            {
                var context = this.m_listener.EndGetContext(result);

                var request = context.Request;

                if (request != null)
                {
                    var response = await this.dispatcher(request, context.Response);

                    await context.Response.OutputStream.WriteAsync(response.ToArray());

                    context.Response.Close();
                }

                this.awaitRequest();
            }
        }

        private async Task<MemoryStream> dispatcher(HttpListenerRequest request)
        {
            if (request == null)
                return null;

            if (request.UserAgent != ClientUserAgent)
                return new MemoryStream(Encoding.UTF8.GetBytes("403 - Access denied.")); // Responding with 200 OK but a payload of a 403...MEH!

#if DEBUG
            Console.WriteLine("[CentralServer] Client hit: " + request.RawUrl);
#endif
            response.ContentType = "text/html";

            var requestedUrl = request.RawUrl;
            if (request.RawUrl == string.Empty)
                requestedUrl = "/";

            HttpListenerResponse response;
            response.ContentType = "text/html";

            var invoker = m_methodsMap[requestedUrl];
            if (invoker != null)
                await invoker.Invoke(request);
            else
                await indexHandler(request);

            switch (request.RawUrl)
            {
                case "/":
                    return new MemoryStream(Encoding.UTF8.GetBytes("Index"));
                case "/dc/helloWorld":
                    return await Endpoints.ClientEntrypoint();
                default:
                    return new MemoryStream(Encoding.UTF8.GetBytes("404 - Not Found."));
            }
        }
    }
}
