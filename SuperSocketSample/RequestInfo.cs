using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase;
using System.Net;
using System.IO;
//using Newtonsoft.Json;
using System.Threading;
using System.Collections;

namespace SuperSocketSample
{
    /// <summary>
    /// Incoming Request
    /// </summary>
    public class RequestInfo : IRequestInfo
    {

        public RequestInfo(string content,string deviceId)
        {
            //Key = key;
            Content = content;
            DeviceId = deviceId;
        }
        public RequestInfo(byte[] content, string deviceId)
        {

            ByteContent = content;
            DeviceId = deviceId;
        }

        //public CommandType Protocol { get; set; }  //This is the protocol Enum
        public string Key { get; set; } //this is the protocol number
        public string Content { get; set; }
        public byte[] ByteContent { get; set; }
        public string SerialNumber { get; set; }
        public int Packetlength { get; set; }
        public string DeviceId { get; set; }


    }

    public class RequestFilter : IReceiveFilter<RequestInfo>
    {
        public RequestInfo Filter(byte[] readBuffer, int offset, int length, bool toBeCopied, out int rest)
        {

            rest = 0;
            
            byte[] bbb = new byte[length];
            Array.Copy(readBuffer, offset, bbb, 0, length);

            var s = Encoding.ASCII.GetString(bbb, 0, length);
           
            String sData = null;

            
            sData = s;

            // shows content on the console.
            Console.WriteLine("Client &gt; \"" + s + "\" \r\n" + DateTime.Now.ToString("dd-MMM-yyy hh mm ss fff tt"));
           
            //Push this into a different Thread
            if (sData.Length < 4)
            {
                s = ".";
                Thread.Sleep(10);
            }
            else
            {
                string responseData ;

                Console.WriteLine("call process data");
                var returnedData = EndpointServiceProcessData(sData, "TCP", out responseData);
                Console.WriteLine("after call process data");
                if (responseData=="download")
                {
                    return new RequestInfo((byte[])returnedData, "DeviceId");
                }
                s = (string)returnedData;
                return new RequestInfo(s + "\r\n", "DeviceId");
            }
            return new RequestInfo(s + "\r\n","");
        }

        private object EndpointServiceProcessData(string sData, string v, out string responseData)
        {
            //here you push all the data to the apppriopriate methods for processing
            Console.WriteLine($"processed {sData}");
            responseData = ".";
            return $"processed {sData}";
        }

        public IReceiveFilter<RequestInfo> NextReceiveFilter
        {
            get { return this; }
        }
        public FilterState State { get; private set; }

        public int LeftBufferSize { get;  set; }
        

        public void Reset()
        {

        }


    }


    public class ReceiveFilterFactory : IReceiveFilterFactory<RequestInfo>
    {
        public IReceiveFilter<RequestInfo> CreateFilter(IAppServer appServer, IAppSession appSession, IPEndPoint remoteEndPoint)
        {
            return new RequestFilter();
        }
    }

}
public class BinaryRequestInfo :IRequestInfo
{
    public string Key { get; }

    public byte[] Body { get; }
}
