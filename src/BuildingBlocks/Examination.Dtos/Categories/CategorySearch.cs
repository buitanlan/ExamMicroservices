using Examination.Dtos.SeekWork;

namespace Examination.Dtos.Categories;

public class CategorySearch : PagingParameters
{
    public string Name { get; set; }
}