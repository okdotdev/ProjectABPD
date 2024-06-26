using abcAPI.Models.DTOs;
using abcAPI.Models.TableModels;
using abcAPI.Repositories;
using abcAPI.Services;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace AbcTests;

public class AbcTests
{
   private readonly Mock<IClientRepository> _mockClientRepository;
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly ClientService _clientService;

        public AbcTests()
        {
            _mockClientRepository = new Mock<IClientRepository>();
            _mockUserManager = new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);

            _clientService = new ClientService(_mockClientRepository.Object, _mockUserManager.Object);
        }

        [Fact]
        public async Task AddClientAsync_Should_Call_AddClientAsync_On_Repository()
        {
            // Arrange
            var clientDto = new AddClientDto { Address = "123 Main St", Email = "test@example.com", PhoneNumber = "1234567890" };

            // Act
            await _clientService.AddClientAsync(clientDto);

            // Assert
            _mockClientRepository.Verify(r => r.AddClientAsync(clientDto), Times.Once);
        }

        [Fact]
        public async Task UpdateClientAsync_Should_Throw_AccessViolationException_When_User_Is_Not_Admin()
        {
            // Arrange
            var clientDto = new UpdateClientDto {  Address = "123 Main St", Email = "test@example.com", PhoneNumber = "1234567890" };
            _mockUserManager.Setup(m => m.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new User());
            _mockUserManager.Setup(m => m.IsInRoleAsync(It.IsAny<User>(), "Admin")).ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<AccessViolationException>(() => _clientService.UpdateClientAsync(clientDto, "user"));
        }

        [Fact]
        public async Task DeleteClientAsync_Should_Throw_AccessViolationException_When_User_Is_Not_Admin()
        {
            // Arrange
            _mockUserManager.Setup(m => m.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new User());
            _mockUserManager.Setup(m => m.IsInRoleAsync(It.IsAny<User>(), "Admin")).ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<AccessViolationException>(() => _clientService.DeleteClientAsync(1, "user"));
        }

        [Fact]
        public async Task GetClientsListAsync_Should_Return_ClientsList()
        {
            // Arrange
            var clients = new List<GetClientDto>
            {
                new GetClientDto { IdClient = 1, Address = "123 Main St", Email = "test1@example.com", PhoneNumber = "1234567890" },
                new GetClientDto { IdClient = 2, Address = "456 Maple Ave", Email = "test2@example.com", PhoneNumber = "9876543210" }
            };
            _mockClientRepository.Setup(r => r.GetClientsListAsync(It.IsAny<string>())).ReturnsAsync(clients);

            // Act
            var result = await _clientService.GetClientsListAsync("individual");

            // Assert
            Assert.Equal(clients, result);
        }
}