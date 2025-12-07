using FreightShippingTutorial.Models;
using Marten;
using Wolverine.Http;

namespace FreightShipping.DocumentDatabase.Features;

public record GetShipmentsByDestinationResponse(
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

public static class GetShipmentsByDestinationHandler
{
    [WolverineGet("/api/shipments/by-destination/{destination}")]
    public static async Task<IReadOnlyCollection<GetShipmentsByDestinationResponse>> Handle(
        string destination,
        IQuerySession session)
    {
        var shipments = await session
            .Query<FreightShipment>()
            .Where(x => x.Destination == destination)
            .Select(s => new GetShipmentsByDestinationResponse(
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

        return shipments;
    }
}