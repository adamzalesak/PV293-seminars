using FreightShipping.EventSourcing.Aggregates.FreightShipment;
using Marten;
using Wolverine.Http;

namespace FreightShipping.EventSourcing.Features.Commands;

public static class DeliverShipmentHandler
{
    [WolverinePost("/api/event-sourced/shipments/{shipmentId}/deliver")]
    public static void Handle(
        Guid shipmentId,
        IDocumentSession session)
    {
        var deliveredAt = DateTime.UtcNow;

        var @event = new ShipmentDelivered(deliveredAt);

        // Append the event to the existing stream
        session.Events.Append(shipmentId, @event);
    }
}
