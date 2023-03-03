//
//  Program.cs
//
//  Author:
//       LuzFaltex Contributors
//
//  LGPL-3.0 License
//
//  Copyright (c) 2022 LuzFaltex
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System.Diagnostics;
using System.Net;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using ProtoBuf.Grpc.Server;
using Selkhound.Server.Extensions;
using Selkhound.Server.Services;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Selkhound.Server
{
    /// <summary>
    /// The main program entry point.
    /// </summary>
    [UsedImplicitly]
    internal class Program
    {
        private const string LogOutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}";

        private static readonly string AppDir =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LuzFaltex", "Selkhound");

        private static readonly string LogDir = Path.Combine(AppDir, "Logs");

        private static async Task<int> Main(string[] args)
        {
            var fvi = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);

            Console.WriteLine($"Selkhound version {fvi.FileVersion}");
            Console.WriteLine(fvi.LegalCopyright);

            var builder = WebApplication.CreateBuilder(args);

            // Configuration
            builder.Configuration.AddEnvironmentVariables("Selkhound_");
            builder.Configuration.AddJsonFile("appsettings.json", optional: true);
            builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

            if (builder.Environment.IsDevelopment())
            {
                builder.Configuration.AddUserSecrets<Program>();
            }

            // Logging
            Log.Logger = new LoggerConfiguration()
                         .ReadFrom.Configuration(builder.Configuration)
                         .Enrich.FromLogContext()
                         .Enrich.WithEnvironmentName()
                         .WriteTo.Console(outputTemplate: LogOutputTemplate, theme: AnsiConsoleTheme.Code)
                         .WriteTo.File
                             (Path.Combine(LogDir, "_Execution.log"), outputTemplate: LogOutputTemplate)
                         .CreateLogger();
            builder.Host.UseSerilog();

            // Services
            builder.Services.AddSelkhound();

            // Add services to the container.
            builder.Services.AddCodeFirstGrpc
            (
                config =>
                {
                    config.ResponseCompressionLevel = System.IO.Compression.CompressionLevel.Optimal;
                }
            );
            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization();

            var app = builder.Build();

            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapGrpcService<GreeterService>();

            // Configure the HTTP request pipeline.
            app.MapGet
            (
                "/",
                async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.PermanentRedirect;
                    context.Response.Headers.Add("Location", "https://developer.selkhound.com");
                    await context.Response.WriteAsync("Communication with with this API must be done through a registered Selkhound client. To learn how to create a client, visit: https://developer.selkhound.com.");
                }
            );

            try
            {
                Log.Information("Running Database Migrations");
                var dbContext = app.Services.GetRequiredService<SelkhoundContext>();
                await dbContext.Database.MigrateAsync();

                Log.Information("Starting web host");
                await app.RunAsync();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return ex.HResult;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
