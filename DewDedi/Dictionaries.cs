﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DewDedi
{
    class Dictionaries
    {
        public static Dictionary<string, string> LobbyType()
        {
            var Lobby = new Dictionary<string, string>();
            Lobby.Add("Multiplayer", "2");
            Lobby.Add("Forge", "3");
            return Lobby;
        }
        public static Dictionary<string, string> LobbyMode()
        {
            var Lobby = new Dictionary<string, string>();
            Lobby.Add("Online", "3");
            Lobby.Add("Offline", "4");
            return Lobby;
        }
        public static Dictionary<string, string> ServerMap()
        {
            var Lobby = new Dictionary<string, string>();
            Lobby.Add("Random", "random");
            Lobby.Add("Guardian", "guardian");
            Lobby.Add("Valhalla", "riverworld");
            Lobby.Add("Avalanche", "s3d_avalanche");
            Lobby.Add("Edge", "s3d_edge");
            Lobby.Add("Turf", "s3d_turf");
            Lobby.Add("Reactor", "s3d_reactor");
            return Lobby;
        }

        public static Dictionary<string, string> GameGametype()
        {
            var Lobby = new Dictionary<string, string>();
            Lobby.Add("Random", "random");
            Lobby.Add("Slayer", "slayer");
            Lobby.Add("Team Slayer", "team_slayer");
            Lobby.Add("Duel", "duel");
            //Lobby.Add("Skirmish", "skirmish");
            //Lobby.Add("Spartans vs Elites", "spartans_vs_elites");
            Lobby.Add("Team Swat", "team_swat");
            Lobby.Add("FFA Odball", "odd_ball");
            Lobby.Add("Odball", "odd_ball_ffa");
            Lobby.Add("Multi Flag", "ctf");
            Lobby.Add("Siege Assault", "assault_siege");
            Lobby.Add("Crazy King", "koth");
            return Lobby;
        }
    }
}
