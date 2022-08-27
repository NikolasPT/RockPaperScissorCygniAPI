using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RockPaperScissorCygniAPI.Model;
using RockPaperScissorCygniAPI.Model.Dtos;
using RockPaperScissorCygniAPI.Services;
using RockPaperScissorCygniAPI.UnitTests.Utilities;

namespace RockPaperScissorCygniAPI.Controllers.Tests
{
    public class GameControllerTests
    {
        [Fact]
        public async Task CreateGameAsync_PlayerNotDefined_ReturnBadRequestAsync()
        {
            // Arrange
            CreateGameDto createGameDto = new() { Name = String.Empty };

            Mock<IGameService> mockGameServiceAsync = new();
            GameController gameController = new(mockGameServiceAsync.Object);

            // Act
            var actionResult = await gameController.CreateGameAsync(createGameDto);

            // Assert
            var result = actionResult.Result as BadRequestObjectResult;
            result.Should().NotBeNull();
            result?.Value?.ToString()?.Contains("Cannot create game. Player name must be defined in request body.").Should().BeTrue();
        }


        [Fact]
        public async Task CreateGameAsync_Correct_ReturnCreatedAtAction()
        {
            // Arrange
            string name = "Test Player One";
            CreateGameDto createGameDto = new() { Name = name };

            Guid guid = Guid.NewGuid();
            Player testPlayer = new(name);
            Game game = new(guid, testPlayer);

            Mock<IGameService> mockGameServiceAsync = new();
            mockGameServiceAsync.Setup(t => t.CreateGameAsync(createGameDto)).ReturnsAsync(game);
            GameController gameController = new(mockGameServiceAsync.Object);

            // Act
            var actionResult = await gameController.CreateGameAsync(createGameDto);

            // Assert
            var result = actionResult.Result as CreatedAtActionResult;
            result.Should().NotBeNull();
            result?.ActionName.Should().Be("GetGameAsync");
            result?.RouteValues?.Keys.Should().Contain("id");
            result?.RouteValues?.Values.Should().Contain(guid);

            var resultObject = UnitTestsUtilities.GetObjectResultContent<GameDto>(actionResult);
            resultObject.Should().BeOfType<GameDto>();
            resultObject.Player1.Name.Should().Be(name);
            resultObject.Player1.Move.Should().Be(MovesAsStrings.NA);
            resultObject.GameOutcome.Should().Be(Outcome.Unfinished);
        }


        [Fact]
        public async Task JoinGameAsync_Correct_ReturnNoContentResult()
        {
            // Arrange
            string name = "Test Player One";
            JoinGameDto joinGameDto = new() { Name = name };

            Guid guid = Guid.NewGuid();

            Mock<IGameService> mockGameServiceAsync = new();
            mockGameServiceAsync.Setup(t => t.JoinGameAsync(guid, joinGameDto)).ReturnsAsync(new NoContentResult());
            GameController gameController = new(mockGameServiceAsync.Object);

            // Act
            var actionResult = await gameController.JoinGameAsync(guid, joinGameDto);

            // Assert
            var result = actionResult as NoContentResult;
            result.Should().NotBeNull();
        }


        [Fact]
        public async Task JoinGameAsync_GuidEmpty_ReturnBadRequest()
        {
            // Arrange
            string name = "Test Player One";
            JoinGameDto joinGameDto = new() { Name = name };

            Guid guid = Guid.Empty;

            Mock<IGameService> mockGameServiceAsync = new();
            GameController gameController = new(mockGameServiceAsync.Object);

            // Act
            var actionResult = await gameController.JoinGameAsync(guid, joinGameDto);

            // Assert
            var result = actionResult as BadRequestObjectResult;
            result?.Value?.ToString()?.Contains("Guid empty.").Should().BeTrue();
        }


        [Fact]
        public async Task JoinGameAsync_NameEmpty_ReturnBadRequest()
        {
            // Arrange
            JoinGameDto joinGameDto = new() { Name = String.Empty };

            Guid guid = Guid.NewGuid();

            Mock<IGameService> mockGameServiceAsync = new();
            GameController gameController = new(mockGameServiceAsync.Object);

            // Act
            var actionResult = await gameController.JoinGameAsync(guid, joinGameDto);

            // Assert
            var result = actionResult as BadRequestObjectResult;
            result?.Value?.ToString()?.Contains("Cannot join game. Player name must be defined in request body.").Should().BeTrue();
        }


        // TODO test GetGameAsync

        // TODO test MoveAsync

    }
}