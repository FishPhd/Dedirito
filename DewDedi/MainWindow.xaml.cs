using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Threading;
using Microsoft.VisualBasic.Devices;

namespace DewDedi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        //Variables
        private int i = 0;
        private int time = 0;
        private bool running;
        private PerformanceCounter ramCounter;
        private DispatcherTimer timer2 = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            Cfg.Initial(false);
            try
            {
                Load();
            }
            catch
            {
                Cfg.Initial(true);
            }

            Load();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

            timer2.Interval = TimeSpan.FromSeconds(1);
            timer2.Tick += ServerTimer;
        }

        private void btnLaunch_OnClick(object sender, RoutedEventArgs e)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo()
            };

            process.StartInfo.FileName = "eldorado.exe";
            process.StartInfo.Arguments = "-dedicated -launcher";

            if (BtnLaunch.Content == "Close Server")
            {
                rcon.DewCmd("Game.exit");
                BtnLaunch.Content = "Launch Server";
            }
            else
            {
                process.Start();
                while (rcon.DewCmd("Check") == "Is Eldorito Running?")
                {
                    Console.WriteLine(rcon.DewCmd("Check"));
                    Thread.Sleep(1000);
                }

                ServerLaunch();
            }
        }

        private void ServerLaunch()
        {
            rcon.DewCmd("Server.Name " + ServerName.Text);
            rcon.DewCmd("Server.Password " + ServerPassword.Password);

            rcon.DewCmd("Server.LobbyType " + CmbLobbyType.SelectedValue);
            rcon.DewCmd("Server.Mode " + CmbLobbyMode.SelectedValue);

            rcon.DewCmd("Game.Map " + CmbServerMap.SelectedValue);
            rcon.DewCmd("Game.Gametype " + CmbGameGametype.SelectedValue);

            rcon.DewCmd("Server.MaxPlayers " + MaxPlayers.Value);
            rcon.DewCmd("Server.SprintEnabled " + SprintEnabled.Value);
            rcon.DewCmd("Server.UnlimitedSprint " + UnlimitedSprint.Value);
            rcon.DewCmd("Server.DualWieldEnabled " + DualWieldEnabled.Value);
            rcon.DewCmd("Server.AssassinationEnabled " + AssassinationEnabled.Value);

            rcon.DewCmd("Game.Start");
        }

        private void ServerTimer(object sender, EventArgs e)
        {
            Process[] p = Process.GetProcessesByName("eldorado");
            try
            {
                DateTime start = p[0].StartTime;
                TimeSpan time = DateTime.Now - start;

                const string format = "Server Uptime: {0}d {1}h {2}m {3}s";

                // Write uptime.
                ServerUptime.Content = string.Format(format, time.Days, time.Hours, time.Minutes, time.Seconds);
                Console.WriteLine(format, time.Days, time.Hours, time.Minutes, time.Seconds);
            }
            catch
            {
                Console.WriteLine("Process ended");
            }

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (i == 2)
            {
                ramCounter = new PerformanceCounter("Memory", "Available MBytes");
                RamUsage.Content = "Available Memory: " +
                                   Math.Round(CurrentRamusage(), 3, MidpointRounding.AwayFromZero) + " GB";
                running = CheckIfProcessIsRunning("eldorado");
                if (running)
                {
                    BtnLaunch.Content = "Close Server";
                    timer2.Start();
                }
                else
                {
                    ServerUptime.Content = "Server Uptime: 0d 0h 0m 0s";
                    timer2.Stop();
                }
                i = 0;
            }
            ++i;
        }

        // Load control values from cfg
        private void Load()
        {
            ServerName.Text = Cfg.configFile["Server.Name"];
            ServerPassword.Password = Cfg.configFile["Server.Password"];
            CmbLobbyType.SelectedValue = Cfg.configFile["Server.LobbyType"];
            CmbLobbyMode.SelectedValue = Cfg.configFile["Server.Mode"];
            CmbServerMap.SelectedValue = Cfg.configFile["Game.Map"];
            CmbGameGametype.SelectedValue = Cfg.configFile["Game.GameType"];
            MaxPlayers.Value = Convert.ToDouble(Cfg.configFile["Server.MaxPlayers"]);
            SprintEnabled.Value = Convert.ToDouble(Cfg.configFile["Server.SprintEnabled"]);
            UnlimitedSprint.Value = Convert.ToDouble(Cfg.configFile["Server.UnlimitedSprint"]);
            DualWieldEnabled.Value = Convert.ToDouble(Cfg.configFile["Server.DualWieldEnabled"]);
            AssassinationEnabled.Value = Convert.ToDouble(Cfg.configFile["Server.AssassinationEnabled"]);
        }

        private double CurrentRamusage()
        {
            ramCounter.NextValue();
            return ramCounter.NextValue()/1024;
        }

        private static bool CheckIfProcessIsRunning(string nameSubstring)
        {
            return Process.GetProcesses().Any(p => p.ProcessName.Contains(nameSubstring));
        }


        // Saving controls info to cfg
        private void ServerName_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!IsLoaded)
                return;
            Cfg.SetVariable("Server.Name", ServerName.Text, ref Cfg.configFile);
            Cfg.SaveConfigFile("server.cfg", Cfg.configFile);
        }

        private void ServerPassword_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!IsLoaded)
                return;
            Cfg.SetVariable("Server.Password", ServerPassword.Password, ref Cfg.configFile);
            Cfg.SaveConfigFile("server.cfg", Cfg.configFile);
        }

        private void CmbLobbyType_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded)
                return;
            Cfg.SetVariable("Server.LobbyType", Convert.ToString(CmbLobbyType.SelectedValue), ref Cfg.configFile);
            Cfg.SaveConfigFile("server.cfg", Cfg.configFile);

        }

        private void CmbLobbyMode_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded)
                return;
            Cfg.SetVariable("Server.Mode", Convert.ToString(CmbLobbyMode.SelectedValue), ref Cfg.configFile);
            Cfg.SaveConfigFile("server.cfg", Cfg.configFile);
        }

        private void CmbServerMap_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded)
                return;
            Cfg.SetVariable("Game.Map", Convert.ToString(CmbServerMap.SelectedValue), ref Cfg.configFile);
            Cfg.SaveConfigFile("server.cfg", Cfg.configFile);
        }

        private void CmbGameGametype_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded)
                return;
            Cfg.SetVariable("Game.GameType", Convert.ToString(CmbGameGametype.SelectedValue), ref Cfg.configFile);
            Cfg.SaveConfigFile("server.cfg", Cfg.configFile);
        }

        private void MaxPlayers_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!IsLoaded)
                return;
            Cfg.SetVariable("Server.MaxPlayers", Convert.ToString(MaxPlayers.Value), ref Cfg.configFile);
            Cfg.SaveConfigFile("server.cfg", Cfg.configFile);
        }

        private void SprintEnabled_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!IsLoaded)
                return;
            Cfg.SetVariable("Server.SprintEnabled", Convert.ToString(SprintEnabled.Value), ref Cfg.configFile);
            Cfg.SaveConfigFile("server.cfg", Cfg.configFile);
        }

        private void UnlimitedSprint_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!IsLoaded)
                return;
            Cfg.SetVariable("Server.UnlimitedSprint", Convert.ToString(UnlimitedSprint.Value), ref Cfg.configFile);
            Cfg.SaveConfigFile("server.cfg", Cfg.configFile);
        }

        private void DualWieldEnabled_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!IsLoaded)
                return;
            Cfg.SetVariable("Server.DualWieldEnabled", Convert.ToString(DualWieldEnabled.Value), ref Cfg.configFile);
            Cfg.SaveConfigFile("server.cfg", Cfg.configFile);
        }

        private void AssassinationEnabled_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!IsLoaded)
                return;
            Cfg.SetVariable("Server.AssassinationEnabled", Convert.ToString(AssassinationEnabled.Value), ref Cfg.configFile);
            Cfg.SaveConfigFile("server.cfg", Cfg.configFile);
        }
    }
}
