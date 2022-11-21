using System.Dynamic;
using System.Net.Http.Json;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

internal class Program
{
    private static async Task Main(string[] args)
    {
        string ApiKey = "MinhaApiKeyTop";
        Guid UniqueDeviceId = Guid.Parse("fd8d872d-3045-47c8-8b24-9db2d4807b7c"); //Guid.NewGuid();
        string shortUniqueDeviceId = GuidHelper.ToShortString(UniqueDeviceId);


        Console.WriteLine("Guid: " + UniqueDeviceId.ToString());
        Console.WriteLine("Short Guid: " + shortUniqueDeviceId);
        Console.WriteLine("ApiKey: " + ApiKey);

        Estado estado = Estado.Configurando;

        dynamic configuration;

        var jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        // Busca Configuração
        // Seta a configuração
        // Executa a Configuração

        while (true)
        {
            switch (estado)
            {
                case Estado.Configurando:
                    {
                        Console.WriteLine("\nConfigurando");
                        string requestUri = string.Format("https://localhost:7196/api/Device/configuracao/{0}?device_api_key={1}", UniqueDeviceId.ToString(), ApiKey);
                        HttpClient client = new()
                        {
                            BaseAddress = new Uri(requestUri)
                        };

                        HttpResponseMessage httpResponse = await client.GetAsync(requestUri);
                        ExpandoObject? response = await client.GetFromJsonAsync<ExpandoObject>(requestUri, jsonOptions);

                        Console.WriteLine(httpResponse.ToString());
                        Console.WriteLine(JsonSerializer.Serialize(response, jsonOptions));

                        if (response == null)
                            estado = Estado.StandBy;
                        else
                        {
                            configuration = response!;
                            estado = Estado.Executando;
                        }
                        break;
                    }
                case Estado.Executando:
                    Console.WriteLine("Executando...");
                    break;
                case Estado.StandBy:
                    Console.WriteLine(@"Não encontrada uma configuração válida. Entrando no modo StandBy.
                                        \nDigite qualquer tecla para tentar novamente.");
                    Console.ReadKey();
                    estado = Estado.Configurando;
                    break;
                default:
                    break;
            }
            Thread.Sleep(10000);
        }
    }
}