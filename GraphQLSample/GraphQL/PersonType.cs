using GraphQLSample.Models;
using HotChocolate.Types;

namespace GraphQLSample.GraphQL
{
    public class PersonType : ObjectType<Person>
    {
        protected override void Configure(IObjectTypeDescriptor<Person> descriptor)
            => descriptor
                .Field(x => x.Version)
                .IsProjected(true)
                .Ignore(true);
    }
}
