using FreightShipping.EventSourcing.Views;
using Marten;
using Wolverine.Http;

namespace FreightShipping.EventSourcing.Features.Queries;

public record GetShipmentJourneyResponse(ShipmentJourneyDto? Journey);

public record ShipmentJourneyDto(
    Guid Id,
    string Origin,
    string Destination,
    string? CurrentLocation,
    List<WaypointDto> Route,
    double TotalDistanceKm,
    int CheckpointCount,
    bool IsInTransit,
    DateTime? JourneyStarted,
    DateTime? JourneyEnded,
    DateTime? LastUpdate
);

public record WaypointDto(
    string Location,
    double Latitude,
    double Longitude,
    DateTime Timestamp,
    string? Notes
);

public static class GetShipmentJourneyHandler
{
    [WolverineGet("/api/event-sourced/shipments/{shipmentId}/journey")]
    public static async Task<GetShipmentJourneyResponse> Handle(
        Guid shipmentId,
        IQuerySession session)
    {
        // Load the journey projection for this shipment
        var journey = await session.LoadAsync<ShipmentJourney>(shipmentId);

        if (journey == null)
        {
            return new GetShipmentJourneyResponse(null);
        }

        var dto = new ShipmentJourneyDto(
            journey.Id,
            journey.Origin,
            journey.Destination,
            journey.CurrentLocation,
            journey.Route.Select(w => new WaypointDto(
                w.Location,
                w.Latitude,
                w.Longitude,
                w.Timestamp,
                w.Notes
            )).ToList(),
            journey.TotalDistanceKm,
            journey.CheckpointCount,
            journey.IsInTransit,
            journey.JourneyStarted,
            journey.JourneyEnded,
            journey.LastUpdate
        );

        return new GetShipmentJourneyResponse(dto);
    }
}
