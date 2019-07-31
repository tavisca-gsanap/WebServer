using System;
using System.Net;

namespace WebServer
{
    public class Server
    {
        public int Id { get; }
        public string[] Prefixes { get; }
        public string RootDirectory { get; }
        public HttpListener Listener { get; set; } 

        public Server(int id, string[] prefixes, string rootDirectory)
        {
            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");
            this.Id = id;
            this.Prefixes = prefixes;
            this.RootDirectory = rootDirectory;
        }
    }
}