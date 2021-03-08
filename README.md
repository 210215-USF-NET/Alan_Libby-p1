# Alan_Libby-p1

## Requirements
### Functionality
- [ ] Add a new customer
- [ ] Search customers by name
- [ ] Display details of an order
- [ ] Place orders to store locations for customers
- [ ] View order history of customer
- [ ] View order history of location
- [ ] View location inventory
- [ ] The customer should be able to purchase multiple products
- [ ] Order histories should have the option to be sorted by date (latest to oldest and vice versa) or cost (least expensive to most expensive)
- [ ] The manager should be able to replenish inventory

### Database Structure
- [x] User
- [x] Location
- [x] Order
- [x] Product

Additional tables: Inventory, OrderItem

### Additional Requirements
- [ ] Exception Handling
- [ ] Input validation
- [ ] Logging (useful ones)
- [ ] At least 20 unit tests:
  - [ ] DB methods should be tested
- [ ] Data should be persisted, (no data should be hard coded)
  - [x] You should use PostgreSQL DB
  - [ ] Use code first approach to establish a connection to your DB
- [ ] WebApp should be deployed using Azure App Services
- [ ] A CI/CD pipeline should be established use Azure Pipelines
- [x] Use ASP.NET MVC for the UI
- [x] DB structure should be 3NF
  - [ ] Should have an ER Diagram
- [ ] Code should have xml documentation