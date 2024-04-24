using OpenAI_API;
using OpenAI_API.Chat;

namespace ADIA.Tests.Adapters;

public class OpenAIChatAdapter
{
    private readonly OpenAIAPI _openAIApi;

    public OpenAIChatAdapter(OpenAIAPI openAIApi)
    {
        _openAIApi = openAIApi;
    }

    public virtual Task<ChatResult> CreateChatCompletionAsync(ChatRequest request)
    {
        return _openAIApi.Chat.CreateChatCompletionAsync(request);
    }
}