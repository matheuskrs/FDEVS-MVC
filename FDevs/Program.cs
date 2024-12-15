// using Microsoft.EntityFrameworkCore;
// using FDevs.Data;

// var builder = WebApplication.CreateBuilder(args);

// // Add services to the container.
// string conexao = builder.Configuration.GetConnectionString("Conexao");
// var versao = ServerVersion.AutoDetect(conexao);
// builder.Services.AddDbContext<AppDbContext>(
//     options => options.UseMySql(conexao, versao)
// );

// builder.Services.AddControllersWithViews();

// var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (!app.Environment.IsDevelopment())
// {
//     app.UseExceptionHandler("/Home/Error");
//     // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//     app.UseHsts();
// }

// app.UseHttpsRedirection();
// app.UseStaticFiles();

// app.UseRouting();

// app.UseAuthorization();

// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Home}/{action=Index}/{id?}");

// app.Run();

//SQL
using FDevs.Data;
using FDevs.Services.EmailService;
using FDevs.Services.EstadoService;
using FDevs.Services.UsuarioService;
using FDevs.Services.VideoService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços ao contêiner.
string conexao = builder.Configuration.GetConnectionString("Conexao");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(conexao)
);

// Configurar Identity com suporte a usuários e papéis.
builder.Services.AddIdentity<IdentityUser, IdentityRole>(opt =>
    opt.SignIn.RequireConfirmedEmail = false
)
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Registrar os serviços necessários.
builder.Services.AddTransient<IUsuarioService, UsuarioService>();
builder.Services.AddTransient<IVideoService, VideoService>();
builder.Services.AddTransient<IEstadoService, EstadoService>();
builder.Services.AddTransient<IEmailSender, EmailSender>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configuração do pipeline de requisição HTTP.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
