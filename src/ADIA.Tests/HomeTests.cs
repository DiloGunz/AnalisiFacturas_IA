using ADIA.Shared.Response;
using ADIA.Web.Components.Pages;
using ADIA.Web.Components.Shared;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Blazorise.Tests.bUnit;
using Bunit;
using Bunit.TestDoubles;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace ADIA.Tests;

public class HomeTests : TestContext
{
    [Fact]
    public void HomeComponent_ShouldRenderCorrectly()
    {
        using var context = new TestContext();

        context.Services.AddBlazorise().Replace(ServiceDescriptor.Transient<IComponentActivator, ComponentActivator>());
        JSInterop.AddBlazoriseButton();

        // Configurar Blazorise en el contexto de pruebas
        context.Services
            .AddBlazorise(options =>
            {
                
            })
            .AddBootstrapProviders()
            .AddFontAwesomeIcons();

        // Ahora puedes añadir más configuraciones o mocks específicos si lo necesitas
        // Ejemplo: Mock de MediatR
        var mediatorMock = new Mock<IMediator>();
        context.Services.AddScoped<IMediator>(_ => mediatorMock.Object);

        // Renderizar el componente
        var component = context.RenderComponent<Home>();

        // Aserciones para verificar el renderizado correcto
        Assert.NotEmpty(component.Markup);
    }
}