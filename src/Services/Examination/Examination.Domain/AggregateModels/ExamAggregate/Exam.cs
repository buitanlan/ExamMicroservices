﻿using Examination.Domain.AggregateModels.QuestionAggregate;
using Examination.Domain.SeedWord;
using Examination.Shared.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace Examination.Domain.AggregateModels.ExamAggregate;

public class Exam : Entity, IAggregateRoot
{
    public Exam(
        string name,
        string shortDesc,
        string content,
        int numberOfQuestions,
        int? durationInMinutes,
        List<Question> questions,
        Level level,
        string ownerUserId,
        int numberOfQuestionCorrectForPass,
        bool isTimeRestricted,
        string categoryId,
        string categoryName
    )
    {
        if (questions == null && !questions.Any())
            throw new ArgumentNullException($"{nameof(questions)} can not be null.");

        if (questions.Count != numberOfQuestions)
            throw new ArgumentNullException($"{nameof(numberOfQuestions)} is invalid.");

        if (numberOfQuestionCorrectForPass > numberOfQuestions)
            throw new ArgumentNullException($"{nameof(numberOfQuestionCorrectForPass)} is invalid.");


        (Name, ShortDesc, Content, NumberOfQuestions, DurationInMinutes, Questions, Level, DateCreated, OwnerUserId,
                NumberOfQuestionCorrectForPass, IsTimeRestricted, CategoryId, CategoryName)
            = (name, shortDesc, content, numberOfQuestions, durationInMinutes, questions, level, DateTime.UtcNow,
                ownerUserId, numberOfQuestionCorrectForPass, isTimeRestricted, categoryId, categoryName);
    }

    public Exam(string name, string shortDesc, string content, int numberOfQuestions, Level level, DateTime dateCreated,
        string ownerUserId, int numberOfQuestionCorrectForPass, bool isTimeRestricted)
    {
        Name = name;
        ShortDesc = shortDesc;
        Content = content;
        NumberOfQuestions = numberOfQuestions;
        Level = level;
        DateCreated = dateCreated;
        OwnerUserId = ownerUserId;
        NumberOfQuestionCorrectForPass = numberOfQuestionCorrectForPass;
        IsTimeRestricted = isTimeRestricted;
    }

    [BsonElement("name")] public string Name { get; set; }

    [BsonElement("shortDesc")] public string ShortDesc { get; set; }

    [BsonElement("content")] public string Content { get; set; }

    [BsonElement("numberOfQuestions")] public int NumberOfQuestions { get; set; }

    [BsonElement("durationInMinutes")] public int? DurationInMinutes { get; set; }

    [BsonElement("questions")] public List<Question> Questions { get; set; }

    [BsonElement("level")] public Level Level { get; set; }

    [BsonElement("dateCreated")] public DateTime DateCreated { get; set; }

    [BsonElement("ownerUserId")] public string OwnerUserId { get; set; }

    [BsonElement("numberOfQuestionCorrectForPass")]
    public int NumberOfQuestionCorrectForPass { get; set; }

    [BsonElement("isTimeRestricted")] public bool IsTimeRestricted { get; set; }

    [BsonElement("categoryId")] public string CategoryId { get; set; }

    [BsonElement("categoryName")] public string CategoryName { get; set; }
}
