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

            UseSpa(app, env,
                sourcePath: "ClientApp",
                urlPrefix: "/dist");
        }

        private void UseSpa(IApplicationBuilder app, IHostingEnvironment env,
            string sourcePath, string urlPrefix)
        {
            // If you want to enable server-side prerendering for your app, then: 
            // [1] Edit your application .csproj file and set the BuildServerSideRenderer 
            //     property to 'true' so that the entrypoint file is built on publish 
            // [2] Uncomment this code block
            app.UseSpaPrerendering($"{sourcePath}/dist-server/main.bundle.js", urlPrefix,
                buildOnDemand: env.IsDevelopment() ? new AngularCliBuilder("ssr") : null);

            if (env.IsDevelopment())
            {
                // In development, the wwwroot/dist directory does not need to exist on
                // disk - its files will be built and served dynamically via Angular CLI
                app.UseAngularCliServer(sourcePath, urlPrefix);
            }

            // Any remaining requests will serve 'index.html' so that client-side routing 
            // can take effect 
            app.UseSpaDefaultPage(urlPrefix);
        }
    }
}
