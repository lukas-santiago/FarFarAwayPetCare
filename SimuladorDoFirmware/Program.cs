using System.Dynamic;
using System.Net;
using System.Net.Http.Json;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading;
using MADE.Networking.Http.Requests.Json;
using Newtonsoft.Json;
using SimuladorDoFirmware.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SimuladorDoFirmware;
class Program
{

    public static Device configuration;
    public static string ApiKey = "MinhaApiKeyTop";
    public static Guid UniqueDeviceId = Guid.Parse("fd8d872d-3045-47c8-8b24-9db2d4807b7c"); //Guid.NewGuid();
    public static string shortUniqueDeviceId = GuidHelper.ToShortString(UniqueDeviceId);

    static async Task Main(string[] args)
    {
        Console.WriteLine("Guid: " + UniqueDeviceId.ToString());
        Console.WriteLine("Short Guid: " + shortUniqueDeviceId);
        Console.WriteLine("ApiKey: " + ApiKey);

        Estado estado = Estado.Configurando;


        var jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        while (true)
        {
            switch (estado)
            {
                case Estado.Configurando:
                    {
                        Console.WriteLine("\nConfigurando");
                        string url = string.Format("https://localhost:7196/api/Device/configuracao/{0}?device_api_key={1}", UniqueDeviceId.ToString(), ApiKey);
                        JsonGetNetworkRequest request = new JsonGetNetworkRequest(new HttpClient(),url);

                        var response = await request.ExecuteAsync<Device?>();

                        Console.WriteLine(response?.ToString());
                        Console.WriteLine(JsonSerializer.Serialize(response, jsonOptions));

                        if (response == null)
                            estado = Estado.StandBy;
                        else
                        {
                            configuration = response;
                            estado = Estado.Executando;
                        }
                        break;
                    }
                case Estado.Executando:
                    Console.WriteLine("Executando...");

                    ExecutionHelper.RunPH(configuration.DeviceConfig[0].Periodicidade);
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
public static class ExecutionHelper
{
    public static async Task RunPH(int period)
    {
        var timer = new PeriodicTimer(TimeSpan.FromSeconds(period));

        while (await timer.WaitForNextTickAsync())
        {
            int random = new Random().Next(0, 140);
            double ph = random / 100;
            var data = new DeviceData()
            {
                Nome = ph.ToString(),
                DeviceConfig = Program.configuration.DeviceConfig[0],
                DeviceConfigId = (int)Program.configuration.DeviceConfig[0]?.Id,
            };
            await AddDeviceData(data);
        }
    }

    public static async Task AddDeviceData(DeviceData deviceData)
    {
        var url = "https://localhost:7196/api/DeviceData/FromDevice?device_api_key=" + Program.ApiKey;

        JsonPostNetworkRequest request = new JsonPostNetworkRequest(new HttpClient(), url, JsonConvert.SerializeObject(deviceData));
        var response = await request.ExecuteAsync<dynamic>();
        Console.WriteLine(response);
    }
}