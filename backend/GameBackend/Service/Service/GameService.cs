using DataBase;
using DataBase.DTO.Response;
using DataBase.Models;
using Microsoft.EntityFrameworkCore;
using static Service.Middleware.CustomException;

namespace Service.Service;

public class GameService : IGameService
{
    private readonly DataBaseContext _context;
    private readonly GameConfiguration _config;
    private readonly IGameEngine _engine;
    private readonly IETagGenerator _etagGenerator;
    private readonly Random _random = new();

    public GameService(DataBaseContext context, GameConfiguration config, IGameEngine engine, IETagGenerator etagGenerator)
    {
        _context = context;
        _config = config;
        _engine = engine;
        _etagGenerator = etagGenerator;
    }

    public async Task<int> CreateGame()
    {
        if (_config.DefaultBoardSize < 3)
        {
            throw new BadRequestException("Размер поля не может быть меньше 3");
        }

        var game = new Game
        {
            BoardSize = _config.DefaultBoardSize,
            BoardState = new string[_config.DefaultBoardSize * _config.DefaultBoardSize],
            CurrentPlayerSymbol = "X",
            Created = DateTime.UtcNow
        };

        _context.Games.Add(game);
        await _context.SaveChangesAsync();

        return game.Id;
    }

    public async Task<GameDto> MakeMove(int gameId, int row, int col, string? requestETag)
    {
        var game = await LoadGame(gameId);

        var gameDto = ToDto(game);
        if (requestETag != null && requestETag == _etagGenerator.Generate(gameDto))
        {
            return gameDto;
        }

        ValidateCoordinates(row, col, game.BoardSize);

        var index = row * game.BoardSize + col;
        if (game.BoardState[index] != null)
        {
            throw new BadRequestException("Ячейка уже занята");
        }

        var moveNumber = game.Movies.Count + 1;
        var isRandomMove = moveNumber % _config.RandomMoveInterval == 0 &&
                           _random.Next(100) < _config.RandomMoveChancePercent;

        var actualSymbol = isRandomMove
            ? (game.CurrentPlayerSymbol == "X" ? "O" : "X")
            : game.CurrentPlayerSymbol;

        game.BoardState[index] = actualSymbol;
        game.Movies.Add(new Move
        {
            GameId = gameId,
            Row = row,
            Column = col,
            PlayerSymbol = actualSymbol,
            MoveNumber = moveNumber,
            WasRandomized = isRandomMove,
            Created = DateTime.UtcNow
        });

        _engine.CheckGameResult(game, _config.WinCondition, game.BoardSize);
        if (!game.IsCompleted)
        {
            game.CurrentPlayerSymbol = game.CurrentPlayerSymbol == "X" ? "O" : "X";
        }

        await _context.SaveChangesAsync();
        return ToDto(game);
    }

    public async Task<GameDto> GetGameById(int gameId)
    {
        var game = await LoadGame(gameId);
        return ToDto(game);
    }

    private async Task<Game> LoadGame(int gameId)
    {
        if (gameId <= 0)
        {
            throw new BadRequestException("Неверный идентификатор игры");
        }

        var game = await _context.Games.Include(g => g.Movies).FirstOrDefaultAsync(g => g.Id == gameId);
        if (game == null)
        {
            throw new NotFoundException("Игра не найдена");
        }

        return game;
    }

    private static void ValidateCoordinates(int row, int col, int boardSize)
    {
        if (row < 0 || col < 0 || row >= boardSize || col >= boardSize)
        {
            throw new BadRequestException("Ход выходит за пределы игрового поля");
        }
    }

    private static GameDto ToDto(Game game) =>
        new()
        {
            Id = game.Id,
            BoardSize = game.BoardSize,
            BoardState = game.BoardState,
            CurrentPlayerSymbol = game.CurrentPlayerSymbol,
            IsCompleted = game.IsCompleted,
            WinnerSymbol = game.WinnerSymbol,
            CreatedAt = game.Created,
            FinishedAt = game.FinishedAt,
            Moves = game.Movies.Select(m => new MoveDto
            {
                MoveNumber = m.MoveNumber,
                Row = m.Row,
                Column = m.Column,
                PlayerSymbol = m.PlayerSymbol,
                WasRandomized = m.WasRandomized,
                CreatedAt = m.Created
            }).ToList()
        };
}


