using AutoMapper;
using FairWayTest.Api.Features.V1.Users;
using FairWayTest.Api.Features.V1.Users.Validators;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace FairWayTest.Api
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
            new MongoConfigurator().Configure();
            var fairwayConfigSection = Configuration.GetSection("FairWayTest.Api");

            services.AddSingleton(x =>
            {
                var mongoConfiguration = fairwayConfigSection.Get<MongoConfiguration>();
                var mongoUrl = MongoUrl.Create(mongoConfiguration.ConnectionString);
                var client = new MongoClient(mongoUrl);

                var database = client.GetDatabase(mongoUrl.DatabaseName);

                return database;
            });

            services.AddTransient<IValidator<BankDetails>, BankDetailsValidator>();
            services.AddTransient<IValidator<CreateUser.Command>, CreateUserValidator>();

            services.AddMediatR();
            services.AddAutoMapper();
            services.AddMvc();
            services.AddApiVersioning(x =>
            {
                x.ApiVersionReader = new HeaderApiVersionReader("Api-Version");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseMvc();
        }
    }

    public class MongoConfiguration
    {
        public string ConnectionString { get; set; }
    }
}