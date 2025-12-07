using FreightShipping.EventSourcing.Aggregates.FreightShipment;
using Marten;
using Wolverine.Http;

namespace FreightShipping.EventSourcing.Features.Commands;

public static class DeliverShipmentHandler
{
    [WolverinePost("/api/event-sourced/shipments/{shipmentId}/deliver")]
    public static void Handle(
        Guid shipmentId,
        IDocumentSession session)
    {
        // TODO 1: Implement this handler to deliver a shipment
    }
}
