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

// �F�؃T�[�r�X��ǉ�(Cookie�F��)
builder.Services
  .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
  .AddCookie
  (
    options =>
    {
      // ���O�C���y�[�W��URL
      options.LoginPath = "/login";
    }
  );

// �F�؃T�[�r�X��ǉ�(Windwos�F��)
//builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate();

// �F�T�[�r�X��ǉ�
builder.Services.AddAuthorization(options =>
{
  // AllowAnonymous�������w�肳��Ă��Ȃ��S�Ẳ�ʂŃ��[�U�[�F�؂�v������
  options.FallbackPolicy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();

  // �f�t�H���g�̃t�H�[���o�b�N�|���V�[���g���ꍇ
  //options.FallbackPolicy = options.DefaultPolicy;
});

// Blazor�p�̔F�؏���񋟂��邽�߂̃R���|�[�l���g
builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();

// ��ʂŏ����擾���邽�߂̐ݒ�
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

// �A�v���ɔF�؂ƔF��ǉ�
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
