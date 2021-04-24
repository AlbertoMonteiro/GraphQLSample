using GraphQLSample.GraphQL;
using GraphQLSample.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GraphQLSample
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration) => this.configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            var cs = configuration.GetSection("ConnectionStrings").GetValue<string>("Default");
            services.AddDbContext<AppDbContext>(c => c.UseSqlServer(cs));

            services.AddGraphQLServer()
                .AddQueryType<Query>()
                .AddType<PersonType>()
                .AddSorting()
                .AddProjections()
                .AddFiltering()
                .SetPagingOptions(new HotChocolate.Types.Pagination.PagingOptions { MaxPageSize = 10, DefaultPageSize = 10, IncludeTotalCount = true });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            using var scope = app.ApplicationServices.CreateScope();
            using var db = scope.ServiceProvider.GetService<AppDbContext>();
            _ = db.Database.EnsureDeleted();
            _ = db.Database.EnsureCreated();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
            });
        }
    }
}
