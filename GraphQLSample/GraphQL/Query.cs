using GraphQLSample.Models;
using HotChocolate;
using HotChocolate.Data;
using MongoDB.Driver;

namespace GraphQLSample.GraphQL
{
    public class Query
    {
        [ValidVersionFields]
        [UseFirstOrDefault]
        [UseProjection]
        public IExecutable<Person> GetPerson(string name, [Service] IMongoCollection<Person> personCollection)
            => personCollection.Find(x => x.FirstName == name).AsExecutable();
    }
}
