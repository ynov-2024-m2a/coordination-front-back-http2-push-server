using client;
using Grpc.Core;

Channel channel = new("127.0.0.1:5113", ChannelCredentials.Insecure);

await channel.ConnectAsync();

Test test = new(channel);

 await test.GenererJwt();
