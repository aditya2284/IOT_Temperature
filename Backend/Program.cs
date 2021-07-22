using System;
using System.IO;
using Grpc.Core;
using IOT_Temperature.Interface;
using IOT_Temperature.Services;
using Microsoft.Extensions.Configuration;
using Temperatures;

namespace IOT_Temperature
{
    class Program
    {
        const int Port = 30051;
        private static IConfigurationRoot config;
        public static void Main(string[] args)
        {
            Initialize();
            Server server = new Server
            {
                Services = { TemperatureSensing.BindService(new IOT_Temperature.Services.TemperatureService(CreateRepository()))},
                Ports = { new ServerPort("0.0.0.0", Port, ServerCredentials.Insecure) }
            };
            server.Start();

            Console.WriteLine("Temperature server listening on port " + Port);
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            server.ShutdownAsync().Wait();
        }
        private static void Initialize()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).
                AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            config = builder.Build();
        }
        private static ITemperatureRepository CreateRepository()
        {
            return new TemperatureDapperRepository(config.GetConnectionString("DefaultConnection"));
        }
    }
}
