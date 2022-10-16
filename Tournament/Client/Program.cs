using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Tournament.Client;
using Tournament.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("Tournament.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Tournament.ServerAPI"));

builder.Services.AddApiAuthorization();
//  .AddAccountClaimsPrincipalFactory<UserFactory>();


builder.Services.AddScoped<GameService>();
builder.Services.AddScoped<TournamentService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddMudServices();

await builder.Build().RunAsync();
