using Grpc.Core;
using Personnes;
using static Personnes.Personne;

namespace API.ServiceImpl;

public class PersonneServiceImpl: PersonneBase
{
    public async override Task<PersonneReponse> Ajouter(PersonneAjouterRequete request, ServerCallContext context)
    {
        return await Task.FromResult(new PersonneReponse
        {
            Id = 1
        });    
    }
}
