using DataBase;
using DataBase.DTO.Requests;
using DataBase.DTO.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Service;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : Controller
    {
        private readonly IGameService _gameService;
        private readonly IETagGenerator _etagGenerator;

        public GameController(IGameService gameService, IETagGenerator etagGenerator)
        {
            _gameService = gameService;
            _etagGenerator = etagGenerator;
        }
        
        [HttpPost]
        public async Task<ActionResult<int>> CreateGame()
        {
           var gameId = await _gameService.CreateGame();
           return gameId;
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<GameDto>> GetGame(int id)
        {
            var game = await _gameService.GetGameById(id);
            return Ok(game);
        }
        
        [HttpPost("{id}/moves")]
        public async Task<IActionResult> MakeMove(int id, [FromBody] MoveRequest request)
        {
            var requestETag = Request.Headers.IfMatch.FirstOrDefault();

            var gameDto = await _gameService.MakeMove(id, request.Row, request.Column, requestETag);

            var etag = _etagGenerator.Generate(gameDto);

            Response.Headers.Append("ETag", etag);

            return Ok(gameDto); 
        }
    }
}
 