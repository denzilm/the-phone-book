using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using ThePhoneBook.Core.Entities;
using ThePhoneBook.Core.Repositories;
using ThePhoneBook.WebApi.Api.PhoneBooks;
using ThePhoneBook.WebApi.Api.PhoneBooks.Dtos;
using ThePhoneBook.WebApi.Services;
using Xunit;

namespace ThePhoneBook.WebApi.Tests
{
    public class PhoneBooksControllerTests
    {
        [Fact]
        public async Task GetPhoneBook_ShouldReturnNotFound_ForInvalidId()
        {
            // Arrange
            Guid phoneBookId = new Guid("25320c5e-f58a-4b1f-b63a-8ee07a840bdf");
            Mock<IPhoneBookRepository> mockPhoneBooksRepo = new Mock<IPhoneBookRepository>();
            Mock<IPhoneBookEntryRepository> mockPhoneBookEntriesRepo = new Mock<IPhoneBookEntryRepository>();
            Mock<ILogger<PhoneBooksController>> mockLogger = new Mock<ILogger<PhoneBooksController>>();
            Mock<IMapper> mockMapper = new Mock<IMapper>();
            Mock<IUserInfoService> mockUserInfoService = new Mock<IUserInfoService>();

            mockPhoneBooksRepo.Setup(repo => repo.GetByIdAsync(phoneBookId))
                .ReturnsAsync((PhoneBook)null);

            PhoneBooksController controller = new PhoneBooksController(
                mockLogger.Object, mockUserInfoService.Object, mockPhoneBooksRepo.Object,
                mockPhoneBookEntriesRepo.Object, mockMapper.Object);

            // Act
            var result = await controller.GetPhoneBook(phoneBookId).ConfigureAwait(false);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PhoneBookResponse>>(result);
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async Task GetPhoneBok_ShouldReturnForbidden_ForNonAuthenticatedUser()
        {
            // Arrange
            Guid phoneBookId = new Guid("25320c5e-f58a-4b1f-b63a-8ee07a840bdf");
            Guid userId = new Guid("25320c5e-f58a-4b1f-b63a-8ee07a840bdc");
            Mock<IPhoneBookRepository> mockPhoneBooksRepo = new Mock<IPhoneBookRepository>();
            Mock<IPhoneBookEntryRepository> mockPhoneBookEntriesRepo = new Mock<IPhoneBookEntryRepository>();
            Mock<ILogger<PhoneBooksController>> mockLogger = new Mock<ILogger<PhoneBooksController>>();
            Mock<IMapper> mockMapper = new Mock<IMapper>();
            Mock<IUserInfoService> mockUserInfoService = new Mock<IUserInfoService>();

            mockPhoneBooksRepo.Setup(repo => repo.GetByIdAsync(phoneBookId))
               .ReturnsAsync(new PhoneBook
               {
                   Id = new Guid("25320c5e-f58a-4b1f-b63a-8ee07a840bdf"),
                   UserId = new Guid("25320c5e-f58a-4b1f-b63a-8ee07a840bde")
               });

            mockUserInfoService.Setup(service => service.UserId).Returns("25320c5e-f58a-4b1f-b63a-8ee07a840bdc");

            PhoneBooksController controller = new PhoneBooksController(
                mockLogger.Object, mockUserInfoService.Object, mockPhoneBooksRepo.Object,
                mockPhoneBookEntriesRepo.Object, mockMapper.Object);

            // Act
            var result = await controller.GetPhoneBook(phoneBookId).ConfigureAwait(false);
            var actionResult = Assert.IsType<ActionResult<PhoneBookResponse>>(result);
            Assert.IsType<ForbidResult>(actionResult.Result);
        }

        [Fact]
        public async Task GetPhoneBok_ShouldReturnOk_ForAuthenticatedUser()
        {
            // Arrange
            Guid phoneBookId = new Guid("25320c5e-f58a-4b1f-b63a-8ee07a840bdf");
            Guid userId = new Guid("25320c5e-f58a-4b1f-b63a-8ee07a840bdc");
            Mock<IPhoneBookRepository> mockPhoneBooksRepo = new Mock<IPhoneBookRepository>();
            Mock<IPhoneBookEntryRepository> mockPhoneBookEntriesRepo = new Mock<IPhoneBookEntryRepository>();
            Mock<ILogger<PhoneBooksController>> mockLogger = new Mock<ILogger<PhoneBooksController>>();
            Mock<IMapper> mockMapper = new Mock<IMapper>();
            Mock<IUserInfoService> mockUserInfoService = new Mock<IUserInfoService>();

            mockPhoneBooksRepo.Setup(repo => repo.GetByIdAsync(phoneBookId))
               .ReturnsAsync(new PhoneBook
               {
                   Id = new Guid("25320c5e-f58a-4b1f-b63a-8ee07a840bdf"),
                   UserId = new Guid("25320c5e-f58a-4b1f-b63a-8ee07a840bdc")
               });

            mockUserInfoService.Setup(service => service.UserId).Returns("25320c5e-f58a-4b1f-b63a-8ee07a840bdc");

            PhoneBooksController controller = new PhoneBooksController(
                mockLogger.Object, mockUserInfoService.Object, mockPhoneBooksRepo.Object,
                mockPhoneBookEntriesRepo.Object, mockMapper.Object);

            // Act
            var result = await controller.GetPhoneBook(phoneBookId).ConfigureAwait(false);
            var actionResult = Assert.IsType<ActionResult<PhoneBookResponse>>(result);
            Assert.IsType<OkObjectResult>(actionResult.Result);
        }
    }
}