using ADIA.OpenAi.Proxy.Config.Models;
using ADIA.OpenAi.Proxy.Models;
using ADIA.OpenAi.Proxy.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace ADIA.Tests.OpenAiProxyTests;

public class AnalysisImageOpenAIServiceTest
{
    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_IfOpenIaConfigIsNull()
    {
        var loggerMock = new Mock<ILogger<AnalysisImageOpenAIService>>();
        Assert.Throws<ArgumentNullException>(() => new AnalysisImageOpenAIService(null, loggerMock.Object));
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_IfLoggerIsNull()
    {
        var configMock = new Mock<OpenIaConfig>();
        Assert.Throws<ArgumentNullException>(() => new AnalysisImageOpenAIService(configMock.Object, null));
    }

    [Fact]
    public async Task ProcessAsync_ShouldThrowArgumentException_WhenRequestHasInvalidImage()
    {
        var configMock = new Mock<OpenIaConfig>();
        var loggerMock = new Mock<ILogger<AnalysisImageOpenAIService>>();
        var service = new AnalysisImageOpenAIService(configMock.Object, loggerMock.Object);
        var request = new AnalysisOpenIARequest() { File = null };

        var response = await service.ProcessAsync(request);

        Assert.False(response.Success);
    }

    [Fact]
    public async Task ProcessAsync_ShouldSetStartAndEndTime()
    {
        var configMock = new Mock<OpenIaConfig>();
        var loggerMock = new Mock<ILogger<AnalysisImageOpenAIService>>();
        var service = new AnalysisImageOpenAIService(configMock.Object, loggerMock.Object);
        var request = new AnalysisOpenIARequest() { File = new byte[] { 1, 2, 3 } };

        var result = await service.ProcessAsync(request);

        Assert.NotNull(result.Start);
        Assert.NotNull(result.End);
        Assert.True(result.End >= result.Start);
    }


    [Fact]
    public void HandleException_ShouldLogError()
    {
        var configMock = new Mock<OpenIaConfig>();
        var loggerMock = new Mock<ILogger<AnalysisImageOpenAIService>>();
        var service = new AnalysisImageOpenAIService(configMock.Object, loggerMock.Object);
        var analysisResponse = new AnalysisOpenIAResponse();
        var exception = new InvalidOperationException("Error simulado");

        service.HandleException(exception, analysisResponse);

        loggerMock.Setup(x => x.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()));
        Assert.False(analysisResponse.Success);
    }



}