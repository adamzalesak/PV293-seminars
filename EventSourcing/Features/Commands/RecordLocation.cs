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
        // BTW, why not use AggregateStreamAsync here and session.Events.Append below?
        var shipment = await session.Events.FetchForWriting<FreightShipment>(shipmentId, cancellation: cancellationToken);
        if (shipment.Aggregate == null)
        {
            return Results.NotFound($"Shipment {shipmentId} not found");
        }

        // Business rule: Location can only be recorded for shipments in transit
        if (shipment.Aggregate.Status != ShipmentStatus.InTransit)
        {
            return Results.BadRequest(
                $"Location can only be recorded for shipments in transit. Current status: {shipment.Aggregate.Status}");
        }

        var @event = new LocationRecorded(
            command.LocationName,
            command.Latitude,
            command.Longitude,
            DateTime.UtcNow,
            command.Notes
        );

        shipment.AppendOne(@event);

        return Results.Ok();
    }
}
