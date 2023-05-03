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
Each service has its own database which is **SQL Server** database engine.\
Payment service is integrated with **Stripe payment gateway** for customer's order payment processing and management.

![alt text](https://github.com/Minh8181-blz/dotnet-ecommerce-microservices-lab/blob/master/Makta%20Ecommerce.jpeg)

## Applied technical solutions
### Event ordering
In microservices system, when an operation occurs on a service, an event is generated and pushed to message broker. Other services interested in those events will subscribe for them and receive messages upon occurences.\
In most systems, the order which events are generated and processed is vital for maintaining the sanity of business.

By implementing SAGA pattern, events are ensured to be generated in order because they comply to causal ordering law.\
Turning to event processing, I desired the same thing which is achieved with the help of RabbitMQ. RabbitMQ allows subscriber to consume messages in order if there is only 1 active subscriber, the ordering is ensured.\
However, scalability is the pain point of this solution because 2 competing consumers cannot maintain this order. Kafka is a better candidate to solve both ordering and scaling problem with much less effort.

### Domain Driven Design and Clean Architecture for complex business rules
These 2 terms are already very popular, I won't dig deeply down on them. I always keep the following rules in mind when I implement DDD and Clean Architecture:
- Domain layer does not depend on use-case, infrastructure or framework so it should depend as little as possible on library (general utils library can be an exception)
- Application represents the use-case, it coordinates the actions of domain layer. It does not depend on infrastructure either. It can delegate some operations to infrastructure layer
- Infrastructure is where your most of libraries should be installed and configured (e.g: Entity Framework, Redis lib, Stripe SDK...)
