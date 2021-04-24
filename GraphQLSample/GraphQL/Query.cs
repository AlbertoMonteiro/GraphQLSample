using GraphQLSample.Models;
using HotChocolate;
using System.Linq;

namespace GraphQLSample.GraphQL
{
    public class Query
    {
        public IQueryable<Person> GetPerson([Service] AppDbContext appDbContext)
            => appDbContext.Person;
    }
}
