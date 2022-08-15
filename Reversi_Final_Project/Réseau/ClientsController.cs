using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Reversi_Final_Project.Réseau
{
    class ClientsController
    {
		private const int Port = 32432;

		public async Task<PingReply> PingAsync(string ip)
		{
			return await new Ping().SendPingAsync(ip, 100);
		}

		public Task<string> PingTask(string ip)
		{
			//todo changer en une req custom pour que les serveurs répondent
			return Task.Run(() => PingAsync(ip)).ContinueWith(task =>
			{
				//IPHostEntry repEntry = null;

				string res = null;

				if (task.Result.Status == IPStatus.Success)
					res = task.Result.Address.ToString();
				//repEntry = Dns.GetHostEntry(task.Result.Address);

				return res;
			}
			);
		}

		public async Task BroadCastAsync()
		{
			var client = new UdpClient();
			var requestData = Encoding.ASCII.GetBytes("Reversi" + Environment.MachineName);
			var serverEp = new IPEndPoint(IPAddress.Any, 0);

			client.EnableBroadcast = true;
			await client.SendAsync(requestData, requestData.Length, new IPEndPoint(IPAddress.Broadcast, Port));

			/*var serverResponseData = await client.ReceiveAsync();
			var serverResponse = Encoding.ASCII.GetString(serverResponseData.Buffer);
			Console.WriteLine(@"Recived {0} from {1}", serverResponse, serverEp.Address);*/

			client.Close();
		}
	}
}
