using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace Reversi_Final_Project.Réseau
{
    class Clients : Réseau
    {
        private UdpClient client;

        private bool continuer;
        private Thread Ecoute;
        public void Client()
        {
            //On crée automatiquement le client qui sera en charge d'envoyer les messages au serveur
            client = new UdpClient();
            client.Connect("127.0.0.1", 1523);

            //Initialisation des objets nécessaires au client. On lance également le thread en charge d'écouter
            continuer = true;
            Ecoute = new Thread(ThreadEcouteur);
            Ecoute.Start();
        }

        /// Fonction en charge d'écouter les communications réseau
        private void ThreadEcouteur()
        {
            //Déclaration du Socket d'écoute.
            UdpClient ecouteur;

            //Création sécurisée du Socket.
            try
            {
                ecouteur = new UdpClient(5053);
            }
            catch
            {
                MessageBox.Show("Impossible de se lier au port UDP " + 5053 + ". Vérifiez vos configurations réseau.");
                return;
            }

            //Définition du Timeout.
            ecouteur.Client.ReceiveTimeout = 1000;

            //Bouclage infini d'écoute de port.
            while (continuer)
            {
                try
                {
                    IPEndPoint ip = null;
                    byte[] data = ecouteur.Receive(ref ip);
                }
                catch
                {
                }
            }

            ecouteur.Close();
        }

        public void Emettre(string message)
        {
            //Sérialisation du message en tableau de bytes.
            byte[] msg = Encoding.Default.GetBytes(message);

            //La méthode Send envoie un message UDP.
            client.Send(msg, msg.Length);

            //udpClient.Close();
        }

        public void Ferme()
        {
            //clientOuServeur = !clientOuServeur;

            continuer = false;
            client.Close(); // ferme la connexion
            Ecoute.Join(); // arrête le thread
        }
    }
}
