using FluentAssertions;

namespace RockPaperScissorCygniAPI.Model.Tests
{
    public class GameTests
    {
        [Fact]
        public void CreateGameObject_NoArguments_GameCreated()
        {
            Game game = new();

            // Assert
            Guid.TryParse(game.Id.ToString(), out _).Should().BeTrue();
            game.Player1.Name.Should().Be(string.Empty);
            game.Player2.Name.Should().Be(string.Empty);
            game.Player1.Move.Should().Be(Move.NA);
            game.Player2.Move.Should().Be(Move.NA);
            game.GameOutcome.Should().Be(Outcome.Unfinished);
        }


        [Fact]
        public void CreateGameObject_OneArgument_GameCreated()
        {
            Guid guid = Guid.NewGuid();

            Game game = new(guid);

            // Assert
            game.Id.Should().Be(guid);
            game.Player1.Name.Should().Be(string.Empty);
            game.Player2.Name.Should().Be(string.Empty);
            game.Player1.Move.Should().Be(Move.NA);
            game.Player2.Move.Should().Be(Move.NA);
            game.GameOutcome.Should().Be(Outcome.Unfinished);
        }


        [Fact]
        public void CreateGameObject_TwoArguments_GameCreated()
        {
            string name1 = "Test Player One";
            Move move1 = Move.Paper;
            Player testPlayer1 = new(name1, move1);

            Guid guid = Guid.NewGuid();

            Game game = new(guid, testPlayer1);

            // Assert
            game.Id.Should().Be(guid);
            game.Player1.Name.Should().Be(name1);
            game.Player2.Name.Should().Be(string.Empty);
            game.Player1.Move.Should().Be(move1);
            game.Player2.Move.Should().Be(Move.NA);
            game.GameOutcome.Should().Be(Outcome.Unfinished);
        }


        [Fact]
        public void CreateGameObject_ThreeArguments_GameCreated()
        {
            string name1 = "Test Player One";
            Move move1 = Move.Paper;
            Player testPlayer1 = new(name1, move1);

            string name2 = "Test Player Two";
            Move move2 = Move.Rock;
            Player testPlayer2 = new(name2, move2);

            Guid guid = Guid.NewGuid();

            Game game = new(guid, testPlayer1, testPlayer2);

            // Assert
            game.Id.Should().Be(guid);
            game.Player1.Name.Should().Be(name1);
            game.Player2.Name.Should().Be(name2);
            game.Player1.Move.Should().Be(move1);
            game.Player2.Move.Should().Be(move2);
            game.GameOutcome.Should().Be(Outcome.Player1Won);
        }


        [Fact]
        public void GameOutcome_Player1WonGame()
        {
            string name1 = "Test Player One";
            Move move1 = Move.Scissors;
            Player testPlayer1 = new(name1, move1);

            string name2 = "Test Player Two";
            Move move2 = Move.Paper;
            Player testPlayer2 = new(name2, move2);

            Game game = new(Guid.NewGuid(), testPlayer1, testPlayer2);

            // Assert
            game.GameOutcome.Should().Be(Outcome.Player1Won);
        }


        [Fact]
        public void GameOutcome_Player2WonGame()
        {
            string name1 = "Test Player One";
            Move move1 = Move.Rock;
            Player testPlayer1 = new(name1, move1);

            string name2 = "Test Player Two";
            Move move2 = Move.Paper;
            Player testPlayer2 = new(name2, move2);

            Game game = new(Guid.NewGuid(), testPlayer1, testPlayer2);

            // Assert
            game.GameOutcome.Should().Be(Outcome.Player2Won);
        }


        [Fact]
        public void GameOutcome_DrawGame()
        {
            string name1 = "Test Player One";
            Move move1 = Move.Rock;
            Player testPlayer1 = new(name1, move1);

            string name2 = "Test Player Two";
            Move move2 = Move.Rock;
            Player testPlayer2 = new(name2, move2);

            Game game = new(Guid.NewGuid(), testPlayer1, testPlayer2);

            // Assert
            game.GameOutcome.Should().Be(Outcome.Draw);
        }


        [Fact]
        public void GameOutcome_UnfinishedGame()
        {
            string name1 = "Test Player One";
            Move move1 = Move.Rock;
            Player testPlayer1 = new(name1, move1);

            string name2 = "Test Player Two";
            Move move2 = Move.NA;
            Player testPlayer2 = new(name2, move2);

            Game game = new(Guid.NewGuid(), testPlayer1, testPlayer2);

            // Assert
            game.GameOutcome.Should().Be(Outcome.Unfinished);
        }

    }
}