using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Reversi_Final_Project.Réseau
{
	//Gestion client
	public class ClientTcpController : Tcp
	{
		private readonly TcpClient _client;

		public ClientTcpController()
		{
			_client = new TcpClient();
		}

		//Fonction de connexion au serveur
		public async Task ConnectAsync(IPEndPoint serveur)
		{
			await _client.ConnectAsync(serveur.Address, serveur.Port); //adresseIP et le port en argument
			_flux = _client.GetStream();
		}

		public void Close()
		{
			_client.Close();
		}
	}
}
