using Visus.Cuid;

namespace Application.Utilities;

public static class ShortenedPathGenerator
{
    public static string GenerateShortenedPath()
    {
        var symbol = Cuid.NewCuid().ToString();
        var stringChars = new char[6];
        var random = new Random();

        for (int i = 0; i < stringChars.Length; i++)
        {
            stringChars[i] = symbol[random.Next(symbol.Length)];
        }

        return new string(stringChars);
    }
}