using System;
using System.Collections.Generic;

namespace GraphQLSample.Models
{
    public class Person : IEntity
    {
        public long Id { get; set; }
        public DateTimeOffset Version { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public short Age { get; set; }
        public ICollection<Account> Accounts { get; set; }
    }

    public class Account
    {
        public string WebSite { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset LastSeen { get; set; }
    }
}
