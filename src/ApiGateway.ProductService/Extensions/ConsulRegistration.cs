using Consul;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ApiGateway.ProductService.Extensions
{
    public static class ConsulRegistration
    {
        public static IServiceCollection ConfigureConsul(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consuleConfig =>
            {
                var adress = configuration["ConsulConfig:Address"];
                consuleConfig.Address = new Uri(adress);

            }));

            return services;
        }

        public static IApplicationBuilder RegisterWithConsul(this IApplicationBuilder app, IHostApplicationLifetime lifetime)
        {
            var consulClient = app.ApplicationServices.GetRequiredService<ConsulClient>();

            var loggingFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();

            var logger = loggingFactory.CreateLogger<IApplicationBuilder>();

            var features = app.Properties["server.Features"] as FeatureCollection;
            var addresses = features.Get<IServerAddressesFeature>();
            var adress = addresses.Addresses.First();

            //Register service with consul
            var uri = new Uri(adress);
            var registration = new AgentServiceRegistration()
            {
                ID = $"ApiGateway.OrderService",
                Name = "ApiGateway.OrderService",
                Address = $"{uri.Host}",
                Port = uri.Port,
                Tags = new[] { "Order Service", "Order" }
            };

            logger.LogInformation("Registering with Consul");
            consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            consulClient.Agent.ServiceRegister(registration).Wait();


            lifetime.ApplicationStopping.Register(() =>
            {
                logger.LogInformation("deregistering with Consul");
                consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            });

            return app;
        }
    }
}
