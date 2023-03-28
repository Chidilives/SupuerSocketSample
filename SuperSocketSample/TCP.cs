
using SuperSocket.SocketBase;
using SuperSocket.SocketEngine;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace SuperSocketSample
{
    public class TCPImplementation : AppServer<TelnetSession, RequestInfo>
    {
        public TCPImplementation()
        {
            Tcp();
        }      
        public void Tcp()
        {
            var bootstrap = BootstrapFactory.CreateBootstrap();

           // bootstrap.Config
            if (!bootstrap.Initialize())
            {
                Console.WriteLine("Failed to initialize!");
                Console.ReadKey();
                return;
            }

            var result = bootstrap.Start();

           //var servers =  bootstrap.AppServers.ToList();
            Console.WriteLine("Start Result : {0}", result);

            if (result == StartResult.Failed)
            {
                Console.WriteLine("Failed to start!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine();



            Console.WriteLine("The server started successfully, press key 'q' to stop it! \r\n To Generate Pass Key for a Device,\r\n Please Enter the PublicKey space the Pass You want to Use ");
            var vr = Console.ReadLine();
            while (vr != "q")
            {
                //Password.Generate(vr);
                Console.WriteLine("You Can setup another Device");
                vr = Console.ReadLine();

                Console.WriteLine();
                //continue;
            }

            //Stop the appServer
            bootstrap.Stop();
            // appServer.Stop();

            Console.WriteLine("The server was stopped!");
            Console.ReadKey();
        }
    }
}
