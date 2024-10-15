using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using BlazorTest_Ant.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// 認証サービスを追加(Cookie認証)
builder.Services
  .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
  .AddCookie
  (
    options =>
    {
      // ログインページのURL
      options.LoginPath = "/login";
    }
  );

// 認証サービスを追加(Windwos認証)
//builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate();

// 認可サービスを追加
builder.Services.AddAuthorization(options =>
{
  // AllowAnonymous属性が指定されていない全ての画面でユーザー認証を要求する
  options.FallbackPolicy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();

  // デフォルトのフォールバックポリシーを使う場合
  //options.FallbackPolicy = options.DefaultPolicy;
});

// Blazor用の認証情報を提供するためのコンポーネント
builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();

// 画面で情報を取得するための設定
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpContextAccessor();

// 
builder.Services.AddAntDesign();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

// アプリに認証と認可を追加
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
