using FluentAssertions;
using RockPaperScissorCygniAPI.Model;

namespace RockPaperScissorCygniAPI.DataRepository.Tests
{
    public class GamesRepositoryTest
    {
        [Fact]
        public void GetGamesAsync_RepoIsEmpty_EmptyGamesList()
        {
            // Arrange / Act
            IGamesRepository gamesRepo = new InMemGamesRepository();

            // Assert
            gamesRepo.GetGamesAsync().Result.Should().BeEmpty();
        }


        [Fact]
        public void GetGamesAsync_RepoHasGames_ListWithGames()
        {
            // Arrange
            List<Game>? games = new()
            {
                new Game()
            };

            // Act
            IGamesRepository gamesRepo = new InMemGamesRepository(games);

            // Assert
            gamesRepo.GetGamesAsync().Result.Should().NotBeEmpty();
        }


        [Fact]
        public void GetGameAsync_GetGameOnId_CorrectGameReturned()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            Game game = new(guid);
            List<Game>? games = new()
            {
                game
            };

            // Act
            IGamesRepository gamesRepo = new InMemGamesRepository(games);

            // Assert
            gamesRepo.GetGameAsync(guid).Result.Should().NotBeNull();
        }


        [Fact]
        public void SaveGameAsync_EmptyGuid_Exception()
        {
            // Arrange
            Game game = new(Guid.Empty);   
            IGamesRepository gamesRepo = new InMemGamesRepository();

            // Act Func<Task> required here for async call
            Func<Task> func = async () => await gamesRepo.SaveGameAsync(game);

            // Assert
            func.Should().ThrowAsync<ArgumentException>();
        }


        [Fact]
        public async Task SaveGameAsync_SaveAGame_ResultOK()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            Game game = new(guid);
            IGamesRepository gamesRepo = new InMemGamesRepository();

            // Act
            await gamesRepo.SaveGameAsync(game);

            // Assert
            gamesRepo.GetGameAsync(guid).Result.Should().NotBeNull();
        }


        [Fact]
        public async Task UpdateGameAsync_UpdateSpecificGame_GameUpdated()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            Game game = new(guid);
            List<Game>? games = new()
            {
                game
            };
            IGamesRepository gamesRepo = new InMemGamesRepository(games);

            string name = "Test Player One";
            Move move = Move.Scissors;
            Player testPlayer = new(name, move);
            Game updatedGame = new(guid, testPlayer);

            // Act
            await gamesRepo.UpdateGameAsync(updatedGame);

            // Assert
            gamesRepo.GetGameAsync(guid).Result?.Player1.Name.Should().Be(name);
            gamesRepo.GetGameAsync(guid).Result?.Player1.Move.Should().Be(move);
        }



    }
}