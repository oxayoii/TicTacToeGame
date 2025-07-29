using DataBase.DTO.Response;

namespace Service.Service;

public class ETagGenerator : IETagGenerator
{
    public string Generate(GameDto game)
    {
        var stateString = $"{game.Id}-{game.BoardState}-{game.CurrentPlayerSymbol}-{game.IsCompleted}";
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(stateString));
    }
}