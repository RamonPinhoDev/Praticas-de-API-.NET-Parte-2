using APICatologo.Data;
using APICatologo.DTOs.Mappins;
using APICatologo.Extensions;
using APICatologo.Filter;
using APICatologo.Interfaces;
using APICatologo.Logging;
using APICatologo.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string Conexao = builder.Configuration.GetConnectionString("DefaultConnection");

var Valor1 = builder.Configuration["chave1"];
var Valor2 = builder.Configuration["chave2"];
builder.Services.AddDbContext<AppDbContext>(options =>  options.UseSqlServer(Conexao));
builder.Services.AddControllers(options => { options.Filters.Add(typeof(ApiExeceptionFilter)); }).AddJsonOptions(options=> options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles).AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddTransient<IMeuServoco, MeuServico>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ApiLogginFilter>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IProdutosRepository, ProdutosRepository>();
builder.Services.AddScoped(typeof( IRepository<>), typeof( Repository<>));
builder.Services.AddAutoMapper(typeof(ProdutoDTOMappingProfile));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
//adicionando JWT
builder.Services.AddAuthorization();
builder.Services.AddAuthentication("Bearer").AddJwtBearer();
//Configurando JWT
var secretkey = builder.Configuration["JWT:Secretkey"] ?? throw new ArgumentException("Inavalid secret key");
builder.Services.AddAuthentication(options => { options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => { options.SaveToken = true; options.RequireHttpsMetadata = false; options.TokenValidationParameters = new
    TokenValidationParameters()
{ ValidateIssuer = true,
ValidateAudience = true,
ValidateLifetime = true,
ValidateIssuerSigningKey = true,
ClockSkew = TimeSpan.Zero,
ValidAudience = builder.Configuration["JWT:ValidAudience"],
ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretkey)),
};
});


// função para desabilitar o [FromServices]
builder.Services.Configure<ApiBehaviorOptions>(options => options.DisableImplicitFromServicesParameters = true);
builder.Logging.AddProvider(new CustummerLoggerProvider(new CustummerLoggerProviderConfiguration() { LogLevel = LogLevel.Information }));
   
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ConfigureExceptionHandler();
    //app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
//app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Use(async (context,next) =>
{
    //adiciona código antes do request
    await next(context);
    //adicionar um código depois do request 
});
app.Run();
