using GraphQLSample.Models;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using System.Linq;

namespace GraphQLSample.GraphQL
{
    public class Query
    {
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Person> GetPerson([Service] AppDbContext appDbContext)
            => appDbContext.Person;
    }

    public class PersonType : ObjectType<Person>
    {
        protected override void Configure(IObjectTypeDescriptor<Person> descriptor)
            => _ = descriptor
                .Field(x => x.Version)
                .Ignore();
    }
}
