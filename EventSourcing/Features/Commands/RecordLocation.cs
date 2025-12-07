using FreightShipping.EventSourcing.Aggregates.FreightShipment;
using Marten;
using Wolverine.Http;

namespace FreightShipping.EventSourcing.Features.Commands;

public record RecordLocationCommand(
    string LocationName,
    double Latitude,
    double Longitude,
    string? Notes = null
);

public static class RecordLocationHandler
{
    [WolverinePost("/api/event-sourced/shipments/{shipmentId}/location")]
    public static async Task<IResult> Handle(
        Guid shipmentId,
        RecordLocationCommand command,
        IDocumentSession session,
        CancellationToken cancellationToken)
    {
        // Load the current aggregate state to validate business rules
        var shipment = await session.Events.AggregateStreamAsync<FreightShipment>(shipmentId, token: cancellationToken);
        if (shipment == null)
        {
            return Results.NotFound($"Shipment {shipmentId} not found");
        }

        // Business rule: Location can only be recorded for shipments in transit
        if (shipment.Status != ShipmentStatus.InTransit)
        {
            return Results.BadRequest(
                $"Location can only be recorded for shipments in transit. Current status: {shipment.Status}");
        }

        var @event = new LocationRecorded(
            command.LocationName,
            command.Latitude,
            command.Longitude,
            DateTime.UtcNow,
            command.Notes
        );

        session.Events.Append(shipmentId, @event);

        return Results.Ok();
    }
}
