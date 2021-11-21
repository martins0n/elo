using System;
using System.Collections.Generic;

namespace Elo.Lib
{
    public class EloRating
    {
        public int kCoef { get; }
        public EloRating(int k)
        {
            kCoef = k;
        }

        private double startRating { get; } = 1000;

        public Dictionary<Player, double> rating { get; } = new Dictionary<Player, double> { };

        public void AddGame(Player first, Player second, GameResult result)
        {
            AddPlayer(first);
            AddPlayer(second);

            var (ratingChangeFirst, ratingChangeSecond) = ExpectedRatingChange(first, second, result);

            rating[first] += ratingChangeFirst;
            rating[second] += ratingChangeSecond;

        }

        public void AddPlayer(Player player)
        {
            if (!rating.ContainsKey(player))
            {
                rating[player] = startRating;
            }
        }

        public void AddTournament(One2OneTournament tournament)
        {
            var cummulativeChanges = new Dictionary<Player, double> { };
            foreach (var (first, second, result) in tournament.games)
            {
                AddPlayer(first);
                AddPlayer(second);
                var (ratingChangeFirst, ratingChangeSecond) = ExpectedRatingChange(first, second, result);
                if (!cummulativeChanges.ContainsKey(first))
                {
                    cummulativeChanges[first] = 0;
                }
                if (!cummulativeChanges.ContainsKey(second))
                {
                    cummulativeChanges[second] = 0;
                }
                cummulativeChanges[first] += ratingChangeFirst;
                cummulativeChanges[second] += ratingChangeSecond;
            }
            foreach (var item in cummulativeChanges)
            {
                rating[item.Key] += item.Value;
            }
        }

        public (double, double) ExpectedRatingChange(Player first, Player second, GameResult result)
        {
            var rFirst = rating[first];
            var rSecond = rating[second];
            var score = 1 / (1 + Math.Pow(10, (rSecond - rFirst) / 400));
            return result switch
            {
                GameResult.Win => (kCoef * (1 - score), kCoef * (score - 1)),
                GameResult.Lose => (kCoef * (0 - score), kCoef * (score)),
                GameResult.Draw => (kCoef * (0.5 - score), kCoef * (score - 0.5)),
            };
        }
    }
}
