Eaze Coding Exercise

Web.API with NserviceBus (via RabbitMQ) and MongoDB.

Project Requirements:
1. Visual Studio 2015 or later.
2. MongoDB Server: https://www.mongodb.com/download-center#community
3. Erlang: https://www.erlang.org/downloads
4. RabbitMQ: https://www.rabbitmq.com/download.html

Project Description:
WebAPI receives the request, stores the request info via MongoDB, 
and communicates the request to EazeCodingExercise.Endpoint via NServiceBus
(Non blocking communication).

EazeCodingExercise.Endpoint takes the request, processes the request, and updates 
the entry in mongoDB.

Improvements:
Implement SignalR (https://www.asp.net/signalr) or some sort of push notificaiton 
to the client.

Integration tests.

