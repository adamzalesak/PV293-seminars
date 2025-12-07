using FreightShipping.EventSourcing.Aggregates.FreightShipment;
using Marten.Events.Aggregation;

namespace FreightShipping.EventSourcing.Views;

public class ShipmentJourney
{
    public Guid Id { get; set; }

    // Route summary
    public string Origin { get; set; } = null!;
    public string Destination { get; set; } = null!;
    public string? CurrentLocation { get; set; }

    // Journey tracking
    public List<Waypoint> Route { get; set; } = new();
    public double TotalDistanceKm { get; set; }
    public int CheckpointCount { get; set; }

    // Status
    public bool IsInTransit { get; set; }
    public DateTime? JourneyStarted { get; set; }
    public DateTime? JourneyEnded { get; set; }
    public DateTime? LastUpdate { get; set; }
}

public class Waypoint
{
    public string Location { get; set; } = null!;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime Timestamp { get; set; }
    public string? Notes { get; set; }
}

public class ShipmentJourneyProjection : SingleStreamProjection<ShipmentJourney, Guid>
{
    public ShipmentJourney Create(ShipmentScheduled @event)
    {
        return new ShipmentJourney
        {
            Id = @event.ShipmentId,
            Origin = @event.Origin,
            Destination = @event.Destination,
            IsInTransit = false
        };
    }

    public void Apply(ShipmentPickedUp @event, ShipmentJourney journey)
    {
        journey.IsInTransit = true;
        journey.JourneyStarted = @event.PickedUpAt;
        journey.LastUpdate = @event.PickedUpAt;
        journey.CurrentLocation = journey.Origin;
    }

    public void Apply(LocationRecorded @event, ShipmentJourney journey)
    {
        // Add waypoint to route
        var waypoint = new Waypoint
        {
            Location = @event.LocationName,
            Latitude = @event.Latitude,
            Longitude = @event.Longitude,
            Timestamp = @event.RecordedAt,
            Notes = @event.Notes
        };

        journey.Route.Add(waypoint);
        journey.CheckpointCount++;
        journey.CurrentLocation = @event.LocationName;
        journey.LastUpdate = @event.RecordedAt;

        // Calculate total distance traveled using Haversine formula
        if (journey.Route.Count >= 2)
        {
            var previous = journey.Route[^2];
            var current = waypoint;
            journey.TotalDistanceKm += CalculateDistance(
                previous.Latitude, previous.Longitude,
                current.Latitude, current.Longitude
            );
        }
    }

    public void Apply(ShipmentDelivered @event, ShipmentJourney journey)
    {
        journey.IsInTransit = false;
        journey.JourneyEnded = @event.DeliveredAt;
        journey.CurrentLocation = journey.Destination;
        journey.LastUpdate = @event.DeliveredAt;
    }

    public void Apply(ShipmentCancelled @event, ShipmentJourney journey)
    {
        journey.IsInTransit = false;
        journey.JourneyEnded = @event.CancelledAt;
        journey.LastUpdate = @event.CancelledAt;
    }

    // Haversine formula for calculating distance between two GPS coordinates
    private static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371; // Earth's radius in kilometers

        var dLat = ToRadians(lat2 - lat1);
        var dLon = ToRadians(lon2 - lon1);

        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return R * c;
    }

    private static double ToRadians(double degrees)
    {
        return degrees * Math.PI / 180;
    }
}
