using FreightShipping.EventSourcing.Aggregates.FreightShipment;
using Marten;
using Wolverine.Http;

namespace FreightShipping.EventSourcing.Features;

public record PickUpShipmentResponse(Guid ShipmentId, string Status, DateTime PickedUpAt);

public static class PickUpShipmentHandler
{
    [WolverinePost("/api/event-sourced/shipments/{shipmentId}/pickup")]
    public static PickUpShipmentResponse Handle(
        Guid shipmentId,
        IDocumentSession session)
    {
        var pickedUpAt = DateTime.UtcNow;

        var @event = new ShipmentPickedUp(pickedUpAt);

        // Append the event to the existing stream
        session.Events.Append(shipmentId, @event);

        return new PickUpShipmentResponse(shipmentId, "InTransit", pickedUpAt);
    }
}
