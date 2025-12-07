using FreightShipping.DocumentDatabase.Models;
using Marten;
using Wolverine.Http;

namespace FreightShipping.DocumentDatabase.Features.Commands;

public record CreateShipmentCommand(string Origin, string Destination);

public record CreateShipmentResponse(Guid ShipmentId, ShipmentStatus Status);

public static class CreateShipmentHandler
{
    [WolverinePost("/api/shipments")]
    public static CreateShipmentResponse Handle(
        CreateShipmentCommand command,
        IDocumentSession session)
    {
        var shipment = new FreightShipment
        {
            Id = Guid.NewGuid(),
            Origin = command.Origin,
            Destination = command.Destination,
            Status = ShipmentStatus.Scheduled,
            ScheduledAt = DateTime.UtcNow
        };

        session.Store(shipment);

        return new CreateShipmentResponse(shipment.Id, shipment.Status);
    }
}
