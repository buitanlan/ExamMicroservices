﻿using Examination.Domain.AggregateModels.ExamResultAggregate;
using Examination.Shared.SeedWork;
using MediatR;

namespace Examination.Application.Commands.V1.Exams.StartExam;

public class StartExamCommandHandler : IRequestHandler<StartExamCommand, ApiResult<bool>>
{
    private readonly IExamResultRepository _examResultRepository;
    public StartExamCommandHandler(IExamResultRepository examResultRepository)
    {
        _examResultRepository = examResultRepository;
    }
    public async Task<ApiResult<bool>> Handle(StartExam.StartExamCommand request, CancellationToken cancellationToken)
    {
        var examResult = await _examResultRepository.GetDetails(request.UserId, request.ExamId);

        if (examResult != null)
        {
            examResult.ExamStartDate = DateTime.Now;
            examResult.Finished = false;
            examResult.StartExam(request.FirstName, request.LastName);

        }
        else
        {
            examResult = ExamResult.CreateNewResult(request.UserId, request.ExamId);
            examResult.StartExam(request.FirstName, request.LastName);
            await _examResultRepository.InsertAsync(examResult);
        }
        await _examResultRepository.CommitTransactionAsync(examResult, cancellationToken);
        return new ApiSuccessResult<bool>(true);
    }
}