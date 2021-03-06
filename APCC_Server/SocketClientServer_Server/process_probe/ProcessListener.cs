﻿using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace SocketClientServer_Server
{
    internal static class ProcessListener
    {

        private static Model model = Model.singleton;
        private static int latency; // la fréquence (en ms) a laquelle vérifier le cout des process
        
        internal static void RunThread(int latency)
        {
            ProcessListener.latency = latency;

            // DEBUT BOUCLE TOUR
            while (true)
            {

                float cpuUsage = 0;
                float memAvailable = 0;

                // GLOBAL PERFORMANCES
                /// CPU usage
                using (PerformanceCounter proCpuCounter = new PerformanceCounter())
                {
                    proCpuCounter.CategoryName = "Processor";
                    proCpuCounter.CounterName = "% Processor Time";
                    proCpuCounter.InstanceName = "_Total";

                    dynamic firstValue = proCpuCounter.NextValue();
                    System.Threading.Thread.Sleep(1000);
                    // now matches task manager reading
                    cpuUsage = proCpuCounter.NextValue();
                    //Console.WriteLine("[DEBUG] [PROCESS LINTENER] process usage: " + cpuUsage);
                }

                /// MEMORY available
                using (PerformanceCounter proRamCounter = new PerformanceCounter("Memory", "Available MBytes"))
                {
                    memAvailable = proRamCounter.NextValue();
                    //Console.WriteLine("[DEBUG] [PROCESS LINTENER] memory available: " + memAvailable + "MB");
                }


                // EACH CLIENT PROCESS USAGE
                model.updateCpuValue(cpuUsage);
                Thread.Sleep(ProcessListener.latency);

            }

        }
    }
}