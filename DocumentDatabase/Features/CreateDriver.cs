using FreightShippingTutorial.Models;
using Marten;
using Wolverine.Http;

namespace FreightShipping.DocumentDatabase.Features;

public record CreateDriverCommand(string Name, string LicenseNumber);

public record CreateDriverResponse(Guid DriverId, string Name, string LicenseNumber);

public static class CreateDriverHandler
{
    [WolverinePost("/api/drivers")]
    public static CreateDriverResponse Handle(
        CreateDriverCommand command,
        IDocumentSession session)
    {
        var driver = new Driver
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            LicenseNumber = command.LicenseNumber
        };

        session.Store(driver);

        return new CreateDriverResponse(driver.Id, driver.Name, driver.LicenseNumber);
    }
}
