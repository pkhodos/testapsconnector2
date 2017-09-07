using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;

namespace FallballConnectorDotNet
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var configFile = Environment.GetEnvironmentVariable("CONFIG_FILE");

            var version = System.IO.File.ReadAllText("VERSION").Replace("\n", "");
            var dictVersion = new Dictionary<string, string>()
            {
                ["version"] = version
            }; 
                
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddYamlFile(configFile ?? "config.yml", optional: false)
                .AddInMemoryCollection(dictVersion)
                .AddEnvironmentVariables();
            
            Configuration = builder.Build();

            if (!CheckConfig(Configuration))
            {
                throw new Exception( "You can't run your connector with default " +
                "parameters, please update the YML config " +
                "file and replace PUT_HERE_* values with real ones" );
            }
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);
            
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            // Add framework services.
            services.AddMvc(
                config =>
                {
                    config.Filters.Add(new OAuthValidationFilter(Configuration));
                }
                )
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver()); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddAzureWebAppDiagnostics();

            app.UseMiddleware(typeof(InMiddleware));
            
            app.UseMvc();
        }

        private static bool CheckConfig(IConfiguration config)
        {
            var l = new List<string>
            {
                "fallball_service_url",
                "fallball_service_authorization_token",
                "oauth_key",
                "oauth_secret"
            };

            return l.All(s => !config[s].StartsWith("PUT_HERE_"));
        }
    }
}
