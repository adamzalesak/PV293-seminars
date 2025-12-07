using Marten;
using Wolverine.Http;
using FreightShipping.Commands;
using FreightShipping.Queries;
using FreightShippingTutorial.Models;

namespace FreightShipping.Handlers;

public static class ShipmentHandlers
{
    [WolverineGet("/api/shipments")]
    public static async Task<GetAllShipmentsResponse> GetAllShipments(IQuerySession session)
    {
        var shipmentDtos = await session
            .Query<FreightShipment>()
            .Select(s => new ShipmentDto(
                s.Id,
                s.Origin,
                s.Destination,
                s.Status,
                s.ScheduledAt,
                s.PickedUpAt,
                s.DeliveredAt,
                s.CancelledAt
            ))
            .ToListAsync();


        return new GetAllShipmentsResponse(shipmentDtos);
    }

    [WolverinePost("/api/shipments")]
    public static CreateShipmentResponse CreateShipment(
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

    [WolverinePost("/api/shipments/{shipmentId}/status")]
    public static async Task<UpdateShipmentStatusResponse> UpdateShipmentStatus(
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