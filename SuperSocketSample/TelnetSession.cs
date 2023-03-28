using SuperSocket.SocketBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase.Protocol;

namespace SuperSocketSample
{
   public class TelnetSession : AppSession<TelnetSession,RequestInfo>
    {
        protected override void OnSessionStarted()
        {
            //this.Send("connected with session Id: " + this.SessionID);
            Console.WriteLine("Client Connected with session Id: " + this.SessionID);
           // base.OnSessionStarted();
        }
        protected override void HandleUnknownRequest(RequestInfo requestInfo)
        {
            this.Send("This request cannot be processed");
        }
        protected override void HandleException(Exception e)
        {
            Console.WriteLine("Application error detected : {0}", e.ToString());
        }

        protected override void OnSessionClosed(CloseReason reason)
        {
            Console.WriteLine("Session {0} Disconnected, reason {1}",this.SessionID,reason.ToString());
            //this.Connected
            base.OnSessionClosed(reason);   
        }


        public DateTime lastSeen { get; set; }

    }
}
