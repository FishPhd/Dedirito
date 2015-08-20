using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Microsoft.VisualBasic.Devices;

namespace DewDedi
{
    class statistics
    {
        //variables
        PerformanceCounter cpuCounter;
        PerformanceCounter ramCounter;
        public void StartTimer()
        {

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.IsEnabled = true;
            timer.Tick += Timer_Tick;
            //timer.Start();
        }

        public void Timer_Tick(object sender, EventArgs e)
        {
            ramCounter = new PerformanceCounter("Memory", "Available MBytes");

            //cpuUsage.Content = "Cpu Usage: " + Math.Round(CurrentCPUusage("Processor", "% Processor Time", "_Total"), 2, MidpointRounding.AwayFromZero) + "%";
            //ramUsage.Content = "Available Memory: " + Math.Round(ramCounter.NextValue() / 1024, 2, MidpointRounding.AwayFromZero) + " GB";
            //ramTotal.Content = "Total Memory: " + new ComputerInfo().TotalPhysicalMemory / 1024 / 1024 / 1024 + " GB";

            foreach (Process proc in Process.GetProcessesByName("eldorado"))
            {
                try
                {
                    double memory = CurrentCPUusage("Process", "% Processor Time", "eldorado");
                    //eldoritoCpuUsage.Content = "Eldorito Cpu Usage: " + Math.Round(memory, 2, MidpointRounding.AwayFromZero) + "%";
                }
                catch
                {
                    Console.WriteLine("Eldorito not open");
                }
            }

        }

        private double CurrentCPUusage(string category, string name, string instance)
        {
            cpuCounter = new PerformanceCounter();
            cpuCounter.CategoryName = category;
            cpuCounter.CounterName = name;
            cpuCounter.InstanceName = instance;
            cpuCounter.NextValue();
            Thread.Sleep(500);
            return cpuCounter.NextValue();
        }
    }
}
