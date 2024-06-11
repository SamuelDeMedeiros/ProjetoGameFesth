using ProjetoE_CommerceGameFesth.GerenciaArquivos;
using ProjetoE_CommerceGameFesth.Libraries.Login;
using ProjetoE_CommerceGameFesth.Libraries.Middleware;
using ProjetoE_CommerceGameFesth.Repository;
using ProjetoE_CommerceGameFesth.Repository.Contract;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IFuncionarioRepository, FuncionarioRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IVendaRepository, VendaRepository>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();

builder.Services.AddScoped<ProjetoE_CommerceGameFesth.Libraries.Sessao.Sessao>();
builder.Services.AddScoped<LoginCliente>();
builder.Services.AddScoped<LoginFuncionario>();

//Add Gerenciador Arquivo como servi�os
builder.Services.AddScoped<GerenciadorArquivos>();
builder.Services.AddScoped<ProjetoE_CommerceGameFesth.Cookie.Cookie>();
builder.Services.AddScoped<ProjetoE_CommerceGameFesth.CarrinhoCompra.CookieCarrinhoCompra>();
//builder.Services.Configure<CookiePolicyOptions>(options =>
//{
//    options.CheckConsentNeeded = context => true;
//    options.MinimumSameSitePolicy = SameSiteMode.None;
//});

// Corrigir problema com TEMPDATA para aumentar o tempo de dura��o
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    // Set a short timeout for easy testing. 
    options.IdleTimeout = TimeSpan.FromSeconds(900);
    options.Cookie.HttpOnly = true;
    // Deixar informado para o navegador que a sess�o � essencial
    options.Cookie.IsEssential = true;
});
builder.Services.AddMvc().AddSessionStateTempDataProvider();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseDefaultFiles();
app.UseCookiePolicy();
app.UseSession();
//app.UseMiddleware<ValidateAntiForgeryTokenMiddleware>();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
name: "areas",
pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
name: "default",
pattern: "{controller=Home}/{action=frontpage}/{id?}");

app.Run();

