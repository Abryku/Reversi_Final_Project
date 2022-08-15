using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Reversi_Final_Project.Réseau
{
	public class ServerTcpController : Tcp
	{
		private TcpListener _serveur;
		private TcpClient _client;
		private NetworkStream _flux;

		/**
		 * <summary>Démarre le serveur</summary>
		 * <param name="config">Configuration du serveur dans un <see cref="IPEndPoint"/></param>
		 * <returns>true si la connexion a pu s'effectuer, false sinon</returns>
		 */
		public async Task<bool> ListenAsync(IPEndPoint config)
		{
			_serveur = new TcpListener(config);

			try
			{
				await Task.Run(() =>
				{
					_serveur.Start();
					_client = _serveur.AcceptTcpClient(); // accepte une connexion client
					_flux = _client.GetStream();
				});

				Console.WriteLine("Serveur TCP démarré");

				return true;
			}
			catch (Exception e)
			{
				Console.WriteLine(@"Impossible de démarrer le serveur TCP " + e.Message);

				return false;
			}
			
		}

		/**
		 * <summary>Lance l'écoute sur le serveur</summary>
		 * <returns>Les données wrappées dans le model T demandé, null si une erreur</returns>
		 */
		public async Task<T> ReceiveAsync<T>() where T : class
		{
			return await ReceiveAsync<T>(_flux);
		}

		/**
		 * <summary>Envoie un model au client</summary>
		 * <param name="data">Model qui implémente <see cref="IModelReseau"/> à envoyer</param>
		 * <returns>true si tout s'est bien passé, false sinon</returns>
		 */
		public async Task<bool> SendAsync(DataToExchange data)
		{
			return await SendAsync(data, _flux);
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
