using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static System.Int32;

namespace SkyTemple.RogueElementsPMD
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: "SkyTempleCors",
                    builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors("SkyTempleCors");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    var w = ParseDimensionFromParam("w", context.Request.Query);
                    var h = ParseDimensionFromParam("h", context.Request.Query);
                    await context.Response.WriteAsync(Dungeon.Dungeon.Generate(w, h));
                });
            });
        }

        private static int ParseDimensionFromParam(string param, IQueryCollection query, int defaultValue=1, int max=16)
        {
            query.TryGetValue(param, out var raw);
            var result = defaultValue;
            if (raw.Count > 0)
            {
                try
                {
                    result = Parse(raw[0]);
                }
                catch (FormatException exception) { }
            }

            if (result > max)
            {
                return max;
            }

            return result < 1 ? 1 : result;
        }
    }
}
