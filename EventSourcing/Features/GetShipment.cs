using FreightShipping.EventSourcing.Aggregates.FreightShipment;
using Marten;
using Wolverine.Http;

namespace FreightShipping.EventSourcing.Features;

public record GetShipmentResponse(ShipmentDto? Shipment);

public record ShipmentDto(
    Guid Id,
    string Origin,
    string Destination,
    string Status,
    DateTime ScheduledAt,
    DateTime? PickedUpAt,
    DateTime? DeliveredAt,
    DateTime? CancelledAt,
    string? CancellationReason
);

public static class GetShipmentHandler
{
    [WolverineGet("/api/event-sourced/shipments/{shipmentId}")]
    public static async Task<GetShipmentResponse> Handle(
        Guid shipmentId,
        IQuerySession session)
    {
        // Aggregate the event stream to get current state
        var shipment = await session.Events.AggregateStreamAsync<FreightShipment>(shipmentId);

        if (shipment == null)
        {
            return new GetShipmentResponse(null);
        }

        var dto = new ShipmentDto(
            shipment.Id,
            shipment.Origin,
            shipment.Destination,
            shipment.Status.ToString(),
            shipment.ScheduledAt,
            shipment.PickedUpAt,
            shipment.DeliveredAt,
            shipment.CancelledAt,
            shipment.CancellationReason
        );

        return new GetShipmentResponse(dto);
    }
}
