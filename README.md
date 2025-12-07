# Freight Shipping Tutorial - Seminar Assignment

This project demonstrates two approaches to data persistence: **Document Database** and **Event Sourcing** using Marten DB.

## Overview

You'll implement three features to practice both approaches:
- Document database pattern for direct state manipulation
- Event sourcing for tracking state changes through events
- Event projections for optimized queries

## Assignment Tasks

### TODO 1: Document Database - Assign Driver to Shipment

**File**: [DocumentDatabase/Features/Commands/AssignDriverToShipment.cs:19](DocumentDatabase/Features/Commands/AssignDriverToShipment.cs#L19)

Implement the handler to assign a driver to a shipment using the document database approach.

**Requirements**:
- Load the `FreightShipment` document by `shipmentId`
- Load the `Driver` document by `command.DriverId`
- Update the shipment's `AssignedDriverId` property
- Save the updated shipment document
- Return the correct response with shipment ID, driver ID, and driver name

**Hint**: Use `IDocumentSession.LoadAsync<T>()` and `IDocumentSession.SaveChangesAsync()`

---

### TODO 2: Event Sourcing - Deliver Shipment

**File**: [EventSourcing/Features/Commands/DeliverShipment.cs:14](EventSourcing/Features/Commands/DeliverShipment.cs#L14)

Implement the handler to deliver a shipment using event sourcing.

**Requirements**:
- Load the shipment aggregate from the event stream
- Append a `ShipmentDelivered` event with the current timestamp
- Save changes to persist the new event

**Hint**: Use `IDocumentSession.Events.FetchForWriting<T>()` to load the aggregate and append events

---

### TODO 3: Event Projections - ShipmentView

**Files**:
- [EventSourcing/Views/ShipmentView.cs:8](EventSourcing/Views/ShipmentView.cs#L8)
- [EventSourcing/Views/ShipmentView.cs:13](EventSourcing/Views/ShipmentView.cs#L13)
- [EventSourcing/Features/Queries/GetShipment.cs:23](EventSourcing/Features/Queries/GetShipment.cs#L23)

Create a projection to optimize the `GetShipment` query instead of aggregating all events on every read.

**Requirements**:
1. Define `ShipmentView` properties (Id, Origin, Destination, Status)
2. Implement `ShipmentViewProjection` methods to handle events:
   - `Create()` for `ShipmentScheduled`
   - `Apply()` for `ShipmentPickedUp`
   - `Apply()` for `ShipmentDelivered`
   - `Apply()` for `ShipmentCancelled`
3. Register the projection in `Program.cs` with appropriate lifecycle
4. Update `GetShipmentHandler` to load from the view instead of aggregating events

**Hint**: Use `SingleStreamProjection<TView, TId>` and consider using `ProjectionLifecycle.Inline` for real-time updates

---

## Project Structure

```
DocumentDatabase/         - Document database approach
  Models/                 - Domain models
  Features/Commands/      - Command handlers

EventSourcing/           - Event sourcing approach
  Aggregates/            - Event-sourced aggregates
  Events/                - Event definitions
  Features/              - Command and query handlers
  Views/                 - Projections for read models
```
