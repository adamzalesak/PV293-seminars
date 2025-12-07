using FreightShipping.EventSourcing.Aggregates.FreightShipment;
using Marten.Events.Aggregation;

namespace FreightShipping.EventSourcing.Views;

public class ShipmentView
{
    // TODO 3: Define properties for ShipmentView that we need for GetShipment query
}

public class ShipmentViewProjection : SingleStreamProjection<ShipmentView, Guid>
{
    // TODO 3: Implement projection methods to update ShipmentView based on events
    // Don't forget to register this projection in your Marten configuration (Program.cs). Select appropriate projection lifecycle.
}