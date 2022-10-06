using FluentValidation;
using Markdig;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Memory;
using OsborneSupremacy.Extensions.Net.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

configuration.AddUserSecrets<Program>(true);

// get azure config service info
var azureConfigConnectionString = Environment
    .GetEnvironmentVariable("AZURE_CONFIG_CONNECTIONSTRING") ?? string.Empty;

if (!string.IsNullOrWhiteSpace(azureConfigConnectionString))
    configuration.AddAzureAppConfiguration(azureConfigConnectionString);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddValidatorsFromAssemblyContaining<EmailPayloadValidator>();

var openIdConnectSettings = builder
    .GetAndValidateTypedSection("Authentication:Google", new GoogleOpenIdConnectSettingsValidator());

builder.Services
    .AddAuthentication(o =>
    {
        // This forces challenge results to be handled by Google OpenID Handler, so there's no
        // need to add an AccountController that emits challenges for Login.
        o.DefaultChallengeScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
        // This forces forbid results to be handled by Google OpenID Handler, which checks if
        // extra scopes are required and does automatic incremental auth.
        o.DefaultForbidScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
        // Default scheme that will handle everything else.
        // Once a user is authenticated, the OAuth2 token info is stored in cookies.
        o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddGoogleOpenIdConnect(googleOptions =>
    {
        googleOptions.ClientId = openIdConnectSettings.ClientId;
        googleOptions.ClientSecret = openIdConnectSettings.ClientSecret;
        googleOptions.SignedOutCallbackPath = openIdConnectSettings.SignedOutCallbackPath;
    });

var authorizationSettings = builder
    .GetTypedSection<AuthorizationSettings>("Authorization");

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin",
        policy => policy.RequireClaim(System.Security.Claims.ClaimTypes.Email, authorizationSettings.Admins));
});

var sendGridSettings = builder
    .GetAndValidateTypedSection("SendGrid", new SendGridSettingsValidator());
builder.Services.AddSingleton(sendGridSettings);

var markdownPipeLine = new MarkdownPipelineBuilder()
    .UseAdvancedExtensions()
    .Build();

builder.Services.AddSingleton(markdownPipeLine);

builder.Services.AddSingleton<IMemoryCache, MemoryCache>();
builder.Services.RegisterServicesInAssembly(typeof(SendGridEmailService));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
