using System;
using System.Collections.Generic;
using System.Net;

namespace WebServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //List<Server> servers = new List<Server>();
            //while (true)
            //{
            //    Console.WriteLine("1 Create Server 2 Start Server 3 Stop Server 4 Exit\t");
            //    switch (Console.ReadLine())
            //    {
            //        case "1":
            //            break;
            //        case "2":
            //            break;
            //        case "3":
            //            break;
            //        case "4":
            //            break;
            //        default:
            //            break;
            //    }
            //}

            ServerAdministrator serverAdministrator = new ServerAdministrator();
            string[] prefixes = new string[] { "http://localhost:1234/" };
            string rootDirectory = "C:\\Users\\gsanap\\Desktop\\";
            Server server = new Server(1,prefixes,rootDirectory);
            serverAdministrator.StartServer(server);

            string[] prefixes1 = new string[] { "http://localhost:12345/" };
            string rootDirectory1 = "C:\\Users\\gsanap\\Desktop\\Temp\\";
            Server server1 = new Server(2,prefixes1, rootDirectory1);
            serverAdministrator.StartServer(server1);

            if (Console.ReadLine() == "stop")
                serverAdministrator.StopServer(server);
            if (Console.ReadLine() == "stop")
                serverAdministrator.StopServer(server1);
            Console.ReadKey();
        }
    }
}
