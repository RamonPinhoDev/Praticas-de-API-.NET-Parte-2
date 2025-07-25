using APICatologo.Data;
using APICatologo.DTOs.Mappins;
using APICatologo.Extensions;
using APICatologo.Filter;
using APICatologo.Interfaces;
using APICatologo.Logging;
using APICatologo.Models;
using APICatologo.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string Conexao = builder.Configuration.GetConnectionString("DefaultConnection");
var OrigemComacessoPeritido = "_origemComAcessoPermitido";
var Valor1 = builder.Configuration["chave1"];
var Valor2 = builder.Configuration["chave2"];
builder.Services.AddCors(options => options./*AddPolicy*/ AddDefaultPolicy( /*name: OrigemComacessoPeritido, */policy=> { policy.WithOrigins("http://www.apirequest.io", "http://www.apirequest.io")
    .WithMethods("GET", "POST").AllowAnyHeader().AllowCredentials(); } ));


builder.Services.AddDbContext<AppDbContext>(options =>  options.UseSqlServer(Conexao));
builder.Services.AddControllers(options => { options.Filters.Add(typeof(ApiExeceptionFilter)); }).AddJsonOptions(options=> options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles).AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddTransient<IMeuServoco, MeuServico>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "apicatalogo", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme() { Name = "Authorization",
    Type =SecuritySchemeType.ApiKey,
    Scheme = "Bearer",
    BearerFormat = "JWT",
    In = ParameterLocation.Header,
    Description = "Bearer JWT"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement { {new OpenApiSecurityScheme {Reference = new OpenApiReference{
    Type = ReferenceType.SecurityScheme,
    Id = "Bearer"
    } 
    },new string []{ }
    } }
    );
    }

);
builder.Services.AddScoped<ApiLogginFilter>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IProdutosRepository, ProdutosRepository>();
builder.Services.AddScoped(typeof( IRepository<>), typeof( Repository<>));
builder.Services.AddAutoMapper(typeof(ProdutoDTOMappingProfile));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
//adicionando JWT
builder.Services.AddAuthorization(options => { options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
options.AddPolicy("SuperAdminOnly", polyce => polyce.RequireRole("SuperAdmin").RequireClaim("id", "ramon"));
options.AddPolicy("UserOnly", polyce => polyce.RequireRole("User"));

options.AddPolicy("ExclusiveOnLy", polyce=> polyce.RequireAssertion(context=> context.User.HasClaim(claim=> claim.Type == "id"&& claim.Value =="ramon" )
|| context.User.IsInRole("SuperAdmin")
);


});
//builder.Services.AddAuthentication("Bearer").AddJwtBearer();
//Configurando JWT
var secretkey = builder.Configuration["JWT:Secretkey"] ?? throw new ArgumentException("Inavalid secret key");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true; options.RequireHttpsMetadata = false; options.TokenValidationParameters = new
    TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretkey)),
    };
});

builder.Services.AddScoped<ITokenServices, TokenServices>();

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
app.UseAuthentication();
app.UseAuthorization();
app.UseCors(/*OrigemComacessoPeritido*/);
app.MapControllers();
app.Use(async (context,next) =>
{
    //adiciona código antes do request
    await next(context);
    //adicionar um código depois do request 
});
app.Run();
