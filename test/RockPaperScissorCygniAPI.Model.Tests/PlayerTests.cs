using FluentAssertions;

namespace RockPaperScissorCygniAPI.Model.Tests
{
    public class PlayerTests
    {
        [Fact]
        public void CreatePlayerObject_NoArguments_ObjectCreated()
        {
            Player testPlayer = new();

            // Assert
            testPlayer.Name.Should().Be(String.Empty);
            testPlayer.Move.Should().Be(Move.NA);
        }


        [Fact]
        public void CreatePlayerObject_OneArgument_ObjectCreated()
        {
            string name = "Test Player One";
            Player testPlayer = new(name);

            // Assert
            testPlayer.Name.Should().Be(name);
            testPlayer.Move.Should().Be(Move.NA);
        }


        [Fact]
        public void CreatePlayerObject_TwoArguments_ObjectCreated()
        {
            string name = "Test Player One";
            Move move = Move.Scissors;
            Player testPlayer = new(name, move);

            // Assert
            testPlayer.Name.Should().Be(name);
            testPlayer.Move.Should().Be(move);
        }

    }
}