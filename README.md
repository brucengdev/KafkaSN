# KafkaSN - Kafka based social network
A repository used to learn to use Kafka and Event-Driven-Architecture.  

## Running the demo

```
docker compose up -d
```

Check folder `./src/data` for data that are written by services.  

## Services

The WebAPI service provides a REST API that can be called from clients like Postman.  
The WebAPI then sends events to Kafka topics.  

Services consume and send new events through Kafka.  

For simplicity, this demo writes data to a docker bind mount instead of some database system.  

All inter-service communications are through Kafka, there is no direct communication between services, thus services are completely decoupled from each other.  

New services can be added easily to listen to existing topics to add new functionality to the system.  

```
            WebAPI
              |
--------------------------------------
|                KAFKA               |
--------------------------------------
  |          |                  |
Post      Account        EmailNotification
```

## Some data flows  

### Creating new accounts  

1. Client sends request to WebAPI service /accounts endpoint.  
2. WebAPI sends an event to topic `createAccount` of Kafka.  
3. Account service consumes the event and writes the account to storage.  
4. Account service sends event to topic `accountCreated`.  
5. EmailNotification service consumes the event from `accountCreated` and send email to notify account creation.  

### User creates new post  

1. Client sends request to WebAPI service /posts endpoint.  
2. WebAPI sends an event to topic `createPost` of Kafka.
3. Post service consumes the event and writes the post to storage.
4. Post service publishes an event to topic `postCreated`.
5. EmailNotification service consumes the event from `postCreated` and send email to notify of new post.