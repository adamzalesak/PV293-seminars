using FreightShippingTutorial.Models;
using Marten;
using Wolverine.Http;

namespace FreightShipping.DocumentDatabase.Features;

public record GetShipmentsWithDriverDetailsQuery;

public record ShipmentWithDriverDto(
    Guid ShipmentId,
    string Origin,
    string Destination,
    ShipmentStatus Status,
    DateTime ScheduledAt,
    DateTime? PickedUpAt,
    DateTime? DeliveredAt,
    Guid? DriverId,
    string? DriverName,
    string? DriverLicenseNumber
);

public static class GetShipmentsWithDriverDetailsHandler
{
    [WolverineGet("/api/shipments/with-driver-details")]
    public static async Task<IReadOnlyCollection<ShipmentWithDriverDto>> Handle(IQuerySession session)
    {
        // Using Include to join shipments with drivers in a single SQL query
        var drivers = new Dictionary<Guid, Driver>();

        var shipments = await session.Query<FreightShipment>()
            .Where(s => s.AssignedDriverId != null)
            .Include(drivers).On(s => s.AssignedDriverId!.Value)
            .ToListAsync();

        var result = shipments
            .Select(s => new ShipmentWithDriverDto(
                s.Id,
                s.Origin,
                s.Destination,
                s.Status,
                s.ScheduledAt,
                s.PickedUpAt,
                s.DeliveredAt,
                s.AssignedDriverId,
                drivers.TryGetValue(s.AssignedDriverId!.Value, out var driver) ? driver.Name : null,
                drivers.TryGetValue(s.AssignedDriverId!.Value, out var driverLicense) ? driverLicense.LicenseNumber : null
            ))
            .ToList();

        return result;
    }
}
