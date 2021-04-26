using GraphQLSample.Models;
using HotChocolate;
using HotChocolate.Data;
using System.Linq;

namespace GraphQLSample.GraphQL
{
    public class Query
    {
        [ValidVersionFields]
        [UseFirstOrDefault]
        [UseProjection]
        public IQueryable<Person> GetSpecificPerson(string name, [Service] AppDbContext appDbContext)
            => appDbContext.Person.Where(x => x.FirstName == name).AsQueryable();
    }
}
