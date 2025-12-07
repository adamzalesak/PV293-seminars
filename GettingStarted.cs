using JasperFx;
using Marten;
using FreightShipping.Commands;
using FreightShipping.Handlers;

namespace FreightShipping;

public static class GettingStarted
{
    public static async Task Run()
    {
        var connectionString = Utils.GetConnectionString();
        #region store-setup
        var store = DocumentStore.For(opts =>
        {
            opts.Connection(connectionString!);
            opts.AutoCreateSchemaObjects = AutoCreate.All; // Dev mode: create tables if missing
        });
        #endregion store-setup

        #region create-shipment-with-commands
        await using var session = store.LightweightSession();  // open a new session

        // 1. Create a new shipment using command
        var createCommand = new CreateShipmentCommand("Rotterdam", "New York");
        var createResponse = await ShipmentHandlers.CreateShipment(createCommand, session);
        Console.WriteLine($"Created shipment {createResponse.ShipmentId} with status: {createResponse.Status}");

        // 2. Later... load the shipment by Id
        var loaded = await session.LoadAsync<FreightShipment>(createResponse.ShipmentId);
        Console.WriteLine($"Shipment status: {loaded!.Status}");  // Outputs: Scheduled

        // 3. Update shipment status using command
        var updateCommand = new UpdateShipmentStatusCommand(createResponse.ShipmentId, ShipmentStatus.InTransit);
        var updateResponse = await ShipmentHandlers.UpdateShipmentStatus(createResponse.ShipmentId, updateCommand, session);
        Console.WriteLine($"Updated shipment status to: {updateResponse.Status}");
        #endregion create-shipment-with-commands
    }
}

#region models
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
}
#endregion models


