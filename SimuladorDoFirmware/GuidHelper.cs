
// Busca Configuração
// Se não encontra utiliza a antiga
// Seta a configuração
// Executa a Configuração


public static class GuidHelper
{
    public static string ToShortString(Guid guid)
    {
        var base64Guid = Convert.ToBase64String(guid.ToByteArray());
        base64Guid = base64Guid.Replace('+', '-').Replace('/', '_');
        // Remove the trailing ==
        return base64Guid.Substring(0, base64Guid.Length - 2);
    }

    public static Guid FromShortString(string str)
    {
        str = str.Replace('_', '/').Replace('-', '+');
        var byteArray = Convert.FromBase64String(str + "==");
        return new Guid(byteArray);
    }
}