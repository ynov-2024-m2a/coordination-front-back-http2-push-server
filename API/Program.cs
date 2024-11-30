using API;
using API.ServiceImpl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();
builder.Services.AddSingleton<JwtService>();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, option =>
    {
        option.TokenValidationParameters = new TokenValidationParameters
        {
            // se qu'on veut valider ou non
            ValidateIssuer = false,
            ValidateAudience = false
        };

        // permet de valider le chiffrement du JWT en definissant la clé utilisée
        option.Configuration = new OpenIdConnectConfiguration
        {
            SigningKeys = { new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtService.CLE)) }
        };

        // pour avoir les cl? valeur normal comme dans les claims
        // par defaut ajouter des Uri pour certain truc comme le "sub"
        option.MapInboundClaims = false;
    });

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcService<PersonneServiceImpl>();
app.MapGrpcService<MathServiceImpl>();

app.Run();
