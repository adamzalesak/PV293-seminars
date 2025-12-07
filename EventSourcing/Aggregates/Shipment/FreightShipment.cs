namespace FreightShipping.EventSourcing.Aggregates.Shipment;

public enum ShipmentStatus { Scheduled, InTransit, Delivered, Cancelled }

public class FreightShipment
{
    public Guid Id { get; private set; }
    public string Origin { get; private set; } = null!;
    public string Destination { get; private set; } = null!;
    public ShipmentStatus Status { get; private set; }
    public DateTime ScheduledAt { get; private set; }
    public DateTime? PickedUpAt { get; private set; }
    public DateTime? DeliveredAt { get; private set; }
    public DateTime? CancelledAt { get; private set; }
    public string? CancellationReason { get; private set; }

    public static FreightShipment Create(ShipmentScheduled @event)
    {
        return new FreightShipment
        {
            Id = @event.ShipmentId,
            Origin = @event.Origin,
            Destination = @event.Destination,
            Status = ShipmentStatus.Scheduled,
            ScheduledAt = @event.ScheduledAt
        };
    }

    public static FreightShipment Apply(FreightShipment current, ShipmentPickedUp @event)
    {
        current.Status = ShipmentStatus.InTransit;
        current.PickedUpAt = @event.PickedUpAt;
        return current;
    }

    public static FreightShipment Apply(FreightShipment current, ShipmentDelivered @event)
    {
        current.Status = ShipmentStatus.Delivered;
        current.DeliveredAt = @event.DeliveredAt;
        return current;
    }

    public static FreightShipment Apply(FreightShipment current, ShipmentCancelled @event)
    {
        current.Status = ShipmentStatus.Cancelled;
        current.CancelledAt = @event.CancelledAt;
        current.CancellationReason = @event.Reason;
        return current;
    }
}