using DataBase;
using DataBase.DTO.Response;

public interface IGameService
{
    Task<int> CreateGame();
    Task<GameDto> MakeMove(int gameId, int row, int col, string? requestETag);
    Task<GameDto> GetGameById(int gameId);
}