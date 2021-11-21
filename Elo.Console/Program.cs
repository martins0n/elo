using System.Collections.Generic;
using Elo.Lib;

namespace Elo.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var elo = new EloRating(20);

            var nepo = new Player("nepo");
            var veteran = new Player("veteran75");
            var magnus = new Player("DrDrunkenstein");

            var games = new List<(Player, Player, GameResult)>  {
                (nepo, veteran, GameResult.Win),
                (nepo, magnus, GameResult.Win),
                (veteran, magnus, GameResult.Win),
                (veteran, magnus, GameResult.Draw),
                (nepo, magnus, GameResult.Draw),
            };

            foreach (var (first, second, result) in games)
            {
                elo.AddGame(first, second, result);
            }

            System.Console.WriteLine("Rating change after each game:");
            foreach (var pair in elo.rating)
            {
                System.Console.WriteLine("{0}, Rating: {1}", pair.Key.name, pair.Value);
            }


            elo = new EloRating(20);

            var tournament = new One2OneTournament(games);
            elo.AddTournament(tournament);

            System.Console.WriteLine("Rating change after batch of game:");
            foreach (var pair in elo.rating)
            {
                System.Console.WriteLine("{0}, Rating: {1}", pair.Key.name, pair.Value);
            }

        }
    }
}
