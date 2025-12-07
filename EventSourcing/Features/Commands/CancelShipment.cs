using FreightShipping.EventSourcing.Aggregates.FreightShipment;
using Marten;
using Wolverine.Http;

namespace FreightShipping.EventSourcing.Features.Commands;

public record CancelShipmentCommand(Guid ShipmentId, string Reason);

public static class CancelShipmentHandler
{
    [WolverinePost("/api/event-sourced/shipments/{shipmentId}/cancel")]
    public static void Handle(
        Guid shipmentId,
        CancelShipmentCommand command,
        IDocumentSession session)
    {
        var cancelledAt = DateTime.UtcNow;

        var @event = new ShipmentCancelled(command.Reason, cancelledAt);

        // Append the event to the existing stream
        session.Events.Append(shipmentId, @event);
    }
}
