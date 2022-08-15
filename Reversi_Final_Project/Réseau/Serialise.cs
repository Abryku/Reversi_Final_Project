using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Reversi_Final_Project.Réseau
{
	public static class Serialise
	{
		/**
		 * <summary>Converti un objet en un tableau de byte</summary>
		 * <param name="obj">Objet à sérialiser</param>
		 * <returns>L'objet sérialisé</returns>
		 */
		public static byte[] ObjectToByteArray(object obj)
		{
			BinaryFormatter bf = new BinaryFormatter();
			using (var ms = new MemoryStream())
			{
				bf.Serialize(ms, obj);
				return ms.ToArray();
			}
		}

		public static Board MemoryStreamToObject(MemoryStream stream)
		{
			var binForm = new BinaryFormatter();

			stream.Seek(0, SeekOrigin.Begin);
			var obj = binForm.Deserialize(stream);

			return obj as Board;
		}
	}
}
