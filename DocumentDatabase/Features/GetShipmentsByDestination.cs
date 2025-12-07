using FreightShippingTutorial.Models;
using Marten;
using Wolverine.Http;

namespace FreightShipping.DocumentDatabase.Features;

public record GetShipmentsByDestinationQuery(string Destination);

public static class GetShipmentsByDestinationHandler
{
    [WolverineGet("/api/shipments/by-destination/{destination}")]
    public static async Task<IReadOnlyCollection<ShipmentDto>> Handle(
        string destination,
        IQuerySession session)
    {
        var shipments = await session
            .Query<FreightShipment>()
            .Where(x => x.Destination == destination)
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

        return shipments;
    }
}
