using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Proxet.Tournament
{
    public class TeamGenerator
    {
        private List<Player> _blueTeam { get; set; } = new List<Player>();
        private List<Player> _redTeam { get; set; } = new List<Player>();

        private void AddPlayersToTeams(List<Player> players)
        {
            _blueTeam.AddRange(players.Skip(3));
            _redTeam.AddRange(players.Take(3));

        }

        private List<Player> GetPlayersFromFile(string path)
        {

            List<Player> players = new List<Player>();
            string Data;
            using (StreamReader SR = new StreamReader(path))
            {
                Data = SR.ReadToEnd();
            }

            string[] playersInTxt;// array of players in txt format 
            playersInTxt = Data.Split('\n').ToArray().Skip(1).ToArray();
            foreach (var it in playersInTxt)
            {

                string[] props; //array with properties of our players in txt format 
                props = it.Split('\t');

                Player player = new Player();
                player.Nickname = props[0];
                player.WaitingTime = int.Parse(props[1]);
                player.Type = int.Parse(props[2]);
                players.Add(player);

            }
            return players;
        }


        public (string[] team1, string[] team2) GenerateTeams(string filePath)
        {

            List<Player> players = GetPlayersFromFile(filePath);

            for (int i = 1; i < 4; i++)
            {
                var sortedPlayers = players.Where(x => x.Type == i).OrderByDescending(x => x.WaitingTime).Take(6).ToList();
                AddPlayersToTeams(sortedPlayers);
            }
            return ( _redTeam.Select(x => x.Nickname).ToArray(), _blueTeam.Select(x => x.Nickname).ToArray());
        }

    }
}
