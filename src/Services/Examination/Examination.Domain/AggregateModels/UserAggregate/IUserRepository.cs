using Examination.Domain.SeedWord;

namespace Examination.Domain.AggregateModels.UserAggregate;

public interface IUserRepository: IRepositoryBase<User>
{
    Task<User> GetUserByIdAsync(string externalId);
}