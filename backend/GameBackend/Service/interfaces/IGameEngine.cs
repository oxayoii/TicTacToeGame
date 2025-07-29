using DataBase.Models;

namespace Service.Service;

public interface IGameEngine
{
    void CheckGameResult(Game game, int winCondition, int boardSize);
    bool IsBoardFull(string[] board);
}