using SuperSocket.SocketBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Protocol;

namespace SuperSocketSample
{
    public class MyTelnetServer : AppServer<TelnetSession, RequestInfo>
    {
        Timer _timer = new Timer();
        public MyTelnetServer() : base(new ReceiveFilterFactory())
        {
            _timer.Enabled = true;
            _timer.Interval = 1000 * 60 * 5;
            _timer.Elapsed += _timer_Elapsed;
            this.NewRequestReceived += TelnetServer_NewRequestReceived;
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var sess = this.GetAllSessions();
            var dt = DateTime.Now;
            Console.WriteLine("Timer here: NOS:{0} at {1}", sess.Count(), dt.ToString());
            try
            {
                _timer.Enabled = false;

                foreach (var item in sess)
                {
                    //var mn = item.LastActiveTime.Subtract(dt);
                    //if (mn.Minutes>5)
                    //{
                    //}
                    if (item.TrySend("."))
                    {
                        Console.WriteLine("Keep Alive Sent");
                    }
                    else
                    {
                        Console.WriteLine("Error sending Keep Alive");
                        item.Close();
                        Console.WriteLine("SessionClosed");
                    }

                }
                _timer.Enabled = true;
            }
            catch (Exception x)
            {
                Console.WriteLine("Error sending Keep Alive");
                x.ToString();
                _timer.Enabled = true;
                //throw;
            }
        }

        private void TelnetServer_NewRequestReceived(TelnetSession session, RequestInfo requestInfo)
        {
            
            if (string.IsNullOrEmpty(requestInfo.Content))
            {
                Console.WriteLine("Current time {0}", DateTime.Now.ToString("dd-MMM-yyy hh mm ss fff tt"));
                Console.WriteLine("Byte content {0}", requestInfo.ByteContent.Length);
                session.Send(requestInfo.ByteContent, 0, requestInfo.ByteContent.Length);
            }
            else
            {
                Console.WriteLine(requestInfo.Content);
                Console.WriteLine("Current time {0}", DateTime.Now.ToString("dd-MMM-yyy hh mm ss fff tt"));
                Console.WriteLine("Last Active time {0}", session.LastActiveTime.ToString("dd-MMM-yyy hh mm ss fff tt"));

                byte[] response = ASCIIEncoding.ASCII.GetBytes(requestInfo.Content);

                session.Send(response, 0, response.Length);

            }
        }

        protected override bool Setup(IRootConfig rootConfig, IServerConfig config)
        {
            return base.Setup(rootConfig, config);
        }

        protected override void OnStarted()
        {
            base.OnStarted();
        }
        protected override void OnStopped()
        {
            base.OnStopped();
        }
        protected override void OnNewSessionConnected(TelnetSession session)
        {
            base.OnNewSessionConnected(session);
        }
    }
}
