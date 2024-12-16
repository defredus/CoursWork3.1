using System;
using CoursWorkUI.Users;
using CoursWorkUI.Interfaces;
using CoursWorkUI;
using Xunit;
using Moq;
using BLL.IServices;
using System.Text;
using System.Security.Cryptography;


namespace CoursWorkTests
{

    namespace CoursWorkUI.Tests
    {
        public class UnitClientTests
        {
            private readonly Mock<IClientService> _mockClientService;
            private readonly Mock<IServiceService> _mockServiceService;
            private readonly Mock<IPaymentService> _mockPaymentService;
            private readonly Mock<IAuthService> _mockAuthService;
            private readonly Mock<IAdminService> _mockAdminService;
            private readonly Mock<IManagerService> _mockManagerService;
            private readonly ServiceStorage _serviceStorage;
            private readonly ClientMenu _clientMenu;

            public UnitClientTests()
            {
                Console.OutputEncoding = Encoding.UTF8;
                Console.InputEncoding = Encoding.UTF8;
                //Console.OutputEncoding = Encoding.Default;
                //Console.InputEncoding = Encoding.Default;

                // Создаем моки для всех зависимостей
                _mockClientService = new Mock<IClientService>();
                _mockServiceService = new Mock<IServiceService>();
                _mockPaymentService = new Mock<IPaymentService>();
                _mockAuthService = new Mock<IAuthService>();
                _mockAdminService = new Mock<IAdminService>();
                _mockManagerService = new Mock<IManagerService>();

                // Инициализируем ServiceStorage с моками
                _serviceStorage = new ServiceStorage(
                    _mockAuthService.Object,
                    _mockClientService.Object,
                    _mockServiceService.Object,
                    _mockPaymentService.Object,
                    _mockAdminService.Object,
                    _mockManagerService.Object
                );

                // Создаем экземпляр ClientMenu
                _clientMenu = new ClientMenu("1", _serviceStorage);
            }

            [Fact]
            public void ShowMenu_ShouldCallGetDataOfClient_WhenChoiceIs1()
            {
                // Arrange
                var choice = "1"; // Пользователь выбирает "1"
                var consoleReader = new StringReader(choice); // Эмулируем ввод
                Console.SetIn(consoleReader); // Настройка ввода

                // Настроим мок, чтобы не делать ничего лишнего, просто проверим вызов
                _mockClientService.Setup(service => service.GetDataOfClient(It.IsAny<string>()));

                // Act
                _serviceStorage.clientService.GetDataOfClient(choice);

                // Assert
                _mockClientService.Verify(service => service.GetDataOfClient(It.Is<string>(id => id == "1")), Times.Once);
            }


            [Fact]
            public void ShowMenu_ShouldCallShowMyTariffPlan_WhenChoiceIs2()
            {
                // Arrange
                var choice = "2";
                var consoleReader = new StringReader(choice);
                Console.SetIn(consoleReader);

                // Act
                _serviceStorage.clientService.ShowMyTariffPlan(choice);
                _serviceStorage.clientService.ChangeTariffPlan(choice);

                // Assert
                _mockClientService.Verify(service => service.ShowMyTariffPlan(It.IsAny<string>()), Times.Once);
            }
            [Fact]
            public void ShowMenu_ShouldCallAddNewServiceToClient_WhenChoiceIs3()
            {
                // Arrange
                var choice = "3";
                var consoleReader = new StringReader(choice);
                Console.SetIn(consoleReader);

                // Act
                _serviceStorage.serviceService.GetAll();
                _serviceStorage.serviceService.AddNewServiceToClient("1");

                // Assert
                _mockServiceService.Verify(service => service.AddNewServiceToClient(It.IsAny<string>()), Times.Once);
            }
            [Fact]
            public void ShowMenu_ShouldCallReplenishBalance_WhenChoiceIs6()
            {
                // Arrange
                var choice = "6";
                var consoleReader = new StringReader(choice);
                Console.SetIn(consoleReader);

                // Act
                _serviceStorage.paymentService.ReplenishBalance("2");

                // Assert
                _mockPaymentService.Verify(service => service.ReplenishBalance(It.IsAny<string>()), Times.Once);
            }

            [Fact]
            public void ShowMenu_ShouldCallChangePassword_WhenChoiceIs7()
            {
                // Arrange
                var choice = "2";
                var consoleReader = new StringReader(choice);
                Console.SetIn(consoleReader);

                // Act
                _serviceStorage.clientService.ChangePassword("2");

                // Assert
                _mockClientService.Verify(service => service.ChangePassword(It.IsAny<string>()), Times.Once);
            }
        }
    }
}