using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Reversi_Final_Project.Réseau
{
	public class ServerTcpController : Tcp
	{
		private TcpListener _serveur;
		private TcpClient _client;

		/**
		 * <summary>Démarre le serveur</summary>
		 * <param name="config">Configuration du serveur dans un <see cref="IPEndPoint"/></param>
		 * <returns>true si la connexion a pu s'effectuer, false sinon</returns>
		 */
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

		/**
		 * <summary>Envoie un model au client</summary>
		 * <param name="data">Model qui implémente <see cref="Board"/> à envoyer</param>
		 * <returns>true si tout s'est bien passé, false sinon</returns>
		 */
		public async Task SendAsync(Board data)
		{
			await SendAsync(data, _flux);
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
