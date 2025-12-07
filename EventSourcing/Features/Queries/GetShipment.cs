using FreightShipping.EventSourcing.Aggregates.FreightShipment;
using Marten;
using Wolverine.Http;

namespace FreightShipping.EventSourcing.Features.Queries;

public record GetShipmentResponse(ShipmentDto? Shipment);

public record ShipmentDto(
    Guid Id,
    string Origin,
    string Destination,
    string Status
);

public static class GetShipmentHandler
{
    [WolverineGet("/api/event-sourced/shipments/{shipmentId}")]
    public static async Task<GetShipmentResponse> Handle(
        Guid shipmentId,
        IQuerySession session)
    {
        // TODO 2: Currently loading all events to get the current status.
        // Create a SingleStreamProjection and load from a view model here (that is updated via events). 
        
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
            shipment.Status.ToString()
        );

        return new GetShipmentResponse(dto);
    }
}