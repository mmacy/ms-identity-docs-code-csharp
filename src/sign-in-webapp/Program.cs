// <ms_docref_import_types>
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
// </ms_docref_import_types>

var builder = WebApplication.CreateBuilder(args);

var initialScopes = builder.Configuration["DownstreamApi:Scopes"]?.Split(' ');

// <ms_docref_add_msal>
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
        .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
            .AddDownstreamWebApi("DownstreamApi", builder.Configuration.GetSection("DownstreamApi"))
            .AddInMemoryTokenCaches();
// </ms_docref_add_msal>

// <ms_docref_add_default_authz_policies>
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;
});
// </ms_docref_add_default_authz_policies>
builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// <ms_docref_enable_authn_capabilities>
app.UseAuthentication();
// </ms_docref_enable_authn_capabilities>
// <ms_docref_enable_authz_capabilities>
app.UseAuthorization();
// </ms_docref_enable_authz_capabilities>

app.MapRazorPages();
app.MapControllers();

app.Run();
