using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Sibers.ProjectManagementSystem.Presentation.Web.Blazor;
using MediatR;
using System.Reflection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);


IConfiguration configuration = builder.Configuration;
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("", client =>
{
    client.BaseAddress = new Uri(configuration["ApiBaseAddress"], UriKind.Absolute);
});
//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();
builder.Services.AddMediatR(typeof(Marker).GetTypeInfo().Assembly);


await builder.Build().RunAsync();
