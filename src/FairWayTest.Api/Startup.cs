using AutoMapper;
using FairWayTest.Api.Configuration;
using FairWayTest.Api.Features.V1.Accounts;
using FairWayTest.Api.Infrastructure;
using FairWayTest.Api.Infrastructure.AccountProviders;
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

            services.AddTransient<IAccountProvider>(x => new BizfiBankAccountProvider(x.GetService<BizfiBankClient>(), x.GetService<IMapper>()));
            services.AddTransient<IAccountProvider>(x => new FairWayBankAccountProvider(x.GetService<FairWayBankClient>(), x.GetService<IMapper>()));
            services.AddSingleton<BizfiBankClient>();
            services.AddSingleton<FairWayBankClient>();

            services.AddOptions();
            services.AddMediatR();
            services.AddAutoMapper();
            services.AddMvc()
                    .AddFluentValidation(x => x.RegisterValidatorsFromAssembly(GetType().Assembly));
            services.AddApiVersioning(x =>
            {
                x.ApiVersionReader = new HeaderApiVersionReader("Api-Version");
            });

            services.Configure<BizfiBankConfiguration>(Configuration.GetSection("BizfiBank"));
            services.Configure<FairWayBankConfiguration>(Configuration.GetSection("FairWayBank"));
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