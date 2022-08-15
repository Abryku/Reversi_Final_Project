using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Reversi_Final_Project.Réseau
{
		public class ServeurController
		{
			private const int Port = 32432;
			private UdpClient _broadcastServer;

			private bool _state = true;
			private readonly object cadenas = new object();

			public bool State
			{
				get => _state;
				set
				{
					lock (cadenas)
					{
						_state = value;
					}

					if (value)
					{
						Console.WriteLine(@"Server is ON");
					}
					else
					{
						_broadcastServer.Close();
						Console.WriteLine(@"Server is OFF");
					}
				}
			}

			public async Task ReceiveBroadCastAsync()
			{
				_broadcastServer = new UdpClient(Port);

				//var clientEp = new IPEndPoint(IPAddress.Any, 0);

				while (State)
				{
					UdpReceiveResult clientData;
					try
					{
						clientData = await _broadcastServer.ReceiveAsync();
						var clientRequest = Encoding.ASCII.GetString(clientData.Buffer);
						//Console.WriteLine(@"Recived {0} from {1}, sending response", clientRequest, clientEp.Address);
					}
					catch (Exception e)
					{
						Console.WriteLine(e.Message);
					}
				}
			}
		}
}
