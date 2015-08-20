using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DewDedi
{
    class rcon
    {
        public static string DewCmd(string cmd)
        {
            var data = new byte[1024];
            string stringData;
            TcpClient server;
            try
            {
                server = new TcpClient("127.0.0.1", 2448);
            }
            catch (SocketException)
            {
                return "Is Eldorito Running?";
            }
            var ns = server.GetStream();

            var recv = ns.Read(data, 0, data.Length);
            stringData = Encoding.ASCII.GetString(data, 0, recv);

            ns.Write(Encoding.ASCII.GetBytes(cmd), 0, cmd.Length);
            ns.Flush();

            ns.Close();
            server.Close();
            return stringData;
        }
    }
}
