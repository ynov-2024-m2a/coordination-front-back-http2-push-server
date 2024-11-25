
namespace API.ServiceImpl;

using Grpc.Core;
using MathPerso;
using System.Threading.Tasks;
using static MathPerso.MathPerso;

public class MathServiceImpl: MathPersoBase
{
    public override async Task Factorielle(Nombre request, IServerStreamWriter<Resultat> responseStream, ServerCallContext context)
    {
        long total = 1;
        for (int i = 1; i < request.Chiffre; i++)
        {
            total *= i;

            // equivaux a un return du stream vers le client
            await responseStream.WriteAsync(new Resultat
            {
                Total = total
            });
        }
    }

    public override async Task<Resultat> Addiction(IAsyncStreamReader<Nombre> requestStream, ServerCallContext context)
    {
        int total = 0;

        while(await requestStream.MoveNext())
            total += requestStream.Current.Chiffre;

        return new Resultat { Total = total };
    }

    public override async Task Calcul(IAsyncStreamReader<Nombre> requestStream, IServerStreamWriter<Resultat> responseStream, ServerCallContext context)
    {
        int total = 0;

        while (await requestStream.MoveNext())
        {
            total += requestStream.Current.Chiffre;
            await responseStream.WriteAsync(new Resultat { Total = total });
        }
    }
}
