using Microsoft.AspNetCore.Mvc;
using RockPaperScissorCygniAPI.Model;
using RockPaperScissorCygniAPI.Model.Dtos;

namespace RockPaperScissorCygniAPI.Services
{
    public interface IGameService
    {
        public Task<ActionResult<IEnumerable<GameDto>>> GetGamesAsync();
        public Task<ActionResult<GameDto>> GetGameAsync(Guid id);
        public Task<Game> CreateGameAsync(CreateGameDto createGameDto);
        public Task<ActionResult> JoinGameAsync(Guid id, JoinGameDto joinGameDto);
        public Task<ActionResult> MoveAsync(Guid id, MoveDto moveDto);
    }
}
