# dotnet-ecommerce-microservices-lab

This project is a simulation of an e-commerce microservices system in real world, being inspired by Microsoft's [eShopOnContainers](https://github.com/dotnet-architecture/eShopOnContainers). It is my proof of concept about scalable Microservices architecture, Domain Driven Design, Clean Architecture and SAGA pattern.

#### *I hope to see any distribution from anyone for improvement!*

## System scope

The project has implemented the following use cases:
- Product browsing
- Shopping cart
- Customer account registration
- Checkout with online payment (with Stripe gateway)

## Overall system architecture
All services all written in .NET Core 3.1, consisting:
- Catalog & Inventory API Service (.NET Core API)
- Cart Service (.NET Core API)
- Ordering Service (.NET Core API)
- Payment Service (.NET Core API)
- Identity Service (.NET Core API)
- Ocelot API Gateway (.NET Core API)
- Pricing Service (.NET Core GRPC)
- Marketing Site (.NET Core MVC)

**RabbitMQ** is the message broker among these services, playing as a means for asynchronous communication and SAGA pattern.\
Each service has its own database which is **SQL Server** database engine.
Payment service is integrated with **Stripe payment gateway** for customer's order payment processing and management.

![alt text](https://github.com/Minh8181-blz/dotnet-ecommerce-microservices-lab/blob/master/Makta%20Ecommerce.jpeg)

## Applied technical solutions
### Event ordering
todo
### Idempotence in event processing
todo
### Domain Driven Design and Clean Architecture for complex business rules
todo
