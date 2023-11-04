using Examination.Domain.AggregateModels.UserAggregate;
using Examination.Domain.Events;
using MediatR;

namespace Examination.Application.DomainEventHandlers.V1;

public class SynchronizeUserWhenExamStartedDomainEventHandler(IUserRepository userRepository) : INotificationHandler<ExamStartedDomainEvent>
{
    public async Task Handle(ExamStartedDomainEvent notification, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByIdAsync(notification.UserId);
        if (user is null)
        {
            userRepository.StartTransaction();
            user = User.CreateNewUser(notification.UserId, notification.FirstName, notification.LastName);
            await userRepository.InsertAsync(user);
            await userRepository.CommitTransactionAsync(user, cancellationToken);
        }
    }
}

