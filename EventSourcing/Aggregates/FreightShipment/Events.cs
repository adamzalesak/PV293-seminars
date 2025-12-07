namespace FreightShipping.EventSourcing.Aggregates.FreightShipment;

public record ShipmentScheduled(Guid ShipmentId, string Origin, string Destination, DateTime ScheduledAt);
public record ShipmentPickedUp(DateTime PickedUpAt);
public record ShipmentDelivered(DateTime DeliveredAt);
public record ShipmentCancelled(string Reason, DateTime CancelledAt);
public record LocationRecorded(
    string LocationName,
    double Latitude,
    double Longitude,
    DateTime RecordedAt,
    string? Notes = null
);