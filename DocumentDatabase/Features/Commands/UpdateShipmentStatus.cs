using FreightShipping.DocumentDatabase.Models;
using Marten;
using Wolverine.Http;

namespace FreightShipping.DocumentDatabase.Features.Commands;

public record UpdateShipmentStatusCommand(Guid ShipmentId, ShipmentStatus Status);

public record UpdateShipmentStatusResponse(Guid ShipmentId, ShipmentStatus Status);

public static class UpdateShipmentStatusHandler
{
    [WolverinePost("/api/shipments/{shipmentId}/status")]
    public static async Task<UpdateShipmentStatusResponse> Handle(
        Guid shipmentId,
        UpdateShipmentStatusCommand command,
        IDocumentSession session)
    {
        var shipment = await session.LoadAsync<FreightShipment>(shipmentId);

        if (shipment == null)
        {
            throw new InvalidOperationException($"Shipment with ID {shipmentId} not found");
        }

        shipment.Status = command.Status;

        switch (command.Status)
        {
            case ShipmentStatus.InTransit:
                shipment.PickedUpAt = DateTime.UtcNow;
                break;
            case ShipmentStatus.Delivered:
                shipment.DeliveredAt = DateTime.UtcNow;
                break;
            case ShipmentStatus.Cancelled:
                shipment.CancelledAt = DateTime.UtcNow;
                break;
        }

        session.Store(shipment);

        return new UpdateShipmentStatusResponse(shipment.Id, shipment.Status);
    }
}
