using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Reversi_Final_Project.Réseau
{
	public class ClientTcpController : Tcp
	{
		private readonly TcpClient _client;

		public ClientTcpController()
		{
			_client = new TcpClient();
		}

		/**
		 * <summary>Se connecte à un serveur</summary>
		 * <param name="serveur">Informations du serveur auquel se connecter</param>
		 */
		public async Task ConnectAsync(IPEndPoint serveur)
		{
			await _client.ConnectAsync(serveur.Address, serveur.Port);
			_flux = _client.GetStream();
		}

		public void Close()
		{
			_client.Close();
		}
	}
}
