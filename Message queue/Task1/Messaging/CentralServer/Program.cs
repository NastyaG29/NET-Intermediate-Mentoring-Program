using System;
using System.IO;
using Common;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CentralServer
{
    public class Program
    {
        private static readonly MessageHelper MessageHelper = new MessageHelper();

        private static string _filesStorePath;

        public static void Main(string[] args)
        {
            Console.WriteLine("Enter directory path.");
            _filesStorePath = Console.ReadLine();
            if (!Directory.Exists(_filesStorePath))
                Directory.CreateDirectory(_filesStorePath);

            var factory = new ConnectionFactory { HostName = Common.Constants.Host };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(Common.Constants.ExchangeName, ExchangeType.Fanout);

            var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queueName, Common.Constants.ExchangeName, "");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += Received;

            channel.BasicConsume(queueName, true, consumer);

            Console.ReadLine();
        }

        private static void Received(object sender, BasicDeliverEventArgs args)
        {
            var messageChunk = args.Body.ToArray().GetMessageChunk();

            if (MessageHelper.TryGetMessage(messageChunk, out var data))
            {
                var fileName = FileHelper.FromBytes(data, out var file);

                var filePath = $"{_filesStorePath}\\{fileName}";
                var index = 0;
                while (File.Exists(filePath))
                {
                    index++;
                    var newName = $"{Path.GetFileNameWithoutExtension(filePath)}_{index}";
                    var extension = Path.GetExtension(filePath);
                    filePath = $"{_filesStorePath}\\{newName}{extension}";
                }

                FileHelper.WriteToFile(filePath, file);

                Console.WriteLine($"File {fileName} was saved.");
            }
        }
    }
}