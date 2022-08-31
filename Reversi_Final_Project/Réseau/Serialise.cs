using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Reversi_Final_Project.Réseau
{
	public static class Serialise
	{
		//Transforme un objet en tableau de byte pour préparer a l'envoi
		public static byte[] ObjectToByteArray(object obj)
		{
			BinaryFormatter bf = new BinaryFormatter();
			using (var ms = new MemoryStream())
			{
				bf.Serialize(ms, obj);
				return ms.ToArray();
			}
		}
		//Convertis les bits recus en board
		public static Board MemoryStreamToObject(MemoryStream stream)
		{
			var binForm = new BinaryFormatter();

			stream.Seek(0, SeekOrigin.Begin);
			var obj = binForm.Deserialize(stream);

			return obj as Board;
		}
	}
}
