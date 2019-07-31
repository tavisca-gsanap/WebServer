using System;
using System.Collections.Generic;
using System.Text;

namespace WebServer
{
    public interface IStatusCode
    {
        int Code { get; }
        string Message { get; }
    }

    public class Code404 : IStatusCode
    {
        public int Code => 404;

        public string Message => "<html><body><h1>Error 404 file not found</h1></body></html>";
    }
}
