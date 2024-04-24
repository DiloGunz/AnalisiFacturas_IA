using ADIA.OpenAi.Proxy.Config.Models;
using ADIA.OpenAi.Proxy.Models;
using ADIA.OpenAi.Proxy.Services;
using ADIA.Tests.Adapters;
using Microsoft.Extensions.Logging;
using Moq;
using OpenAI_API;
using OpenAI_API.Chat;

namespace ADIA.Tests.OpenAiProxyTests;

public class AnalysisTextOpenAITests
{
    private readonly Mock<OpenIaConfig> _mockConfig;
    private readonly Mock<ILogger<AnalysisTextOpenAI>> _mockLogger;
    private readonly Mock<OpenAIAPI> _mockApi;
    private readonly AnalysisTextOpenAI _service;

    public AnalysisTextOpenAITests()
    {
        _mockConfig = new Mock<OpenIaConfig>();
        _mockLogger = new Mock<ILogger<AnalysisTextOpenAI>>();
        _mockApi = new Mock<OpenAIAPI>("fake-api-key");
        _service = new AnalysisTextOpenAI(_mockConfig.Object, _mockLogger.Object);
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_IfConfigIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new AnalysisTextOpenAI(null, _mockLogger.Object));
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_IfLoggerIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new AnalysisTextOpenAI(_mockConfig.Object, null));
    }

    [Fact]
    public async Task ProcessAsync_ThrowsArgumentException_WhenPromptUserIsEmpty()
    {
        var request = new AnalysisOpenIARequest { PromptUser = "" };
        var reponse = await _service.ProcessAsync(request);
        Assert.False(reponse.Success);
    }


    [Fact]
    public async Task ProcessAsync_CatchesExceptions_AndLogsError()
    {
        var request = new AnalysisOpenIARequest { PromptUser = "Hello, world!" };
        _mockApi.Setup(api => api.Chat.CreateChatCompletionAsync(It.IsAny<ChatRequest>())).Throws(new Exception("API failure"));

        var response = await _service.ProcessAsync(request);

        Assert.False(response.Success);
        _mockLogger.Verify(logger => logger.LogError(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_HandlesGeneralException_AndReturnsCustomMessage()
    {
        var request = new AnalysisOpenIARequest { PromptUser = "test" };
        _mockApi.Setup(api => api.Chat.CreateChatCompletionAsync(It.IsAny<ChatRequest>()))
                .Throws(new Exception("Test exception"));

        var result = await _service.ProcessAsync(request);

        Assert.False(result.Success);
        Assert.Contains("Test exception", result.Result);
    }

 
}