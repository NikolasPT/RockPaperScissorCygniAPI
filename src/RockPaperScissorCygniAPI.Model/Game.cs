namespace RockPaperScissorCygniAPI.Model
{

    public class Game
    {
        public Guid Id { get; private set; }
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public string GameOutcome => GetOutcome();

        public Game()
        {
            Id = Guid.NewGuid();
            Player1 = new Player();
            Player2 = new Player();
        }

        public Game(Guid id)
        {
            Id = id;
            Player1 = new Player();
            Player2 = new Player();
        }

        public Game(Guid id, Player player1)
        {
            Id = id;
            Player1 = player1;
            Player2 = new Player();
        }

        public Game(Guid id, Player player1, Player player2)
        {
            Id = id;
            Player1 = player1;
            Player2 = player2;
        }


        private string GetOutcome()
        {
            string gameOutcome = Outcome.Unfinished;

            // Check that both players have played
            if (Player1?.Move == Move.NA || Player2?.Move == Move.NA)
                return gameOutcome;

            gameOutcome = (Player1?.Move) switch
            {
                Move.Paper when Player2?.Move == Move.Rock => Outcome.Player1Won,
                Move.Paper when Player2?.Move == Move.Scissors => Outcome.Player2Won,
                Move.Scissors when Player2?.Move == Move.Paper => Outcome.Player1Won,
                Move.Scissors when Player2?.Move == Move.Rock => Outcome.Player2Won,
                Move.Rock when Player2?.Move == Move.Paper => Outcome.Player2Won,
                Move.Rock when Player2?.Move == Move.Scissors => Outcome.Player1Won,
                _ => Outcome.Draw,
            };

            return gameOutcome;
        }


    }

}