using Examination.Domain.Events;
using Examination.Domain.SeedWord;
using MongoDB.Bson.Serialization.Attributes;

namespace Examination.Domain.AggregateModels.ExamResultAggregate;

public class ExamResult(string userId, string examId) : Entity, IAggregateRoot
{
    [BsonElement("examId")] public string ExamId { get; set; } = examId;

    [BsonElement("userId")] public string UserId { set; get; } = userId;

    [BsonElement("examQuestionReviews")] public IEnumerable<ExamResultDetail> ExamResultDetails { get; set; }

    [BsonElement("examDate")] public DateTime ExamStartDate { get; set; } = DateTime.Now;

    [BsonElement("examFinishDate")] public DateTime? ExamFinishDate { get; set; }

    [BsonElement("passed")] public bool? Passed { get; set; }

    [BsonElement("finished")] public bool Finished { get; set; } = false;

    public static ExamResult CreateNewResult(string userId, string examId)
    {
        var result = new ExamResult(userId, examId);
        return result;
    }

    public void StartExam(string firstName, string lastName)
    {
        AddDomainEvent(new ExamStartedDomainEvent(UserId, firstName, lastName));
    }

    public void SetUserChoices(List<ExamResultDetail> examResultDetails)
    {
        ExamResultDetails = examResultDetails;
    }

    public void Finish()
    {
        Finished = true;
        ExamFinishDate = DateTime.Now;
    }
}