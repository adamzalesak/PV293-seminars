using FreightShippingTutorial.Models;

namespace FreightShipping.Queries;

public record GetAllShipmentsQuery;

public record GetAllShipmentsResponse(IReadOnlyCollection<ShipmentDto> Shipments);

public record ShipmentDto(
    Guid Id,
    string Origin,
    string Destination,
    ShipmentStatus Status,
    DateTime ScheduledAt,
    DateTime? PickedUpAt,
    DateTime? DeliveredAt,
    DateTime? CancelledAt
);
