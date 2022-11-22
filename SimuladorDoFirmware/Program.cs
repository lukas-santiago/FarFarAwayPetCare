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

    public static Device? configuration;
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
                        JsonGetNetworkRequest request = new JsonGetNetworkRequest(new HttpClient(), url);

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
                    ExecutionHelper.RunTemperatura(configuration.DeviceConfig[0].Periodicidade);
                    ExecutionHelper.RunAmonia(configuration.DeviceConfig[0].Periodicidade);
                    ExecutionHelper.RunImagem(configuration.DeviceConfig[0].Periodicidade);
                    ExecutionHelper.RunIluminacao(configuration.DeviceConfig[0].extras);
                    ExecutionHelper.RunTomada(configuration.DeviceConfig[0].extras);
                    ExecutionHelper.RunAlimentacao(configuration.DeviceConfig[0].extras);

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
            double ph = random;
            var data = new DeviceData()
            {
                Nome = ph.ToString().Insert(ph.ToString().Length > 2 ? 2 : 1, ","),
                //DeviceConfig = Program.configuration.DeviceConfig[0],
                DeviceConfigId = (int)Program.configuration.DeviceConfig[0]?.Id,
                CreatedOn = DateTime.Now,
                UpdatedDate = DateTime.Now,
            };
            await AddDeviceData(data);
        }
    }
    public static async Task RunTemperatura(int period)
    {
        var timer = new PeriodicTimer(TimeSpan.FromSeconds(period));

        while (await timer.WaitForNextTickAsync())
        {
            int random = new Random().Next(10, 40);
            double temperatura = random;
            var data = new DeviceData()
            {
                Nome = temperatura.ToString(),
                //DeviceConfig = Program.configuration.DeviceConfig[0],
                DeviceConfigId = (int)Program.configuration.DeviceConfig[1]?.Id,
                CreatedOn = DateTime.Now,
                UpdatedDate = DateTime.Now,
            };
            await AddDeviceData(data);
        }
    }
    public static async Task RunAmonia(int period)
    {
        var timer = new PeriodicTimer(TimeSpan.FromSeconds(period));

        while (await timer.WaitForNextTickAsync())
        {
            int random = new Random().Next(0, 110);
            double amonia = random / 1000;
            var data = new DeviceData()
            {
                Nome = string.Format("{0:N3}%", amonia),
                //DeviceConfig = Program.configuration.DeviceConfig[0],
                DeviceConfigId = (int)Program.configuration.DeviceConfig[2]?.Id,
                CreatedOn = DateTime.Now,
                UpdatedDate = DateTime.Now,
            };
            await AddDeviceData(data);
        }
    }
    public static async Task RunImagem(int period)
    {
        var timer = new PeriodicTimer(TimeSpan.FromSeconds(period));

        while (await timer.WaitForNextTickAsync())
        {
            int random = new Random().Next(0, 100);
            double amonia = random;
            var data = new DeviceData()
            {
                Nome = string.Format("{0}%", amonia),
                //DeviceConfig = Program.configuration.DeviceConfig[0],
                DeviceConfigId = (int)Program.configuration.DeviceConfig[3]?.Id,
                CreatedOn = DateTime.Now,
                UpdatedDate = DateTime.Now,
            };
            await AddDeviceData(data);
        }
    }
    public static async Task RunIluminacao(string extra)
    {
        var timer = new PeriodicTimer(TimeSpan.FromSeconds(60));
        
        int[] timeIniText = extra.Split('-')[0].Split(':').ToList().Select(a => int.Parse(a)).ToArray();
        int[] timeFinalText = extra.Split('-')[1].Split(':').ToList().Select(a => int.Parse(a)).ToArray();
        TimeSpan timeIni = new TimeSpan(timeIniText[0], timeIniText[1], 0);
        TimeSpan timeFinal = new TimeSpan(timeFinalText[0], timeFinalText[1], 0);

        while (await timer.WaitForNextTickAsync())
        {
            var now = DateTime.Now.TimeOfDay;
            int value = 0;

            if (timeIni.Hours == now.Hours && timeIni.Minutes == now.Minutes)
                value = 1;
            else if (timeFinal.Hours == now.Hours && timeFinal.Minutes == now.Minutes)
                value = 1;
            else continue;

            var data = new DeviceData()
            {
                Nome = string.Format("{0}%", value),
                //DeviceConfig = Program.configuration.DeviceConfig[0],
                DeviceConfigId = (int)Program.configuration.DeviceConfig[4]?.Id,
                CreatedOn = DateTime.Now,
                UpdatedDate = DateTime.Now,
            };
            await AddDeviceData(data);
        }
    }
    public static async Task RunTomada(string extra)
    {
        var timer = new PeriodicTimer(TimeSpan.FromSeconds(60));

        int[] timeIniText = extra.Split('-')[0].Split(':').ToList().Select(a => int.Parse(a)).ToArray();
        int[] timeFinalText = extra.Split('-')[1].Split(':').ToList().Select(a => int.Parse(a)).ToArray();
        TimeSpan timeIni = new TimeSpan(timeIniText[0], timeIniText[1], 0);
        TimeSpan timeFinal = new TimeSpan(timeFinalText[0], timeFinalText[1], 0);

        while (await timer.WaitForNextTickAsync())
        {
            var now = DateTime.Now.TimeOfDay;
            int value = 0;

            if (timeIni.Hours == now.Hours && timeIni.Minutes == now.Minutes)
                value = 1;
            else if (timeFinal.Hours == now.Hours && timeFinal.Minutes == now.Minutes)
                value = 0;
            else continue;

            var data = new DeviceData()
            {
                Nome = string.Format("{0}%", value),
                //DeviceConfig = Program.configuration.DeviceConfig[0],
                DeviceConfigId = (int)Program.configuration.DeviceConfig[5]?.Id,
                CreatedOn = DateTime.Now,
                UpdatedDate = DateTime.Now,
            };
            await AddDeviceData(data);
        }
    }
    public static async Task RunAlimentacao(string extra)
    {
        var timer = new PeriodicTimer(TimeSpan.FromSeconds(60));

        int[] timeIniText = extra.Split('-')[0].Split(':').ToList().Select(a => int.Parse(a)).ToArray();
        int[] timeFinalText = extra.Split('-')[1].Split(':').ToList().Select(a => int.Parse(a)).ToArray();
        TimeSpan timeIni = new TimeSpan(timeIniText[0], timeIniText[1], 0);
        TimeSpan timeFinal = new TimeSpan(timeFinalText[0], timeFinalText[1], 0);

        while (await timer.WaitForNextTickAsync())
        {
            var now = DateTime.Now.TimeOfDay;
            int value = 0;

            if (timeIni.Hours == now.Hours && timeIni.Minutes == now.Minutes)
                value = 1;
            else if (timeFinal.Hours == now.Hours && timeFinal.Minutes == now.Minutes)
                value = 1;
            else continue;

            var data = new DeviceData()
            {
                Nome = string.Format("{0}%", value),
                //DeviceConfig = Program.configuration.DeviceConfig[0],
                DeviceConfigId = (int)Program.configuration.DeviceConfig[6]?.Id,
                CreatedOn = DateTime.Now,
                UpdatedDate = DateTime.Now,
            };
            await AddDeviceData(data);
        }
    }

    public static async Task AddDeviceData(DeviceData deviceData)
    {
        var url = "https://localhost:7196/api/DeviceData/FromDevice?device_api_key=" + Program.ApiKey;
        Console.WriteLine(JsonConvert.SerializeObject(deviceData));

        JsonPostNetworkRequest request = new JsonPostNetworkRequest(new HttpClient(), url, JsonConvert.SerializeObject(deviceData));
        var response = await request.ExecuteAsync<dynamic>();
        Console.WriteLine(response);
    }
}