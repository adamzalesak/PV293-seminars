namespace FreightShipping.EventSourcing.Aggregates.Shipment;

public record ShipmentScheduled(Guid ShipmentId, string Origin, string Destination, DateTime ScheduledAt);
public record ShipmentPickedUp(DateTime PickedUpAt);
public record ShipmentDelivered(DateTime DeliveredAt);
public record ShipmentCancelled(string Reason, DateTime CancelledAt);