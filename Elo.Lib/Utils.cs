using System.Collections.Generic;

namespace Elo.Lib
{
    public record Player(string name);

    public enum GameResult
    {
        Win,
        Lose,
        Draw
    }

    public record One2OneTournament(List<(Player, Player, GameResult)> games);
}