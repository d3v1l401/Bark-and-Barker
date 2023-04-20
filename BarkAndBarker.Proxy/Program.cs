using System.Net.Sockets;
using System.Net;
using System.Text;

namespace BarkAndBarker.Proxy
{
    class Program
    {
        static void Main(string[] args)
        {
            var centralProxy = new CentralProxy("127.0.0.1", 30000, "54.148.133.180", 30000); //Normal server
            //var centralProxy = new CentralProxy("127.0.0.1", 30000, "15.164.117.187", 30000); //QA server
            centralProxy.Start();

            Console.ReadLine();
        }
    }
}