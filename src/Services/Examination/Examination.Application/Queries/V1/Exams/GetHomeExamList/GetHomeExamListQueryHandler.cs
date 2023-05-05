using AutoMapper;
using Examination.Domain.AggregateModels.ExamAggregate;
using Examination.Shared.Exams;
using MediatR;
using MongoDB.Driver;

namespace Examination.Application.Queries.V1.Exams.GetHomeExamList;

public class GetHomeExamListQueryHandler(IExamRepository examRepository,
        IMapper mapper,
        IClientSessionHandle clientSessionHandle)
    : IRequestHandler<GetHomeExamListQuery, IEnumerable<ExamDto>>
{
    private readonly IExamRepository _examRepository = examRepository ?? throw new ArgumentNullException(nameof(examRepository));
    private readonly IClientSessionHandle _clientSessionHandle = clientSessionHandle ?? throw new ArgumentNullException(nameof(_clientSessionHandle));

    public async Task<IEnumerable<ExamDto>> Handle(GetHomeExamListQuery request, CancellationToken cancellationToken)
    {
        var exams = await _examRepository.GetExamListAsync();
        var examDtos = mapper.Map<IEnumerable<ExamDto>>(exams);
        return examDtos;
    }
}
