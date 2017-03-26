using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using StompNet;

namespace NemApi.Async
{
    public class WebsocketConnector
    {
        public WebsocketConnector(string path)
        {
            UriBuilder UriBuilder = new UriBuilder()
            {
                Port = 7778

            };
            Con = new Connection(UriBuilder);
            this.Path = Path;
            Con.SetTestNet();
            
        }

        private Connection Con { get; }

        private string Path { get; }

        // Example: Send three messages before receiving them back.
         async Task ReadmeExample()
        {
            // Establish a TCP connection with the STOMP service.
            using (TcpClient tcpClient = new TcpClient())
            {
                await tcpClient.ConnectAsync(Con.GetHost(), Con.Uri.Port);

                //Create a connector.
                using (IStompConnector stompConnector =
                    new Stomp12Connector(
                        tcpClient.GetStream(),
                        Con.GetHost()))
                {
                    // Create a connection.
                    IStompConnection connection = await stompConnector.ConnectAsync();

                    // Send a message.
                    await connection.SendAsync("/w/api/account/subscribe?address=TALICE6KJ2SRSIJFVVFFH6ICUIYZ2ZZGNFUDJGRT");
                    

                    //// Send two messages using a transaction.
                    //IStompTransaction transaction = await connection.BeginTransactionAsync();
                    //await transaction.SendAsync("/queue/example", "Hi!");
                    //await transaction.SendAsync("/queue/example", "My name is StompNet");
                    //await transaction.CommitAsync();

                    // Receive messages back.
                    // Message handling is made by the ConsoleWriterObserver instance.
                    var a = new ConsoleWriterObserver();
                    await connection.SubscribeAsync(
                        a, Path);

                    // Wait for messages to be received.
                    await Task.Delay(250);

                    // Disconnect.
                    await connection.DisconnectAsync();
                }
            }


        }

        class ConsoleWriterObserver : IObserver<IStompMessage>
        {

            // Incoming messages are processed here.
            public void OnNext(IStompMessage message)
            {
                Console.WriteLine("MESSAGE: " + message.GetContentAsString());

                if (message.IsAcknowledgeable)
                    message.Acknowledge();

                Console.ReadLine();
            }

            // Any ERROR frame or stream exception comes through here.
            public void OnError(Exception error)
            {
                Console.WriteLine("ERROR: " + error.Message);
            }

            // OnCompleted is invoked when unsubscribing.
            public void OnCompleted()
            {
                Console.WriteLine("UNSUBSCRIBED!");
            }
        }
    }
}
