using FreightShipping.EventSourcing.Views;
using FreightShippingTutorial.Models;
using JasperFx;
using JasperFx.Events.Projections;
using Marten;

// ReSharper disable once CheckNamespace
namespace FreightShipping.EventSourcedAggregate;

public static class EventSourcedAggregate
{
    public static async Task Run()
    {
        var connectionString = Utils.GetConnectionString();
        #region store-setup
        var store = DocumentStore.For(opts =>
        {
            opts.Connection(connectionString!);
            opts.AutoCreateSchemaObjects = AutoCreate.All; // Dev mode: create tables if missing
            opts.Projections.Add<ShipmentViewProjection>(ProjectionLifecycle.Inline);
        });
        #endregion store-setup
        
        #region storing-events

        await using var session = store.LightweightSession();

        // 1. Start a new event stream for a shipment
        var shipmentId = Guid.NewGuid();
        var scheduleEvent = new ShipmentScheduled(shipmentId, "Rotterdam", "New York", DateTime.UtcNow);
        session.Events.StartStream<FreightShipment>(shipmentId, scheduleEvent);
        await session.SaveChangesAsync();
        Console.WriteLine($"Started stream {shipmentId} with ShipmentScheduled.");

        // 2. Append a ShipmentPickedUp event (in a real scenario, later in time)
        var pickupEvent = new ShipmentPickedUp(DateTime.UtcNow.AddHours(5));
        session.Events.Append(shipmentId, pickupEvent);

        // 3. Append a ShipmentDelivered event
        var deliveredEvent = new ShipmentDelivered(DateTime.UtcNow.AddDays(1));
        session.Events.Append(shipmentId, deliveredEvent);

        // 4. Commit the new events
        await session.SaveChangesAsync();
        Console.WriteLine($"Appended PickedUp and Delivered events to stream {shipmentId}.");
        #endregion storing-events
        
        #region live-aggregate
        // Assuming we have a stream of events for shipmentId (from earlier Part)
        var currentState = await session.Events.AggregateStreamAsync<FreightShipment>(shipmentId);
        Console.WriteLine($"State: {currentState!.Status}, PickedUpAt: {currentState.PickedUpAt}");
        #endregion live-aggregate
        
        #region shipment-example
        await using var session2 = store.LightweightSession();

        var sid = Guid.NewGuid();
        var evt1 = new ShipmentScheduled(sid, "Los Angeles", "Tokyo", DateTime.UtcNow);
        session2.Events.StartStream<ShipmentView>(sid, evt1);
        await session.SaveChangesAsync();  // Inserts initial ShipmentView

        var evt2 = new ShipmentPickedUp(DateTime.UtcNow.AddHours(2));
        session2.Events.Append(sid, evt2);
        await session2.SaveChangesAsync();  // Updates ShipmentView.Status and PickedUpAt

        var doc = await session2.LoadAsync<ShipmentView>(sid);
        Console.WriteLine(doc!.Status);         // InTransit
        Console.WriteLine(doc.PickedUpAt);    // Set to pickup time
        #endregion shipment-example
    }
}

