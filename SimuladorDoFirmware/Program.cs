using System.Drawing;
using System.Drawing.Imaging;
using System.Dynamic;
using System.Net;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
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
    public static string host = "https://farfarawaypetcare-api.azurewebsites.net/"; //"https://localhost:7196/"

    static async Task Main(string[] args)
    {
        Console.WriteLine("Guid: " + UniqueDeviceId.ToString());
        Console.WriteLine("Short Guid: " + shortUniqueDeviceId);
        Console.WriteLine("ApiKey: " + ApiKey);
        Console.WriteLine("Host: " + host);

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
                        string url = string.Format(host + "api/Device/configuracao/{0}?device_api_key={1}", UniqueDeviceId.ToString(), ApiKey);
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
                    ExecutionHelper.RunTemperatura(configuration.DeviceConfig[1].Periodicidade);
                    ExecutionHelper.RunAmonia(configuration.DeviceConfig[2].Periodicidade);
                    ExecutionHelper.RunImagem(configuration.DeviceConfig[3].Periodicidade);
                    ExecutionHelper.RunIluminacao(configuration.DeviceConfig[4].extras);
                    ExecutionHelper.RunTomada(configuration.DeviceConfig[5].extras);
                    ExecutionHelper.RunAlimentacao(configuration.DeviceConfig[6].extras);

                    estado = Estado.Off;
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
            double ph = random / (double)100;
            var data = new DeviceData()
            {
                Value = ph,
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
            int random = new Random().Next(100, 400);
            double temperatura = random / (double)10;
            var data = new DeviceData()
            {
                Value = temperatura,
                Nome = "ºC",
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
            double amonia = random / (double)1000;
            var data = new DeviceData()
            {
                Value = amonia,
                Nome = " ppm",
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
                Value = amonia,
                Nome = GenerateBase64ImageString(),
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
            string nome = "";

            if (timeIni.Hours == now.Hours && timeIni.Minutes == now.Minutes)
            {
                value = 1;
                nome = timeIni.ToString();
            }
            else if (timeFinal.Hours == now.Hours && timeFinal.Minutes == now.Minutes)
            {
                value = 0;
                nome = timeIni.ToString();
            }
            else continue;

            var data = new DeviceData()
            {
                Value = value,
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
            string nome = "";

            if (timeIni.Hours == now.Hours && timeIni.Minutes == now.Minutes)
            {
                value = 1;
                nome = timeIni.ToString();
            }
            else if (timeFinal.Hours == now.Hours && timeFinal.Minutes == now.Minutes)
            {
                value = 0;
                nome = timeIni.ToString();
            }
            else continue;

            var data = new DeviceData()
            {
                Value = value,
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
            string nome = "";

            if (timeIni.Hours == now.Hours && timeIni.Minutes == now.Minutes)
            {
                value = 1;
                nome = timeIni.ToString();
            }
            else if (timeFinal.Hours == now.Hours && timeFinal.Minutes == now.Minutes)
            {
                value = 1;
                nome = timeFinal.ToString();
            }
            else continue;

            var data = new DeviceData()
            {
                Value = value,
                Nome = nome,
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
        var url = Program.host + "/api/DeviceData/FromDevice?device_api_key=" + Program.ApiKey;
        Console.WriteLine(JsonConvert.SerializeObject(deviceData));

        JsonPostNetworkRequest request = new JsonPostNetworkRequest(new HttpClient(), url, JsonConvert.SerializeObject(deviceData));
        var response = await request.ExecuteAsync<dynamic>();
        Console.WriteLine(response);
    }

    public static string GenerateBase64ImageString()
    {
        // 1. Create a bitmap
        using (Bitmap bitmap = new Bitmap(80, 20, PixelFormat.Format24bppRgb))
        {
            // 2. Get access to the raw bitmap data
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);

            // 3. Generate RGB noise and write it to the bitmap's buffer.
            // Note that we are assuming that data.Stride == 3 * data.Width for simplicity/brevity here.
            byte[] noise = new byte[data.Width * data.Height * 3];
            new Random().NextBytes(noise);
            Marshal.Copy(noise, 0, data.Scan0, noise.Length);

            bitmap.UnlockBits(data);

            // 4. Save as JPEG and convert to Base64
            using (MemoryStream jpegStream = new MemoryStream())
            {
                bitmap.Save(jpegStream, ImageFormat.Jpeg);
                return Convert.ToBase64String(jpegStream.ToArray());
            }
        }
    }
}