using DataBase.Models;

namespace Service.Service;

public class GameEngine : IGameEngine
{
    public void CheckGameResult(Game game, int winCondition, int boardSize)
    {
        var board = game.BoardState;
        if (CheckWinCondition(board, "X", winCondition, boardSize))
        {
            CompleteGame(game, "X");
        }
        else if (CheckWinCondition(board, "O", winCondition, boardSize))
        {
            CompleteGame(game, "O");
        }
        else if (IsBoardFull(board))
        {
            CompleteGame(game, null);
        }
    }

    public bool IsBoardFull(string[] board)
    {
        return !board.Any(string.IsNullOrEmpty);
    }
    
    private bool CheckWinCondition(string[] board, string symbol, int winCondition, int boardSize)
    {
        for (var i = 0; i < boardSize; i++)
        {
            if (CountConsecutive(board, symbol, i, 0, 0, 1, boardSize) >= winCondition ||
                CountConsecutive(board, symbol, 0, i, 1, 0, boardSize) >= winCondition)
            {
                return true;
            }
        }

        return CountConsecutive(board, symbol, 0, 0, 1, 1, boardSize) >= winCondition ||
               CountConsecutive(board, symbol, 0, boardSize - 1, 1, -1, boardSize) >= winCondition;
    }

    private int CountConsecutive(string[] board, string symbol, int startX, int startY, int stepX, int stepY, int boardSize)
    {
        var count = 0;
        for (var i = 0; i < boardSize; i++)
        {
            var x = startX + stepX * i;
            var y = startY + stepY * i;
            if (x < 0 || y < 0 || x >= boardSize || y >= boardSize)
            {
                break;
            }

            if (board[x * boardSize + y] == symbol)
            {
                count++;
            }
            else
            {
                count = 0;
            }
        }
        return count;
    }

   

    private void CompleteGame(Game game, string? winner)
    {
        game.IsCompleted = true;
        game.WinnerSymbol = winner;
        game.FinishedAt = DateTime.UtcNow;
    }
}
