namespace FreightShipping.DocumentDatabase.Models;

public enum ShipmentStatus { Scheduled, InTransit, Delivered, Cancelled }

public class FreightShipment
{
    public Guid Id { get; set; }
    public string Origin { get; set; } = null!;
    public string Destination { get; set; } = null!;
    public ShipmentStatus Status { get; set; }
    public DateTime ScheduledAt { get; set; }
    public DateTime? PickedUpAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public DateTime? CancelledAt { get; set; }
    public Guid? AssignedDriverId { get; set; }
}