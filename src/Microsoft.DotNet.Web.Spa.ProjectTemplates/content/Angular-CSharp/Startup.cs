using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AngularSpa
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseWebSockets();

            app.UseSpa("/dist", configure: () =>
            {
                /*
                // If you want to enable server-side prerendering for your app, then: 
                // [1] Edit your application .csproj file and set the BuildServerSideRenderer 
                //     property to 'true' so that the entrypoint file is built on publish 
                // [2] Uncomment this code block
                app.UseSpaPrerendering("ClientApp/dist-server/main.bundle.js",
                    buildOnDemand: env.IsDevelopment() ? new AngularCliBuilder("ssr") : null,
                    excludeUrls: new[] { "/dist", "/sockjs-node" });
                */

                if (env.IsDevelopment())
                {
                    app.UseAngularCliServer(sourcePath: "ClientApp");
                }
            });
        }
    }
}
