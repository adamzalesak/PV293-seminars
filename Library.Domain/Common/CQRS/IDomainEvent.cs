using MediatR;

namespace Library.Domain.Common.CQRS;

public interface IDomainEvent : INotification;

public interface IDomainEventHandler<in TDomainEvent> : INotificationHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent;