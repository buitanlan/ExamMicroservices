using MediatR;

namespace Examination.Domain.Events;

public class ExamStartedDomainEvent(string userId, string firstName, string lastName) : INotification
{
    public string UserId { set; get; } = userId;
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
}
