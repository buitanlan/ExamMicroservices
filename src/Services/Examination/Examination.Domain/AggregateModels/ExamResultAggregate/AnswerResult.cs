using System.Security.Cryptography;
using Examination.Domain.SeedWord;
using MongoDB.Bson.Serialization.Attributes;

namespace Examination.Domain.AggregateModels.ExamResultAggregate;

public class AnswerResult : Entity
{
    public AnswerResult() {}

    public AnswerResult(string id, string content, bool? userChosen, bool isCorrect)
       =>  (Id, Content, UserChosen, IsCorrect) = (id, content, userChosen, isCorrect);

    [BsonElement("content")]
    public string Content { get; set; }

    [BsonElement("userChosen")]
    public bool? UserChosen { get; set; }

    [BsonElement("isCorrect")]
    public bool IsCorrect { get; set; }
}
