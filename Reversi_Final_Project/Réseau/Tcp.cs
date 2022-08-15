using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Reversi_Final_Project.Réseau
{
	public class Tcp
	{
		protected NetworkStream _flux;
		
		/**
		 * <summary>Lance l'écoute sur le serveur</summary>
		 * <returns>Les données wrappées dans le model T demandé, null si une erreur</returns>
		 */
		public async Task<Board> ReceiveAsync()
		{
			return await ReceiveAsync(_flux);
		}
		
		/**
		 * <summary>Lance l'écoute sur le serveur</summary>
		 * <returns>Les données wrappées dans le model T demandé, null si une erreur</returns>
		 */
		private async Task<Board> ReceiveAsync(NetworkStream flux)
		{
			Board board = null;
			
			await Task.Run(() =>
			{
				byte[] buffer = new byte[2048];
				using (MemoryStream ms = new MemoryStream())
				{
					int numBytesRead = flux.Read(buffer, 0, buffer.Length);
					while (flux.DataAvailable || numBytesRead > 0)
					{
						if(numBytesRead == 0)
							numBytesRead = flux.Read(buffer, 0, buffer.Length);

						ms.Write(buffer, 0, numBytesRead);
						numBytesRead = 0;
					}

					board = Serialise.MemoryStreamToObject(ms); // converti les octets en un model demandé
				}
			});

			return board;
		}
		
		/**
		 * <summary>Envoie un model au client</summary>
		 * <param name="board">Model qui implémente <see cref="Board"/> à envoyer</param>
		 * <returns>true si tout s'est bien passé, false sinon</returns>
		 */
		public async Task SendAsync(Board board)
		{
			await SendAsync(board, _flux);
		}

		/**
		 * <summary>Envoie un model au client</summary>
		 * <param name="board">Model qui implémente <see cref="IModelReseau"/> à envoyer</param>
		 * <param name="flux">Flux <see cref="NetworkStream"/> à envoyer</param>
		 * <returns>true si tout s'est bien passé, false sinon</returns>
		 */
		protected async Task SendAsync(Board board, NetworkStream flux)
		{
			await Task.Run(() =>
			{
				BinaryWriter binaryWriter = new BinaryWriter(flux); // converti le flux en binaire

				byte[] byteData = Serialise.ObjectToByteArray(board);

				binaryWriter.Write(byteData); // envoie le model sous forme de bytes[]
			});
		}
	}
}
