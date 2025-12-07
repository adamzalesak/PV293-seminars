using FreightShipping.DocumentDatabase.Models;
using Marten;
using Wolverine.Http;

namespace FreightShipping.DocumentDatabase.Features.Queries;

public record GetAllShipmentsQuery;

public record ShipmentDto(
    Guid Id,
    string Origin,
    string Destination,
    ShipmentStatus Status,
    DateTime ScheduledAt,
    DateTime? PickedUpAt,
    DateTime? DeliveredAt,
    DateTime? CancelledAt,
    Guid? AssignedDriverId
);

public static class GetAllShipmentsHandler
{
    [WolverineGet("/api/shipments")]
    public static async Task<IReadOnlyCollection<ShipmentDto>> Handle(IQuerySession session)
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
                s.CancelledAt,
                s.AssignedDriverId
            ))
            .ToListAsync();


        return shipmentDtos;
    }
}
