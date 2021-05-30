using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Common
{
    public class MessageHelper
    {
        private readonly ConcurrentDictionary<Guid, List<MessageChunk>> _messageChunksDictionary;

        public MessageHelper()
        {
            _messageChunksDictionary = new ConcurrentDictionary<Guid, List<MessageChunk>>();
        }

        public static List<MessageChunk> GetMessageChunks(byte[] data)
        {
            var result = new List<MessageChunk>();

            var messageId = Guid.NewGuid();

            if (data.Length <= Constants.MaxMessageSize)
            {
                result.Add(new MessageChunk
                {
                    MessageId = messageId,
                    Position = 0,
                    SequenceLength = 1,
                    Data = data
                });
            }
            else
            {
                var sequenceLength = (data.Length - 1) / Constants.MaxMessageSize + 1;
                for (var i = 0; i < sequenceLength; i++)
                {
                    var chunkSize = i != sequenceLength - 1 ? Constants.MaxMessageSize : data.Length % Constants.MaxMessageSize;
                    var chunkData = new byte[chunkSize];

                    Array.Copy(data, Constants.MaxMessageSize * i, chunkData, 0, chunkSize);

                    var messageChunk = new MessageChunk
                    {
                        MessageId = messageId,
                        Position = i,
                        SequenceLength = sequenceLength,
                        Data = chunkData
                    };

                    result.Add(messageChunk);
                }
            }

            return result;
        }

        public bool TryGetMessage(MessageChunk messageChunk, out byte[] data)
        {
            if (messageChunk.SequenceLength == 1)
            {
                data = new byte[messageChunk.Data.Length];
                messageChunk.Data.CopyTo(data, 0);
                return true;
            }

            var chunksList = _messageChunksDictionary.GetOrAdd(messageChunk.MessageId, new List<MessageChunk>());

            chunksList.Add(messageChunk);

            if (chunksList.Count != messageChunk.SequenceLength)
            {
                data = null;
                return false;
            }

            chunksList.Sort((x, y) => x.Position.CompareTo(y.Position));
            var result = new List<byte>();
            for (var i = 0; i < chunksList.Count; i++)
            {
                result.AddRange(chunksList[i].Data);
            }

            data = result.ToArray();
            return true;
        }
    }
}