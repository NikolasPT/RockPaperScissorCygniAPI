using RockPaperScissorCygniAPI.Model.Dtos;

namespace RockPaperScissorCygniAPI.Model
{
    public static class Extensions
    {
        public static GameDto AsDto(this Game game)
        {
            return new GameDto
            {
                Id = game.Id,
                Player1 = game.Player1.AsDto(),
                Player2 = game.Player2.AsDto(),
                GameOutcome = game.GameOutcome,
            };
        }

        public static PlayerDto AsDto(this Player game)
        {
            return new PlayerDto
            {
                Name = game.Name,
                Move = game.Move.ToString()
            };
        }

        public static Player MoveDtoAsPlayer(this MoveDto moveDto)
        {
            _ = Enum.TryParse(moveDto.Move, out Move move);

            return new Player
            {
                Name = moveDto.Name,
                Move = move
            };
        }
    }
}
