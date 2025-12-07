using Marten;
using Wolverine.Http;
using FreightShipping.EventSourcing.Aggregates.Shipment;

namespace FreightShipping.EventSourcing.Features;

public record DeliverShipmentResponse(Guid ShipmentId, string Status, DateTime DeliveredAt);

// Handler
public static class DeliverShipmentHandler
{
    [WolverinePost("/api/event-sourced/shipments/{shipmentId}/deliver")]
    public static DeliverShipmentResponse Handle(
        Guid shipmentId,
        IDocumentSession session)
    {
        var deliveredAt = DateTime.UtcNow;

        var @event = new ShipmentDelivered(deliveredAt);

        // Append the event to the existing stream
        session.Events.Append(shipmentId, @event);

        return new DeliverShipmentResponse(shipmentId, "Delivered", deliveredAt);
    }
}
