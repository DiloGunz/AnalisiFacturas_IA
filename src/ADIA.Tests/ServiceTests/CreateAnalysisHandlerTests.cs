using ADIA.Model.DataTransfer.Commands.AnalysisCommands;
using ADIA.Repository.Interfaces.Repositories.AnalysisRepositories;
using ADIA.Service.AnalysisStrategies.Interfaces;
using ADIA.Service.EventHandlers.AnalysisEventHandlers;
using ADIA.Uow.Interfaces;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;

namespace ADIA.Tests.ServiceTests;

public class CreateAnalysisHandlerTests
{
    [Fact]
    public async Task Handle_ValidationFails_ReturnsValidationErrorResponse()
    {
        var mockUow = new Mock<IUnitOfWork>();
        var mockValidator = new Mock<IValidator<CreateAnalysisCommand>>();
        var mockLogger = new Mock<ILogger<CreateAnalysisHandler>>();
        var mockMapper = new Mock<IMapper>();
        var mockStrategyResolver = new Mock<IAnalysisStrategyResolver>();

        var validationResult = new ValidationResult(new List<ValidationFailure> { new ValidationFailure("file", "Invalid file") });
        mockValidator.Setup(v => v.ValidateAsync(It.IsAny<CreateAnalysisCommand>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(validationResult);

        var handler = new CreateAnalysisHandler(mockUow.Object, mockLogger.Object, mockValidator.Object, mockMapper.Object, mockStrategyResolver.Object);

        var response = await handler.Handle(new CreateAnalysisCommand(), CancellationToken.None);

        Assert.False(response.IsSuccess);
        Assert.Contains("Invalid file", response.Messages);
    }

    [Fact]
    public async Task Handle_ValidatorThrowsException_ReturnsFailureResponse()
    {
        var mockUow = new Mock<IUnitOfWork>();
        var mockValidator = new Mock<IValidator<CreateAnalysisCommand>>();
        var mockLogger = new Mock<ILogger<CreateAnalysisHandler>>();
        var mockMapper = new Mock<IMapper>();
        var mockStrategyResolver = new Mock<IAnalysisStrategyResolver>();

        mockValidator.Setup(v => v.ValidateAsync(It.IsAny<CreateAnalysisCommand>(), It.IsAny<CancellationToken>()))
                     .ThrowsAsync(new Exception("Validation exception"));

        var handler = new CreateAnalysisHandler(mockUow.Object, mockLogger.Object, mockValidator.Object, mockMapper.Object, mockStrategyResolver.Object);

        var response = await handler.Handle(new CreateAnalysisCommand(), CancellationToken.None);

        Assert.False(response.IsSuccess);
        Assert.Equal("Validation exception", response.Message);
    }

    [Fact]
    public async Task Handle_CreateEntityFails_ErrorDatabase()
    {
        var mockUow = new Mock<IUnitOfWork>();
        var mockValidator = new Mock<IValidator<CreateAnalysisCommand>>();
        var mockLogger = new Mock<ILogger<CreateAnalysisHandler>>();
        var mockMapper = new Mock<IMapper>();
        var mockStrategyResolver = new Mock<IAnalysisStrategyResolver>();
        var mockUowRepository = new Mock<IUnitOfWorkRepository>();
        var mockAnaylysisReposiotry = new Mock<IAnalysisRepository>();

        mockUowRepository.Setup(uow => uow.Analysis).Returns(mockAnaylysisReposiotry.Object);
        mockUow.Setup(x => x.Repository).Returns(mockUowRepository.Object);
        mockUow.Setup(u => u.SaveChangesAsync()).ThrowsAsync(new Exception("Database error"));

        var validationResult = new ValidationResult();
        mockValidator.Setup(v => v.ValidateAsync(It.IsAny<CreateAnalysisCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

        var handler = new CreateAnalysisHandler(mockUow.Object, mockLogger.Object, mockValidator.Object, mockMapper.Object, mockStrategyResolver.Object);

        var command = new CreateAnalysisCommand()
        {
            File = new byte[10],
            FileExtension = "pdf",
            FileName = "file"
        };

        var response = await handler.Handle(command, CancellationToken.None);

        Assert.False(response.IsSuccess);
        Assert.Equal("Database error", response.Message);
    }

    
}