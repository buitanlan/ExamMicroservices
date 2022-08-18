﻿using AutoMapper;
using Examination.Domain.AggregateModels.QuestionAggregate;
using Examination.Shared.Questions;
using MediatR;
using MongoDB.Bson;
using Serilog;

namespace Examination.Application.Commands.V1.Questions.CreateQuestion;

public class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand, QuestionDto>
{
    private readonly IQuestionRepository _questionRepository;
    private readonly IMapper _mapper;

    public CreateQuestionCommandHandler(
        IQuestionRepository questionRepository,
        IMapper mapper
    )
    {
        _questionRepository = questionRepository;
        _mapper = mapper;

    }

    public async Task<QuestionDto> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
    {
        var itemToAdd = await _questionRepository.GetQuestionsByIdAsync(request.Content);
        if (itemToAdd != null)
        {
            Log.Fatal($"Item name existed: {request.Content}");
            return null;

        }
        var questionId = ObjectId.GenerateNewId().ToString();
        var answers = _mapper.Map<List<AnswerDto>, List<Answer>>(request.Answers);
        itemToAdd = new Question(questionId,
            request.Content,
            request.QuestionType,
            request.Level,
            request.CategoryId,
            answers,
            request.Explain, null);
        try
        {
            await _questionRepository.InsertAsync(itemToAdd);
            return _mapper.Map<Question, QuestionDto>(itemToAdd);
        }
        catch (Exception ex)
        {
            Log.Fatal(ex.Message);
            throw;
        }
    }
}