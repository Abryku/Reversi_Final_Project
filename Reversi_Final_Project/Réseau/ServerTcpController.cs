using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Reversi_Final_Project.Réseau
{
	//Gestion serveur
	public class ServerTcpController : Tcp
	{
		private TcpListener _serveur;
		private TcpClient _client;

		//Démarrage et configuration du serveur
		public async Task ListenAsync(IPEndPoint config)
		{
			_serveur = new TcpListener(config);

			await Task.Run(() =>
			{
				_serveur.Start();
				_client = _serveur.AcceptTcpClient(); // accepte une connexion client
				_flux = _client.GetStream();
			});

		}

		public void Close()
		{
			_client?.GetStream().Close();
			_client?.Close();
			_flux?.Close();
			_serveur?.Stop();
		}
	}
}
