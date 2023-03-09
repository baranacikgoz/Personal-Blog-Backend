using HashidsNet;
using Microsoft.Extensions.Configuration;
using Application.Interfaces;

namespace Infrastructure.Shared;

public class HashIdService : IHashIdService
{
    private readonly Hashids _hashIds;

    public HashIdService(IConfiguration configuration)
    {
        _hashIds = new Hashids(
            configuration.GetValue<string>("HashIdService:Seed"),
            configuration.GetValue<int>("HashIdService:MinGeneratedLength")
            );
    }

    public string Encode(int rawInt) => _hashIds.Encode(rawInt);

    public int Decode(string hashedId) => _hashIds.DecodeSingle(hashedId);
}