using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PrimeiraAPI;
using PrimeiraAPI.Application.Mapping;
using PrimeiraAPI.Domain.Model.EmployeeAggregate;
using PrimeiraAPI.Infrastructure.Repositories;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Para fazer o processo de mapeamento.
builder.Services.AddAutoMapper(typeof(DomainToDTOMapping));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

#region Configuração do Swagger
builder.Services.AddSwaggerGen(c =>
{
    // c.OperationFilter<SwaggerDefaultValues>();

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
    {
        new OpenApiSecurityScheme
        {
        Reference = new OpenApiReference
            {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
            },
            Scheme = "oauth2",
            Name = "Bearer",
            In = ParameterLocation.Header,

        },
        new List<string>()
        }
    });

});
#endregion

#region Injeção de Dependência
builder.Services.AddTransient<IEmployeeRepository, EmployeeRepository>();
#endregion

#region Utilização do JWT para Autenticação com o TOKEN
var key = Encoding.ASCII.GetBytes(Key.Secret);

builder.Services.AddAuthentication(x =>
// Falando pra usar o JwT
 {
     x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
     x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
 }).AddJwtBearer(x =>
 {
     x.RequireHttpsMetadata = false;
     x.SaveToken = true;
     x.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuerSigningKey = true,
         IssuerSigningKey = new SymmetricSecurityKey(key),
         ValidateIssuer = false,
         ValidateAudience = false
     };
 });
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
// Configurando o erro que vai aparecer de acordo com o ambiente .
#region Tratamento de erro
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error-development");
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/error");
}
#endregion 

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
