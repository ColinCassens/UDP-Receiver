using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UDPReceiver
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("UDP RECEIVER");
            UdpClient uc = new UdpClient();
            uc.ExclusiveAddressUse = false;
            IPEndPoint localEP = new IPEndPoint(IPAddress.Any, 49552);
            uc.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            uc.Client.Bind(localEP);
            IPAddress multiadress = IPAddress.Parse("239.0.0.222");
            uc.JoinMulticastGroup(multiadress);
            UdpState state = new UdpState();
            state.client = uc;
            state.endpoint = localEP;
            uc.BeginReceive(new AsyncCallback(ReceiveCallback), state);
            Console.ReadLine();
        }

        private struct UdpState
        {
            public UdpClient client;
            public IPEndPoint endpoint;
        }

        public static void ReceiveCallback(IAsyncResult ar)
        {
            UdpClient uc = ((UdpState)(ar.AsyncState)).client;
            IPEndPoint ep = ((UdpState)ar.AsyncState).endpoint;
            byte[] Bytes = uc.EndReceive(ar, ref ep);
            UdpState s = new UdpState();
            s.client = uc;
            s.endpoint = ep;
            uc.BeginReceive(new AsyncCallback(ReceiveCallback), s);
            string encoded = Encoding.Unicode.GetString(Bytes);
            //string decoded = encryptDecrypt(encoded);
            Console.WriteLine(encoded);
        }

    }
}
