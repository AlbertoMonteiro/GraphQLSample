using System;

namespace GraphQLSample.Models
{
    public interface IEntity
    {
        DateTimeOffset Version { get; set; }
    }
}