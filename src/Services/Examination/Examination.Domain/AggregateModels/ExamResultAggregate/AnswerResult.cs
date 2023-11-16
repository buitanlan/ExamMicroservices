using System.Security.Cryptography;
using Examination.Domain.SeedWord;
using MongoDB.Bson.Serialization.Attributes;

namespace Examination.Domain.AggregateModels.ExamResultAggregate;

public class AnswerResult : Entity
{
    public AnswerResult() {}

    public AnswerResult(string id, string content, bool? userChosen)
       =>  (Id, Content, UserChosen) = (id, content, userChosen);

    [BsonElement("content")]
    public string Content { get; set; }

    [BsonElement("userChosen")]
    public bool? UserChosen { get; set; }
}
