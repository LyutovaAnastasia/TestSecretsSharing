using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SecretsSharing.Data;
using SecretsSharing.Data.Models;
using SecretsSharing.Data.Repository;
using SecretsSharing.Data.Repository.impl;
using SecretsSharing.DTO;
using SecretsSharing.Service;
using SecretsSharing.Service.impl;

namespace SecretsSharing
{
    public class Startup
    {
        public string DbConnectionString { get; set; }
        public Startup(IConfiguration configuration)
        {
            //var builder = new ConfigurationBuilder().AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "Properties", "launchSettings.json"));
            //Configuration = builder.Build();
           Configuration = configuration;
           DbConnectionString = Configuration.GetConnectionString("DbConnectionString");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddSingleton<IConfiguration>(Configuration);


            services.AddControllers();
            services.AddTransient<IFileRepository<TextFile>, TextFileRepository>();
            services.AddTransient<IFileRepository<DocumentFile>, DocumentFileRepository>();
            //services.AddTransient<IService<TextFileDTO, UrlResponse> , TextFileService>();
            services.AddTransient<DocumentFileService>();
            services.AddTransient<TextFileService>();
            services.AddTransient<UserRepository>();

            // Config DBContext with PostgreSQl
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(DbConnectionString));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SecretsSharing", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SecretsSharing v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
