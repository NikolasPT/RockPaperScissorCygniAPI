using Microsoft.AspNetCore.Mvc;
using RockPaperScissorCygniAPI.Model;
using RockPaperScissorCygniAPI.Model.Dtos;
using RockPaperScissorCygniAPI.Services;

namespace RockPaperScissorCygniAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IGameService gameService;

        public GameController(IGameService gameService)
        {
            this.gameService = gameService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGamesAsync()
        {
            var games = await gameService.GetGamesAsync();
            return games;
        }


        [HttpGet("{id}")]
        [ActionName("GetGameAsync")]
        public async Task<ActionResult<GameDto>> GetGameAsync(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Guid empty.");

            var game = await gameService.GetGameAsync(id);

            return game;
        }


        [HttpPost]
        public async Task<ActionResult<GameDto>> CreateGameAsync(CreateGameDto createGameDto)
        {
            if (string.IsNullOrEmpty(createGameDto?.Name))
                return BadRequest("Cannot create game. Player name must be defined in request body.");

            var game = await gameService.CreateGameAsync(createGameDto);

            return CreatedAtAction(nameof(GetGameAsync), new { id = game.Id }, game.AsDto());
        }


        [HttpPut("{id}/join")]
        public async Task<ActionResult> JoinGameAsync(Guid id, JoinGameDto joinGameDto)
        {
            if (id == Guid.Empty)
                return BadRequest("Guid empty.");

            if (string.IsNullOrEmpty(joinGameDto?.Name))
                return BadRequest("Cannot join game. Player name must be defined in request body.");

            var result = await gameService.JoinGameAsync(id, joinGameDto);
            return result;
        }


        [HttpPut("{id}/move")]
        public async Task<ActionResult> MoveAsync(Guid id, MoveDto moveDto)
        {
            if (id == Guid.Empty)
                return BadRequest("Guid empty.");

            if (string.IsNullOrEmpty(moveDto?.Name))
                return BadRequest("Cannot perform the move. Player name must be defined in request body.");

            if (string.IsNullOrEmpty(moveDto?.Move))
                return BadRequest("Cannot perform the move. Player move must be defined in request body.");

            var result = await gameService.MoveAsync(id, moveDto);
            return result;
        }



    }
}