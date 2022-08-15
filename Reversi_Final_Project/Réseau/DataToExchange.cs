using System;

namespace Reversi_Final_Project.Réseau
{
	[Serializable]
	public class DataToExchange
	{
		public Board Board { get; set; }
		public Command Command { get; set; }
	}
}