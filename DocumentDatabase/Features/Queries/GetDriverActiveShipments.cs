using FreightShipping.DocumentDatabase.Models;
using Marten;
using Wolverine.Http;

namespace FreightShipping.DocumentDatabase.Features.Queries;

public record GetDriverActiveShipmentsQuery(Guid DriverId);

public record DriverActiveShipmentsResponse(Guid DriverId, string DriverName, int ActiveShipmentsCount);

public static class GetDriverActiveShipmentsHandler
{
    [WolverineGet("/api/drivers/{driverId}/active-shipments")]
    public static async Task<DriverActiveShipmentsResponse> Handle(
        Guid driverId,
        IQuerySession session)
    {
        var driver = await session.LoadAsync<Driver>(driverId);

        if (driver == null)
        {
            throw new InvalidOperationException($"Driver with ID {driverId} not found");
        }

        var activeCount = await session
            .Query<FreightShipment>()
            .CountAsync(x => x.AssignedDriverId == driverId && x.Status != ShipmentStatus.Delivered);

        return new DriverActiveShipmentsResponse(driver.Id, driver.Name, activeCount);
    }
}
