using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace WebServer
{
    public class ServerAdministrator
    {
        public void StartServer(Server server)
        {
            if(server.Listener != null)
            {
                if (server.Listener.IsListening)
                {
                    Console.WriteLine("Server {0} Already Running",server.Id);
                    return;
                }
            }
            else
            {
                string[] prefixes = server.Prefixes;

                // Create a listener.
                server.Listener = new HttpListener();
                // Add the prefixes.
                foreach (string prefix in prefixes)
                {
                    server.Listener.Prefixes.Add(prefix);
                }
            }
            Thread listenerThread = new Thread(new ThreadStart(() => this.StartListening(server)));
            listenerThread.Start();
        }
        public void StopServer(Server server)
        {
            if (server.Listener == null)
            {
                Console.WriteLine("Server{0} was never started",server.Id);
                return;
            }
            else
            {
                if (server.Listener.IsListening)
                {
                    server.Listener.Stop();
                    Console.WriteLine("Server{0} Stopped",server.Id);
                }
                else
                {
                    Console.WriteLine("Server{0} was stopped already",server.Id);
                }
            }
        }

        private void StartListening(Server server)
        {
            server.Listener.Start();
            Console.WriteLine("Server {0} Started Listening on {1}",server.Id, string.Join(", ", server.Prefixes));
            // Note: The GetContext method blocks while waiting for a request. 
            while (server.Listener.IsListening)
            {
                HttpListenerContext context = server.Listener.GetContext();
                Thread requestHandlerThread = new Thread(new ThreadStart(() => this.HandleRequest(server, context)));
                requestHandlerThread.Start();
            }
            Console.WriteLine("Server {0} Stopped Listening",server.Id);
        }
        private void HandleRequest(Server server,HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            //Console.WriteLine(request.Url);
            Console.WriteLine("Server {0} Got request for url {1}",server.Id, request.Url.OriginalString);
            string filePath = ParseRequest(server, request);
            string content = FileReader(server, filePath);

            //response.
            string responseString = content;//"<HTML><BODY> Hello world!</BODY></HTML>";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            // Get a response stream and write the response to it.
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            // You must close the output stream.
            output.Close();
        }
        
        private string ParseRequest(Server server,HttpListenerRequest request)
        {
            string filePath="";
            string url = request.Url.OriginalString;
            foreach (string prefix in server.Prefixes)
            {
                if (url.StartsWith(prefix, StringComparison.Ordinal) == true)
                {
                    int index = url.IndexOf(prefix);
                    filePath = (index < 0)
                        ? url
                        : url.Remove(index, prefix.Length);
                }
            }
            if (filePath.Equals("") || filePath.EndsWith('/'))
                return filePath+"index.html";
            return filePath;
        }

        private string FileReader(Server server,string filePath)
        {
            string content = "";
            try
            {
                FileStream fileStream = new FileStream(server.RootDirectory + filePath, FileMode.Open, FileAccess.Read);
                StreamReader streamReader = new StreamReader(fileStream);
                streamReader.BaseStream.Seek(0, SeekOrigin.Begin);
                string line = streamReader.ReadLine();
                while (line != null)
                {
                    //Console.WriteLine(str);
                    content += line;
                    line = streamReader.ReadLine();
                }
                //Console.ReadLine();
                streamReader.Close();
                fileStream.Close();
            }
            catch(FileNotFoundException fileNotFoundException)
            {
                content = new Code404().Message;
            }
            return content;
        }
    }
}
