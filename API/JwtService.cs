using Microsoft.IdentityModel.Tokens;
using Personnes;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API;

public class JwtService
{
    public const string CLE = "fjfnvcjdlzsmzsmpwacncbdvgsfzhdlcocudvzsbcjcldk";

    public string Generer(PersonneConnexionRequete requete)
    {
        var h = new JwtSecurityTokenHandler();

        var cle = Encoding.UTF8.GetBytes(CLE);

        Claim[] listeClaim = [
            new("mail", requete.Mail)
        ];

        var description = new SecurityTokenDescriptor
        {
            Audience = "moi",
            Issuer = "moi",
            Subject = new ClaimsIdentity(listeClaim),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(cle), 
                SecurityAlgorithms.HmacSha256
            )
        };

        var jwt = h.CreateToken(description);

        return h.WriteToken(jwt);
    }
}
