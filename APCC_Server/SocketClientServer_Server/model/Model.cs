﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace SocketClientServer_Server
{
    class Model
    {
        // Singleton
        public static Model singleton { get; private set; }

        // Model data
        public List<BoClient> clients { get; private set; }
        public int marge { get; private set; }
        public int port { get; private set; }
        public int latency { get; private set; }

        public bool clientListHasChange { get; set; }

        internal float cpuLoad { get; private set; }
        public bool cpuLimitExceeded { get; set; }
        public bool cpuFreeZone { get; set; }
        private bool clientListChangeForAPOC;

        public IPAddress adrLocal { get; set; }
        
        
        public Model(int marge, int port, int latency)
        {
            if (Model.singleton == null)
                Model.singleton = this;
            else
                throw new Exception();
            
            this.marge = marge;
            this.port = port;
            this.latency = latency;
            this.clients = new List<BoClient>();
            this.cpuLimitExceeded = false;
            this.cpuFreeZone = false;
        }

        internal List<BoClient> hasClientListChangeForAPOC()
        {
            if (clientListChangeForAPOC)
            {
                clientListChangeForAPOC = false;
                return new List<BoClient>(clients.ToArray()); ;
            } 
            return null;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public BoClient newClient(TcpClient tcp, Boolean type, int priority, int probe, string proName, string proDescription)
        {
            BoClient nClient = new BoClient(tcp, type, priority, probe, proName, proDescription);
            this.clients.Add(nClient);
            clientListHasChange = true;
            clientListChangeForAPOC = true;
            return nClient;
        }

        public void removeClient(TcpClient tcp)
        {
            clients.Remove(clients.Find(x => x.tcp == tcp));
            clientListHasChange = true;
            clientListChangeForAPOC = true;
            Console.WriteLine("[INFO] [MODEL] Removed client from list");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        internal void updateCpuValue(float cpuUsage)
        {
            cpuLoad = cpuUsage;
            //Console.WriteLine("[INFO] [MODEL] MAJ de la consommation CPU " + cpuUsage);

            // if cpu to high
            if (cpuUsage > (100-this.marge))
            {
                cpuLimitExceeded = true;
                Console.WriteLine("[WARNING] [MODEL] CPU IN RED ZONE !");
            }
            else if (cpuUsage < (100-this.marge-20))
            {
                cpuLimitExceeded = false;
                cpuFreeZone = true;
                //Console.WriteLine("[INFO] [MODEL] CPU IN GREEN ZONE !");
            } else
            {
                cpuLimitExceeded = false;
                cpuFreeZone = false;
            }
        }
    }
}
