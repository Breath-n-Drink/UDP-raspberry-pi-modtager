using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UDP_raspberry_pi_modtager
{
    class Program
    {
        // https://msdn.microsoft.com/en-us/library/tst0kwb1(v=vs.110).aspx
        // IMPORTANT Windows firewall must be open on UDP port 7000
        // https://www.windowscentral.com/how-open-port-windows-firewall
        // Use the network MGV-xxx to capture from local IoT devices (fake or real)
        private const int Port = 9031;
        //private static readonly IPAddress IpAddress = IPAddress.Parse("192.168.5.137"); 
        // Listen for activity on all network interfaces
        // https://msdn.microsoft.com/en-us/library/system.net.ipaddress.ipv6any.aspx

        public static Promille prom;


        static void Main()
        {

            using var db = new BreathndrinkContext();

            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, Port);
            using (UdpClient socket = new UdpClient(ipEndPoint))
            {
                IPEndPoint remoteEndPoint = new IPEndPoint(0, 0);
                while (true)
                {
                    Console.WriteLine("Waiting for broadcast {0}", socket.Client.LocalEndPoint);
                    byte[] datagramReceived = socket.Receive(ref remoteEndPoint);

                    string message = Encoding.ASCII.GetString(datagramReceived, 0, datagramReceived.Length);
                    Console.WriteLine("Receives {0} bytes from {1} port {2} message {3}", datagramReceived.Length,
                        remoteEndPoint.Address, remoteEndPoint.Port, message);
                    Console.WriteLine(double.Parse(message, System.Globalization.CultureInfo.InvariantCulture));

                    Parse(message);
                }
            }
        }

        // To parse data from the IoT devices (depends on the protocol)
        private static void Parse(string response)
        {

            prom = new Promille() { DrinkerId = 1, Promille1 = double.Parse(response, System.Globalization.CultureInfo.InvariantCulture), Time = DateTime.Now };
            using (var context = new BreathndrinkContext())
            {
                context.Promille.Add(prom);
                context.SaveChanges();
            }

        }
    }
}
