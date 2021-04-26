using GraphQLSample.GraphQL;
using GraphQLSample.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using System;

namespace GraphQLSample
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration) => this.configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            CarrefourDatabaseSettings dbConfigs = new();
            configuration.GetSection(nameof(CarrefourDatabaseSettings)).Bind(dbConfigs);

            services.AddScoped(sp => new MongoClient(dbConfigs.ConnectionString));

            services.AddScoped(sp => sp.GetService<MongoClient>().GetDatabase(dbConfigs.DatabaseName).GetCollection<Person>(dbConfigs.PersonsCollectionName));

            services.AddGraphQLServer()
                    .AddType<PersonType>()
                    .AddQueryType<Query>()
                    .AddProjections();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //using var scope = app.ApplicationServices.CreateScope();
            //var collection = scope.ServiceProvider.GetService<IMongoCollection<Person>>();
            //for (short i = 1; i < 50; i++)
            //{
            //    collection.InsertOne(new Person { Id = i, FirstName = $"Alberto {i}", LastName = $"Monteiro {i}", Age = i, Version = DateTime.Today });
            //}

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
            });
        }
    }
}
