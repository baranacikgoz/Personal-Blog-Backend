namespace Application.Interfaces;

public interface IHashIdService
{
    public string Encode(int rawInt);

    public int Decode(string hashedId);
}