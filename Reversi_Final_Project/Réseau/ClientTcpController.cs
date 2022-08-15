using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Reversi_Final_Project.Réseau
{
	public class ClientTcpController : Tcp
	{
		private readonly TcpClient _client;
		private NetworkStream _flux;

		public ClientTcpController()
		{
			_client = new TcpClient();
		}

		/**
		 * <summary>Se connecte à un serveur</summary>
		 * <param name="serveur">Informations du serveur auquel se connecter</param>
		 */
		public async Task<bool> ConnectAsync(IPEndPoint serveur)
		{
			try
			{
				await _client.ConnectAsync(serveur.Address, serveur.Port);
				_flux = _client.GetStream();
				Console.WriteLine($@"Connecté au serveur TCP {serveur.Address}:{serveur.Port}");

				return true;
			}
			catch (Exception e)
			{
				Console.WriteLine($@"Impossible de se connecter au serveur TCP {e.Message}");

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
			_client.Close();
		}
	}
}
