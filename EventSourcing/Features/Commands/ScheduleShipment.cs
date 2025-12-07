using FreightShipping.EventSourcing.Aggregates.FreightShipment;
using Marten;
using Wolverine.Http;

namespace FreightShipping.EventSourcing.Features.Commands;

public record ScheduleShipmentCommand(string Origin, string Destination);

public static class ScheduleShipmentHandler
{
    [WolverinePost("/api/event-sourced/shipments")]
    public static Guid Handle(
        ScheduleShipmentCommand command,
        IDocumentSession session)
    {
        var shipmentId = Guid.NewGuid();
        var scheduledAt = DateTime.UtcNow;

        var @event = new ShipmentScheduled(shipmentId, command.Origin, command.Destination, scheduledAt);

        // Start a new event stream for the shipment aggregate
        session.Events.StartStream<FreightShipment>(shipmentId, @event);

        return shipmentId;
    }
}