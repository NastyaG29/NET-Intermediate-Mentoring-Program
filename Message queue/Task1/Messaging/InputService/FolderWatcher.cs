using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Common;
using RabbitMQ.Client;

namespace InputService
{
    public class FolderWatcher : IDisposable
    {
        private readonly IModel _channel;

        private readonly FileSystemWatcher _fileSystemWatcher;

        private readonly List<string> _allowedExtensions;

        private const int ThreadTimeout = 3 * 1000;

        private const int MaxFileReadAttempts = 10;

        public FolderWatcher(IModel channel, string path, List<string> allowedExtensions)
        {
            _channel = channel;

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            _fileSystemWatcher = new FileSystemWatcher(path)
            {
                EnableRaisingEvents = true
            };
            _fileSystemWatcher.Created += OnCreated;

            _allowedExtensions = allowedExtensions;
        }

        public void Dispose()
        {
            _fileSystemWatcher.Dispose();
        }

        private void OnCreated(object source, FileSystemEventArgs e)
        {
            var fileExtension = Path.GetExtension(e.FullPath);
            if (!_allowedExtensions.Contains(fileExtension))
                return;

            Console.WriteLine($"File {e.Name} was created.");

            Task.Run(() =>
            {
                var currentRetry = 0;
                while (currentRetry < MaxFileReadAttempts)
                {
                    try
                    {
                        var data = FileHelper.ToBytes(e.FullPath, e.Name);

                        var messageChunks = MessageHelper.GetMessageChunks(data);
                        foreach (var messageChunk in messageChunks)
                        {
                            _channel.BasicPublish(Common.Constants.ExchangeName, "", null, messageChunk.ToBytes());
                        }

                        Console.WriteLine($"File {e.Name} was sent.");

                        return;
                    }
                    catch
                    {
                        Thread.Sleep(ThreadTimeout);
                        currentRetry++;
                    }
                }
            });
        }
    }
}