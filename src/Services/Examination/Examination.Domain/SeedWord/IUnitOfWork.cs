namespace Examination.Domain.SeedWord;

public interface IUnitOfWork: IDisposable
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);

}