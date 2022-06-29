using AutoMapper;
using Examination.Domain.AggregateModels.CategoryAggregate;
using Examination.Domain.AggregateModels.ExamAggregate;
using Examination.Shared.Categories;
using Examination.Shared.Exams;

namespace Examination.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Exam, ExamDto>().ReverseMap();
        CreateMap<Category, CategoryDto>().ReverseMap();

    }
}