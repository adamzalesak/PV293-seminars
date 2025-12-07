using FreightShipping.EventSourcing.Aggregates.FreightShipment;
using Marten;
using Wolverine.Http;

namespace FreightShipping.EventSourcing.Features.Commands;

public static class PickUpShipmentHandler
{
    [WolverinePost("/api/event-sourced/shipments/{shipmentId}/pickup")]
    public static void Handle(
        Guid shipmentId,
        IDocumentSession session)
    {
        var pickedUpAt = DateTime.UtcNow;

        var @event = new ShipmentPickedUp(pickedUpAt);

        // Append the event to the existing stream
        session.Events.Append(shipmentId, @event);
    }
}
