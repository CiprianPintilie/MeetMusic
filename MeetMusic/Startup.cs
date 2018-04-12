using Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MeetMusic
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", reloadOnChange: true, optional: false)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", reloadOnChange: true, optional: true);
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MeetMusicDbContext>(options =>
            {
                options.UseMySQL(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("MeetMusic"));
            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(ConfigureRoute);
        }

        private void ConfigureRoute(IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute(
                name: "index",
                template: "index",
                defaults: new { controller = "Home", action = "Index" });

            routeBuilder.MapRoute(
                name: "Default",
                template: "{controller}/{action}/{id?}");
        }

    }
}
