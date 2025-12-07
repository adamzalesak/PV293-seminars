using FreightShippingTutorial.Models;
using Wolverine.Http;

namespace FreightShipping.Commands;

public record CreateShipmentCommand(string Origin, string Destination);

public record CreateShipmentResponse(Guid ShipmentId, ShipmentStatus Status);
