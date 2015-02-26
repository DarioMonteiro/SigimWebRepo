using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GIR.Sigim.Infrastructure.Crosscutting.Test
{
    public class CustomWorkerRequest : HttpWorkerRequest
    {
        public override void EndOfRequest()
        {
            throw new NotImplementedException();
        }

        public override void FlushResponse(bool finalFlush)
        {
            throw new NotImplementedException();
        }

        public override string GetHttpVerbName()
        {
            throw new NotImplementedException();
        }

        public override string GetHttpVersion()
        {
            throw new NotImplementedException();
        }

        public override string GetLocalAddress()
        {
            throw new NotImplementedException();
        }

        public override int GetLocalPort()
        {
            throw new NotImplementedException();
        }

        public override string GetQueryString()
        {
            throw new NotImplementedException();
        }

        public override string GetRawUrl()
        {
            return "/localhost";
        }

        public override string GetRemoteAddress()
        {
            throw new NotImplementedException();
        }

        public override int GetRemotePort()
        {
            throw new NotImplementedException();
        }

        public override string GetUriPath()
        {
            throw new NotImplementedException();
        }

        public override void SendKnownResponseHeader(int index, string value)
        {
            throw new NotImplementedException();
        }

        public override void SendResponseFromFile(IntPtr handle, long offset, long length)
        {
            throw new NotImplementedException();
        }

        public override void SendResponseFromFile(string filename, long offset, long length)
        {
            throw new NotImplementedException();
        }

        public override void SendResponseFromMemory(byte[] data, int length)
        {
            throw new NotImplementedException();
        }

        public override void SendStatus(int statusCode, string statusDescription)
        {
            throw new NotImplementedException();
        }

        public override void SendUnknownResponseHeader(string name, string value)
        {
            throw new NotImplementedException();
        }
    }
}