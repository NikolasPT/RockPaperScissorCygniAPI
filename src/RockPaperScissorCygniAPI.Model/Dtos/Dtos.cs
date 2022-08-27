using System.ComponentModel.DataAnnotations;

namespace RockPaperScissorCygniAPI.Model.Dtos
{
    public record GameDto
    {
        public Guid Id { get; set; }
        public PlayerDto Player1 { get; set; } = new PlayerDto();
        public PlayerDto Player2 { get; set; } = new PlayerDto();
        public string GameOutcome { get; set; } = String.Empty;
    }

    public record PlayerDto
    {
        public string Name { get; set; } = String.Empty;
        public string Move { get; set; } = String.Empty;
    }

    public record CreateGameDto
    {
        [Required]
        public string Name { get; set; } = String.Empty;
    }

    public record JoinGameDto
    {
        [Required]
        public string Name { get; set; } = String.Empty;
    }

    public record MoveDto
    {
        [Required]
        public string Name { get; set; } = String.Empty;

        [Required]
        public string Move { get; set; } = String.Empty;
    }

}
