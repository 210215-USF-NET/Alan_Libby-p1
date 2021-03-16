# Alan_Libby-p1

## Requirements
### Functionality
- [x] Add a new customer
- [x] Search customers by name
- [x] Display details of an order
- [x] Place orders to store locations for customers
- [x] View order history of customer
- [x] View order history of location
- [ ] View location inventory
- [x] The customer should be able to purchase multiple products
- [x] Order histories should have the option to be sorted by date (latest to oldest and vice versa) or cost (least expensive to most expensive)
- [ ] The manager should be able to replenish inventory

### Database Structure
- [x] User
- [x] Location
- [x] Order
- [x] Product

Additional tables: Inventory, OrderItem

### Additional Requirements
- [x] Exception Handling
- [x] Input validation
- [ ] Logging (useful ones)
- [ ] At least 20 unit tests:
  - [ ] DB methods should be tested
- [x] Data should be persisted, (no data should be hard coded)
  - [x] You should use PostgreSQL DB
  - [x] Use code first approach to establish a connection to your DB
- [ ] WebApp should be deployed using Azure App Services
- [ ] A CI/CD pipeline should be established use Azure Pipelines
- [x] Use ASP.NET MVC for the UI
- [x] DB structure should be 3NF
  - [ ] Should have an ER Diagram
- [ ] Code should have xml documentation