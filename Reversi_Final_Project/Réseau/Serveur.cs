using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Reversi_Final_Project.Réseau
{
    class Serveur : Réseau
    {
      /*  private Thread Ecoute;
        private UdpClient broadcast;
        private bool continuer;

        public Serveur()
        {
            clientOuServeur = Serveur;

            //Initialisation et préparation des objets nécessaires au serveur.
            broadcast = new UdpClient();
            broadcast.EnableBroadcast = true;
            broadcast.Connect(new IPEndPoint(IPAddress.Broadcast, 5053));

            //Démarrage du thread d'écoute.
            continuer = true;
            Ecoute = new Thread(Ecouter);
            Ecoute.Start();
        }

        private void Ecouter()
        {
            // Création d'un Socket qui servira de serveur
            UdpClient serveur = null;
            bool erreur = false;
            int attempts = 0;

            // J'essaie 3 fois car je veux éviter un plantage au serveur à cause de quelques millisecondes
            do
            {
                try
                {
                    serveur = new UdpClient(1523);
                }
                catch
                {
                    erreur = true;
                    attempts++;
                    Thread.Sleep(400);
                }
            } while (erreur && attempts < 4);

            //Si c'est vraiment impossible de se lier, on quitte le thread.
            if (serveur == null)
            {
                Ferme();
                return;
            }

            serveur.Client.ReceiveTimeout = 1000;

            // tant que le serveur est actif
            while (continuer)
            {
                try
                {
                    IPEndPoint ip = null;
                    byte[] data = serveur.Receive(ref ip);

                    MessageBox.Show(Encoding.UTF8.GetString(data));

                    //Préparation des données à l'aide de la classe interne
                    CommunicationData communicationData = new CommunicationData(ip, data);
                    //On lance un nouveau thread avec les données en paramètre
                    new Thread(TraiterMessage).Start(communicationData);
                }
                catch { }
            }

            serveur.Close();
        }

        public override void Ferme()
        {
            continuer = false;

            //erveur.Close();
            clientOuServeur = !clientOuServeur;

            if (Ecoute != null && Ecoute.ThreadState == ThreadState.Running)
                Ecoute.Join();
        }

        /// Méthode en charge de traiter un message entrant.
        private void TraiterMessage(object messageArgs)
        {
            try
            {
                //On récupère les données entrantes et on les formatte comme il faut.
                CommunicationData data = messageArgs as CommunicationData;
                string message = string.Format("{0}:{1} > {2}", data.Client.Address, data.Client.Port, Encoding.Default.GetString(data.Data));

                //On renvoie le message formatté à travers le réseau.
                byte[] donnees = Encoding.Default.GetBytes(message);
                broadcast.Send(donnees, donnees.Length);
            }
            catch { }
        }

        //Définition d'une classe interne privée pour faciliter l'échange de
        //données entre les threads.
        private class CommunicationData
        {
            public IPEndPoint Client { get; private set; }

            public byte[] Data { get; private set; }

            public CommunicationData(IPEndPoint client, byte[] data)
            {
                Client = client;
                Data = data;
            }
        }*/
    }
}
