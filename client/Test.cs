using Grpc.Core;
using static MathPerso.MathPerso;
using static Personnes.Personne;

namespace client;
public class Test(Channel _channel)
{
    public async Task StreamClientAsync()
    {
        Console.WriteLine("----- STREAM COTE CLIENT -----");
        var mathPersoServ = new MathPersoClient(_channel);

        var stream = mathPersoServ.Addiction();
        int saisi;

        do
        {
            Console.WriteLine("chiffre: ");
            saisi = int.Parse(Console.ReadLine()!);

            await stream.RequestStream.WriteAsync(new MathPerso.Nombre
            {
                Chiffre = saisi
            });

        } while (saisi is not 0);

        await stream.RequestStream.CompleteAsync();

        var reponse = await stream;

        Console.WriteLine($"resultat: {reponse.Total}");
    }

    public async Task StreamServerAsync()
    {
        Console.WriteLine("----- STREAM COTE SERVEUR -----");

        var mathPersoServ = new MathPersoClient(_channel);

        var stream = mathPersoServ.Factorielle(new MathPerso.Nombre
        {
            Chiffre = 10
        }).ResponseStream;

        while (await stream.MoveNext())
        {
            Console.WriteLine(stream.Current.Total);
        }
    }

    public async Task StreamBidirectionnelleAsync()
    {
        Console.WriteLine("----- STREAM BIDIRECTIONNELLE -----");

        var mathPersoServ = new MathPersoClient(_channel);

        var stream = mathPersoServ.Calcul();

        _ = Task.Run(async () =>
        {
            while (await stream.ResponseStream.MoveNext())
                Console.WriteLine($"Resultat: {stream.ResponseStream.Current.Total}");
        });

        int saisi;

        do
        {
            Console.WriteLine("chiffre: ");
            saisi = int.Parse(Console.ReadLine()!);

            await stream.RequestStream.WriteAsync(new MathPerso.Nombre
            {
                Chiffre = saisi
            });

        } while (saisi is not 0);

        await stream.RequestStream.CompleteAsync();
    }

    public async Task GenererJwt()
    {
        
        Console.WriteLine("----- JWT -----");

        var client = new PersonneClient(_channel);

        var retour = await client.ConnexionAsync(new Personnes.PersonneConnexionRequete
        {
            Mail = "a@a.com"
        });

        Console.WriteLine(retour.Jwt);

        Console.WriteLine("----- UTILISATION ROUTE AUTHENTIFICATION -----");

        Metadata hearder = new()
        {
           { "Authorization", $"Bearer {retour.Jwt}" }
        };

        try
        {
            var retour2 = await client.ProtegerAsync(
                new Google.Protobuf.WellKnownTypes.Empty(),
                hearder
            );

            Console.WriteLine(retour2.Id);
        }
        catch (RpcException ex)
        {
            Console.WriteLine(ex.Message + " " + ex.StatusCode);
        }
    }
}
