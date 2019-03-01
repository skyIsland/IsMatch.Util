using NetworkSocket;
using NetworkSocket.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace IsMatch.NetworkSocket
{
    public class Startup
    {
        public static void Main()
        {
            var listener = new TcpListener();
            listener.Use<HttpMiddleware>();
            listener.Start(1212);
        }
    }
}
