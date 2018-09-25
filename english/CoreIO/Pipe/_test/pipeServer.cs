using host.pipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.IO.Pipes
{
    public class pipeServer
    {
        public static void init(int id)
        {
            var server = new NamedPipeServer<byte[]>(id.ToString());
            server.ClientConnected += OnClientConnected;
            server.ClientDisconnected += OnClientDisconnected;
            server.ClientMessage += OnClientMessage;
            server.Error += OnError;
            server.Start();
        }

        private static void OnClientConnected(NamedPipeConnection<byte[], byte[]> connection)
        {
            Console.WriteLine("Client {0} is now connected!", connection.Id);
            connection.PushMessage(new byte[] { 99 });
        }

        private static void OnClientDisconnected(NamedPipeConnection<byte[], byte[]> connection)
        {
            Console.WriteLine("Client {0} disconnected", connection.Id);
        }

        private static void OnClientMessage(NamedPipeConnection<byte[], byte[]> connection, byte[] message)
        {
            //main.show_notification(string.Format("Client {0} send: {1}", connection.Id, message.Length.ToString()));
        }

        private static void OnError(Exception exception)
        {
            Console.Error.WriteLine("ERROR: {0}", exception);
        }

    }//end class
}
