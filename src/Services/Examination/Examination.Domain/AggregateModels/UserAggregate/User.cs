using Examination.Domain.SeedWord;
using MongoDB.Bson.Serialization.Attributes;

namespace Examination.Domain.AggregateModels.UserAggregate;

public class User(string externalId, string firstName, string lastName) : Entity, IAggregateRoot
{
    [BsonElement("externalId")]
    public string ExternalId { get; set; } = externalId;

    [BsonElement("firstName")]
    public string FirstName { get; set; } = firstName;

    [BsonElement("lastName")]

    public string LastName { get; set; } = lastName;

    public static User CreateNewUser(string externalId, string firstName, string lastName)
    {
        return new User(externalId, firstName, lastName);
    }
}