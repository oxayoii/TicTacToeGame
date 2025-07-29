using DataBase.DTO.Requests;
using DataBase.DTO.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameTest;

public class GameControllerTests : IClassFixture<GameControllerFixture>
{
    private readonly GameControllerFixture _fixture;

    public GameControllerTests(GameControllerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task CreateGame_ReturnsGameId()
    {
        var result = await _fixture.Controller.CreateGame();
        var actionResult = Assert.IsType<ActionResult<int>>(result);
        Assert.True(actionResult.Value > 0);
    }

    [Fact]
    public async Task GetGame_ReturnsCreatedGame()
    {
        var createdResult = await _fixture.Controller.CreateGame();
        var gameId = createdResult.Value;

        var result = await _fixture.Controller.GetGame(gameId);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedGame = Assert.IsType<GameDto>(okResult.Value);

        Assert.Equal(gameId, returnedGame.Id);
        Assert.Equal(3, returnedGame.BoardSize);
    }

    [Fact]
    public async Task MakeMove_UpdatesBoardAndReturnsETag()
    {
        var gameId = (await _fixture.Controller.CreateGame()).Value;
        var moveRequest = new MoveRequest { Row = 0, Column = 0 };

        var context = new DefaultHttpContext();
        context.Request.Headers["If-Match"] = "test-etag";

        _fixture.Controller.ControllerContext = new ControllerContext { HttpContext = context };

        var result = await _fixture.Controller.MakeMove(gameId, moveRequest);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var gameDto = Assert.IsType<GameDto>(okResult.Value);

        Assert.Equal("X", gameDto.BoardState[0]);  /// X первый ход
        Assert.True(context.Response.Headers.ContainsKey("ETag"));
    }
}