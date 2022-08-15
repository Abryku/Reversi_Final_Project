using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reversi_Final_Project.Réseau
{
	public class NetworkUtilities
	{
		private ClientTcpController client;
		private ServerTcpController serverTcp;
		
		private int ServerPort => 1010;
		public bool IsClient => client != null;

		public async Task StartServerAsync()
		{
			try
			{
				serverTcp = new ServerTcpController();
				await serverTcp.ListenAsync(new IPEndPoint(GetLocalIpAddress(), ServerPort));
			}
			catch
			{
				MessageBox.Show(@"Unable to start server");
			}
		}
		
		public async Task<Board> StartClientAsync()
		{
			try
			{
				client = new ClientTcpController();
				await client.ConnectAsync(new IPEndPoint(GetLocalIpAddress(), ServerPort));
			}
			catch
			{
				MessageBox.Show(@"Unable to connect to server");
			}

			try
			{
				return await client.ReceiveAsync();
			}
			catch
			{
				MessageBox.Show(@"Unable to receive data from server");
			}

			return null;
		}

		public async Task InitializeServerAsync(Board board)
		{
			try
			{
				if(serverTcp != null) await serverTcp.SendAsync(board);
			}
			catch
			{
				MessageBox.Show(@"Unable to initialize server");
			}
		}

		public async Task SendAsync(Board board)
		{
			try
			{
				await GetPlayerRole().SendAsync(board);
			}
			catch (Exception e)
			{
				MessageBox.Show(@"Unable to send data to server");
			}
		}

		public async Task<Board> ReceiveAsync()
		{
			try
			{
				return await GetPlayerRole().ReceiveAsync();
			}
			catch (Exception e)
			{
				MessageBox.Show(@"Unable to receive data from server");
			}

			return null;
		}

		private Tcp GetPlayerRole()
		{
			if (IsClient)
			{
				return client;
			}
            
			return serverTcp;
		}
		
		private static IPAddress GetLocalIpAddress()
		{
			var host = Dns.GetHostEntry(Dns.GetHostName());

			return host.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
		}
	}
}