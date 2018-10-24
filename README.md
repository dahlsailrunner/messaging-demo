# messaging-demo

uses local rabbitmq install

docker pull rabbitmq

docker run -d -p 5672:5672 -p 15672:15672 --hostname rabbit-host --name rabbit-test rabbitmq:3-management

Executing Sender will push a message onto queue "wazzup"

Exectuing Receiver will successfully receive / ack "wazzup" message

Executing NServiceBus-Receiver will *try* same thing but fail reading the message

