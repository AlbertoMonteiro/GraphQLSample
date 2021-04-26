using System;

namespace GraphQLSample.Models
{
    public class Person : IEntity
    {
        public long Id { get; set; }
        public DateTimeOffset Version { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public short Age { get; set; }
    }
}
