using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;
using System.Net;


namespace iMacrosPostingDashboard
{
    class Httpcalls
    {
        public Httpcalls()
        { 
        }

        public bool IsAliveOLD(string proxy, ref object response)
        {
            //
            Ping ping = new Ping();

            try
            {
                PingReply reply = ping.Send(proxy, 2000);
                if (reply == null)
                {
                    response = reply.Status.ToString();
                    return false;
                }
                response = reply.Status.ToString();
                return (reply.Status == IPStatus.Success);
                
            }
            catch (PingException e)
            {
                response = e.InnerException.Message;
                return false;
            }
            
        }

        public bool IsAlive(string proxy)
        {
            
            string[] parts = proxy.Split(':');
            //string Host = parts[0];
            //string Port = parts[1];
            try
            {
                WebClient wc = new WebClient();
                wc.Proxy = new WebProxy(proxy);
                wc.DownloadString("http://google.com/");
                return true;
            }
            catch
            {

                return false;
            }

        }

    }
}
