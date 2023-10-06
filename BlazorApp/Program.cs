using BlazorApp;
using BlazorApp.Authentication;
using BlazorApp.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.EntityFrameworkCore;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });


var connection = String.Empty;

//if (builder.Environment.IsDevelopment())
//{
//    builder.Configuration.AddEnvironmentVariables().AddJsonFile("appsettings.Development.json");
//    connection = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
//}
//else
//{
//    connection = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING");
//}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connection));


//--------------------------------------------------------------------------------------------------------------------------------------------------------
// Requires NuGet package: Blazored.LocalStorage
builder.Services.AddBlazoredLocalStorage();

//--------------------------------------------------------------------------------------------------------------------------------------------------------
// Requires NuGet package:
// - Microsoft.AspNetCore.Components.WebAssembly.Authentication
// - Microsoft.AspNetCore.Components
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

//--------------------------------------------------------------------------------------------------------------------------------------------------------
// Requires NuGet package: Microsoft.Extensions.Http
builder.Services.AddHttpClient<IUserService, UserService>();

await builder.Build().RunAsync();
