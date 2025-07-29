using API.Controllers;
using DataBase;
using DataBase.Models;
using Service.Service;

namespace GameTest;

public class GameControllerFixture : IDisposable
{
    public DataBaseContext Context { get; }
    public GameController Controller { get; }
    public IGameService GameService { get; }

    public GameControllerFixture()
    {
        Context = DbContextFactory.CreateInMemoryContext();

        var config = new GameConfiguration
        {
            DefaultBoardSize = 3,
            RandomMoveInterval = 3,
            RandomMoveChancePercent = 10,
            WinCondition = 3
        };

        var engine = new GameEngine();
        var etagGenerator = new ETagGenerator();

        GameService = new GameService(Context, config, engine, etagGenerator);
        Controller = new GameController(GameService, etagGenerator);
    }


    public void Dispose()
    {
        Context.Dispose();
    }
}
