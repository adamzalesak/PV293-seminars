using Marten;
using Wolverine.Http;
using FreightShipping.EventSourcing.Aggregates.Shipment;

namespace FreightShipping.EventSourcing.Features;


public record CancelShipmentCommand(Guid ShipmentId, string Reason);

public record CancelShipmentResponse(Guid ShipmentId, string Status, string Reason, DateTime CancelledAt);

public static class CancelShipmentHandler
{
    [WolverinePost("/api/event-sourced/shipments/{shipmentId}/cancel")]
    public static async Task<CancelShipmentResponse> Handle(
        Guid shipmentId,
        CancelShipmentCommand command,
        IDocumentSession session)
    {
        var cancelledAt = DateTime.UtcNow;

        var @event = new ShipmentCancelled(command.Reason, cancelledAt);

        // Append the event to the existing stream
        session.Events.Append(shipmentId, @event);

        return new CancelShipmentResponse(shipmentId, "Cancelled", command.Reason, cancelledAt);
    }
}
