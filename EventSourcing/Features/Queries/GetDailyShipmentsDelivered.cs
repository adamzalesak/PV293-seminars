using FreightShipping.EventSourcing.Views;
using Marten;
using Wolverine.Http;

namespace FreightShipping.EventSourcing.Features.Queries;

public record GetDailyShipmentsDeliveredResponse(DailyShipmentsDto? DailyShipments);

public record GetAllDailyShipmentsDeliveredResponse(IReadOnlyCollection<DailyShipmentsDto> DailyShipments);

public record DailyShipmentsDto(
    string Id,
    DateOnly DeliveredDate,
    int DeliveredCount
);

public static class GetDailyShipmentsDeliveredHandler
{
    [WolverineGet("/api/event-sourced/daily-shipments/{date}")]
    public static async Task<GetDailyShipmentsDeliveredResponse> Handle(
        string date,
        IQuerySession session)
    {
        var dailyStats = await session.LoadAsync<DailyShipmentsDeliveredView>(date);

        if (dailyStats == null)
        {
            return new GetDailyShipmentsDeliveredResponse(null);
        }

        var dto = new DailyShipmentsDto(
            dailyStats.Id,
            dailyStats.DeliveredDate,
            dailyStats.DeliveredCount
        );

        return new GetDailyShipmentsDeliveredResponse(dto);
    }
}

// Handler for getting all daily statistics
public static class GetAllDailyShipmentsDeliveredHandler
{
    [WolverineGet("/api/event-sourced/daily-shipments")]
    public static async Task<GetAllDailyShipmentsDeliveredResponse> Handle(
        IQuerySession session)
    {
        var dailyStats = await session
            .Query<DailyShipmentsDeliveredView>()
            .Select(d => new DailyShipmentsDto(
                d.Id,
                d.DeliveredDate,
                d.DeliveredCount
            ))
            .ToListAsync();

        return new GetAllDailyShipmentsDeliveredResponse(dailyStats);
    }
}
