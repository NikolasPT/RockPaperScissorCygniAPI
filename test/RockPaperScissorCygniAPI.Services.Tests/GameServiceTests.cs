using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RockPaperScissorCygniAPI.DataRepository;
using RockPaperScissorCygniAPI.Model;
using RockPaperScissorCygniAPI.Model.Dtos;
using RockPaperScissorCygniAPI.UnitTests.Utilities;

namespace RockPaperScissorCygniAPI.Services.Tests
{
    public class GameServiceTests
    {
        [Fact]
        public async Task GetGamesAsync_Correct_ReturnOkObjectResult()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            Game game = new(guid);
            List<Game>? games = new()
            {
                game
            };

            Mock<IGamesRepository> mockGamesRepositoryAsync = new();
            mockGamesRepositoryAsync.Setup(t => t.GetGamesAsync()).ReturnsAsync(games);
            GameService gameService = new(mockGamesRepositoryAsync.Object);

            // Act
            var actionResult = await gameService.GetGamesAsync();

            // Assert
            var result = actionResult.Result as OkObjectResult;
            result.Should().NotBeNull();

            var resultObject = UnitTestsUtilities.GetObjectResultContent<IEnumerable<GameDto>>(actionResult);
            resultObject.SingleOrDefault(x => x.Id.Equals(guid)).Should().BeOfType<GameDto>();
            resultObject.SingleOrDefault(x => x.Id.Equals(guid))?.GameOutcome.Should().Be(Outcome.Unfinished);
        }


        [Fact]
        public async Task GetGameAsync_GameIsNull_ReturnNotFoundObjectResult()
        {
            // Arrange
            Mock<IGamesRepository> mockGamesRepositoryAsync = new();
            mockGamesRepositoryAsync.Setup(t => t.GetGameAsync(new Guid()));
            GameService gameService = new(mockGamesRepositoryAsync.Object);

            // Act
            var actionResult = await gameService.GetGameAsync(new Guid());

            // Assert
            var result = actionResult.Result as NotFoundObjectResult;
            result.Should().NotBeNull();
            result?.Value?.ToString()?.Contains("No game found with id = ").Should().BeTrue();
        }


        [Fact]
        public async Task GetGameAsync_GameUnfinished_ReturnGameWithHiddenMoves()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            string name = "Test Player One";
            Move move = Move.Scissors;
            Player testPlayer = new(name, move);
            Game game = new(guid, testPlayer);

            Mock<IGamesRepository> mockGamesRepositoryAsync = new();
            mockGamesRepositoryAsync.Setup(t => t.GetGameAsync(guid)).ReturnsAsync(game);
            GameService gameService = new(mockGamesRepositoryAsync.Object);

            // Act
            var actionResult = await gameService.GetGameAsync(guid);

            // Assert
            var result = actionResult.Result as OkObjectResult;
            result.Should().NotBeNull();

            var resultObject = UnitTestsUtilities.GetObjectResultContent<GameDto>(actionResult);
            resultObject.Should().BeOfType<GameDto>();
            resultObject.Player1.Name.Should().Be(name);
            resultObject.Player1.Move.Should().Be(MovesAsStrings.NA);
            resultObject.GameOutcome.Should().Be(Outcome.Unfinished);
        }


        [Fact]
        public async Task GetGameAsync_GameFinished_ReturnOkObjectResult()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            string name1 = "Test Player One";
            Move move1 = Move.Scissors;
            Player testPlayer1 = new(name1, move1);
            string name2 = "Test Player Two";
            Move move2 = Move.Paper;
            Player testPlayer2 = new(name2, move2);

            Game game = new(guid, testPlayer1, testPlayer2);

            Mock<IGamesRepository> mockGamesRepositoryAsync = new();
            mockGamesRepositoryAsync.Setup(t => t.GetGameAsync(guid)).ReturnsAsync(game);
            GameService gameService = new(mockGamesRepositoryAsync.Object);

            // Act
            var actionResult = await gameService.GetGameAsync(guid);

            // Assert
            var result = actionResult.Result as OkObjectResult;
            result.Should().NotBeNull();

            var resultObject = UnitTestsUtilities.GetObjectResultContent<GameDto>(actionResult);
            resultObject.Should().BeOfType<GameDto>();
            resultObject.Player1.Name.Should().Be(name1);
            resultObject.Player1.Move.Should().Be(MovesAsStrings.Scissors);
            resultObject.Player2.Name.Should().Be(name2);
            resultObject.Player2.Move.Should().Be(MovesAsStrings.Paper);
            resultObject.GameOutcome.Should().Be(Outcome.Player1Won);
        }


        // TODO test CreateGameAsync

        // TODO test JoinGameAsync

        // TODO test MoveAsync




    }
}