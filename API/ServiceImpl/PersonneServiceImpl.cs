using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Personnes;
using static Personnes.Personne;

namespace API.ServiceImpl;

public class PersonneServiceImpl(JwtService _jwtServ): PersonneBase
{
    public async override Task<PersonneReponse> Ajouter(PersonneAjouterRequete request, ServerCallContext context)
    {
        return await Task.FromResult(new PersonneReponse
        {
            Id = 1
        });    
    }

    public override async Task<PersonneConnexionReponse> Connexion(PersonneConnexionRequete request, ServerCallContext context)
    {
        return await Task.FromResult(new PersonneConnexionReponse
        {
            Jwt = _jwtServ.Generer(request)
        });
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public override Task<PersonneReponse> Proteger(Empty request, ServerCallContext context)
    {
        return Task.FromResult(new PersonneReponse { Id = 1 });
    }
}
