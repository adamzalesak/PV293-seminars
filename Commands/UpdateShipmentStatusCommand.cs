using FreightShippingTutorial.Models;

namespace FreightShipping.Commands;

public record UpdateShipmentStatusCommand(Guid ShipmentId, ShipmentStatus Status);

public record UpdateShipmentStatusResponse(Guid ShipmentId, ShipmentStatus Status);
