using AutoMapper;
using FairWayTest.Api.Configuration;
using FairWayTest.Api.Features.V1.Accounts;
using FairWayTest.Api.Infrastructure;
using FairWayTest.Api.Infrastructure.AccountProviders.Bizfibank;
using FairWayTest.Api.Infrastructure.AccountProviders.FairWayBank;
using FluentValidation;
using FluentValidation.AspNetCore;
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

            services.AddTransient<IAccountProvider, BizfiBankAccountProvider>();
            services.AddTransient<IAccountProvider, FairWayBankAccountProvider>();
            services.AddSingleton<BizfiBankClient>();

            services.AddMediatR();
            services.AddAutoMapper();
            services.AddMvc()
                    .AddFluentValidation(x => x.RegisterValidatorsFromAssembly(GetType().Assembly));
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
}