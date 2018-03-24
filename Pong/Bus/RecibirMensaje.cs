using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;

namespace Pong.Bus
{
    public static class RecibirMensaje
    {
        private static IConnection connection;
        private static IModel channel;

        public static void Recibir()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            channel.QueueDeclare(
                queue: "PING_PONG", 
                durable: false,
                exclusive: false, 
                autoDelete: false, 
                arguments: null);

            channel.BasicQos(0, 1, false);
            var consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(
                queue: "PING_PONG",
                autoAck: false, 
                consumer: consumer);

            consumer.Received += (model, ea) =>
            {
                Thread.Sleep(2000);
                string response = "PONG_MESSAGE";
                 
                var body = ea.Body;
                var props = ea.BasicProperties;
                var replyProps = channel.CreateBasicProperties();
                replyProps.CorrelationId = props.CorrelationId;

                var message = Encoding.UTF8.GetString(body);

                var responseBytes = Encoding.UTF8.GetBytes(response);
                channel.BasicPublish(
                    exchange: "", 
                    routingKey: props.ReplyTo,
                    basicProperties: replyProps, 
                    body: responseBytes);
                channel.BasicAck(
                    deliveryTag: ea.DeliveryTag,
                    multiple: false);                  
            };
        }

        public static void Stop()
        {
            channel.Close();
            connection.Close();
        }

    }
}