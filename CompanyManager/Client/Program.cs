using CompanyManager.Client;
using CompanyManager.Client.Helpers;
using CompanyManager.Client.DataServices;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("CompanyManager.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("CompanyManager.ServerAPI"));

builder.Services.AddApiAuthorization();

builder.Services.AddTransient<IAppointmentDataService, AppointmentDataService>();
builder.Services.AddTransient<ICustomerDataService, CustomerDataService>();
builder.Services.AddTransient<IOfferDataService, OfferDataService>();

builder.Services.AddTransient<ICalendar, Calendar>();
builder.Services.AddTransient<ICalendarControls, CalendarControls>();

builder.Services.AddMudServices();

await builder.Build().RunAsync();
