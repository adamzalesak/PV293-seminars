using FreightShipping.EventSourcing.Aggregates.Shipment;
using Marten;
using Wolverine.Http;

namespace FreightShipping.EventSourcing.Features;

public record PickUpShipmentCommand(Guid ShipmentId);

public record PickUpShipmentResponse(Guid ShipmentId, string Status, DateTime PickedUpAt);

public static class PickUpShipmentHandler
{
    [WolverinePost("/api/event-sourced/shipments/{shipmentId}/pickup")]
    public static async Task<PickUpShipmentResponse> Handle(
        Guid shipmentId,
        PickUpShipmentCommand command,
        IDocumentSession session)
    {
        var pickedUpAt = DateTime.UtcNow;

        var @event = new ShipmentPickedUp(pickedUpAt);

        // Append the event to the existing stream
        session.Events.Append(shipmentId, @event);

        return new PickUpShipmentResponse(shipmentId, "InTransit", pickedUpAt);
    }
}
