namespace RockPaperScissorCygniAPI.Model
{

    public class Player
    {
        public string Name { get; set; }
        public Move Move { get; set; }

        public Player()
        {
            Name = string.Empty;
            Move = Move.NA;
        }

        public Player(string name)
        {
            Name = name;
            Move = Move.NA;
        }

        public Player(string name, Move move)
        {
            Name = name;
            List<Move> legalMoves = new() { Move.Rock, Move.Paper, Move.Scissors };
            Move = legalMoves.Contains(move) ? move : Move.NA;
        }

    }


}