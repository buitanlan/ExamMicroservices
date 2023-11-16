using Examination.Domain.SeedWord;
using MongoDB.Bson.Serialization.Attributes;

namespace Examination.Domain.AggregateModels.ExamResultAggregate;

public class ExamResult(string userId, string examId) : Entity, IAggregateRoot
{
    [BsonElement("examId")]
    public string ExamId { get; set; } = examId;

    [BsonElement("examTitle")]
    public string ExamTitle { get; set; }


    [BsonElement("userId")]
    public string UserId { set; get; } = userId;

    [BsonElement("email")]
    public string Email { set; get; }

    [BsonElement("fullName")]
    public string FullName { set; get; }

    [BsonElement("questionResults")]
    public List<QuestionResult> QuestionResults { get; set; }

    [BsonElement("correctQuestionCount")]
    public int CorrectQuestionCount { get; set; }

    [BsonElement("examDate")]
    public DateTime ExamStartDate { get; set; } = DateTime.Now;

    [BsonElement("examFinishDate")]
    public DateTime? ExamFinishDate { get; set; }

    [BsonElement("passed")]
    public bool? Passed { get; set; }

    [BsonElement("finished")]
    public bool Finished { get; set; } = false;
}
