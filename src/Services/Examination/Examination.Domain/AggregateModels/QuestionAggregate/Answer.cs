using Examination.Domain.SeedWord;
using MongoDB.Bson.Serialization.Attributes;

namespace Examination.Domain.AggregateModels.QuestionAggregate;

public sealed class Answer : Entity
{
    public Answer(string id, string content, bool isCorrect = false) 
        => (Id, Content, IsCorrect) = (id, content, isCorrect);

    [BsonElement("content")]
    public string Content { get; set; }

    [BsonElement("isCorrect")]
    public bool IsCorrect { get; set; }

}