
using Azure;
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
        private static readonly string ClientUserAgent = "DungeonCrawler/++UE5+Release-5.0-CL-0";

        private static bool isCorrectUserAgent(string userAgent)
            => userAgent.StartsWith(ClientUserAgent);

        private static async Task<bool> isValidRequest(HttpListenerRequest request, HttpListenerResponse response)
        {
#if DEBUG
            return true;
#endif

            if (!isCorrectUserAgent(request.UserAgent))
            {
                response.StatusCode = 403;
                await response.OutputStream.WriteAsync("Access denied".ToByteArray());

                return false;
            }

            return true;
        }

        private static async Task<HttpListenerResponse> indexHandler(HttpListenerRequest request, HttpListenerResponse response)
        {
            if (!await isValidRequest(request, response))
                return response;

            await response.OutputStream.WriteAsync("Index".ToByteArray());

            return response;
        }

        private static async Task<HttpListenerResponse> clientEntrypointHandler(HttpListenerRequest request, HttpListenerResponse response)
        {
            if (!await isValidRequest(request, response))
                return response;

            var responseBuffer = await Endpoints.ClientEntrypoint();
            await response.OutputStream.WriteAsync(responseBuffer.ToArray());

            return response;
        }

        private static readonly Dictionary<string, Func<HttpListenerRequest, HttpListenerResponse, Task< HttpListenerResponse > >> m_methodsMap = new Dictionary<string, Func<HttpListenerRequest, HttpListenerResponse, Task<HttpListenerResponse>>>()
        {
            { "/",              indexHandler },
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

                if (context.Request != null)
                    this.dispatcher(context.Request, context.Response);

                this.awaitRequest();
            }
        }

        private async void dispatcher(HttpListenerRequest request, HttpListenerResponse response)
        {
            if (request == null)
                return;

#if DEBUG
            Console.WriteLine("[CentralServer] Client hit: " + request.RawUrl);
#endif

            var requestedUrl = request.RawUrl;
            if (request.RawUrl == string.Empty)
                requestedUrl = "/";

            var invoker = m_methodsMap[requestedUrl];
            if (invoker != null)
                response = await invoker.Invoke(request, response);
            else
                response = await indexHandler(request, response);

            response.Close();
        }
    }
}
