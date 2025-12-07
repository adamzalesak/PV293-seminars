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
        // TODO 1: Implement this handler to assign a driver to a shipment

        return new AssignDriverToShipmentResponse(Guid.NewGuid(), Guid.NewGuid(), "Driver Name");
    }
}
