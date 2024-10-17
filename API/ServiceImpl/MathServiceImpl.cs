
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
}
