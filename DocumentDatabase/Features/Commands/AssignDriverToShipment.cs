using FreightShipping.DocumentDatabase.Models;
using Marten;
using Wolverine.Http;

namespace FreightShipping.DocumentDatabase.Features.Commands;

public record AssignDriverToShipmentCommand(Guid ShipmentId, Guid DriverId);

public record AssignDriverToShipmentResponse(Guid ShipmentId, Guid DriverId, string DriverName);

public static class AssignDriverToShipmentHandler
{
    [WolverinePost("/api/shipments/{shipmentId}/assign-driver")]
    public static async Task<AssignDriverToShipmentResponse> Handle(
        Guid shipmentId,
        AssignDriverToShipmentCommand command,
        IDocumentSession session)
    {
        var shipment = await session.LoadAsync<FreightShipment>(shipmentId);

        if (shipment == null)
        {
            throw new InvalidOperationException($"Shipment with ID {shipmentId} not found");
        }

        var driver = await session.LoadAsync<Driver>(command.DriverId);

        if (driver == null)
        {
            throw new InvalidOperationException($"Driver with ID {command.DriverId} not found");
        }

        shipment.AssignedDriverId = driver.Id;
        session.Store(shipment);

        return new AssignDriverToShipmentResponse(shipment.Id, driver.Id, driver.Name);
    }
}
