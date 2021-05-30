using System;
using System.Buffers.Binary;

namespace Common
{
    public static class MessageChunkExtensions
    {
        private const int MetaDataSizes = 16 + 4 + 4;

        public static byte[] ToBytes(this MessageChunk messageChunk)
        {
            var result = new byte[MetaDataSizes + messageChunk.Data.Length];

            messageChunk.MessageId.TryWriteBytes(new Span<byte>(result, 0, 16));
            BinaryPrimitives.WriteInt32BigEndian(new Span<byte>(result, 16, 4), messageChunk.SequenceLength);
            BinaryPrimitives.WriteInt32BigEndian(new Span<byte>(result, 20, 4), messageChunk.Position);
            messageChunk.Data.CopyTo(new Span<byte>(result, 24, messageChunk.Data.Length));

            return result;
        }

        public static MessageChunk GetMessageChunk(this byte[] bytes)
        {
            var result = new MessageChunk
            {
                MessageId = new Guid(new Span<byte>(bytes, 0, 16)),
                SequenceLength = BinaryPrimitives.ReadInt32BigEndian(new Span<byte>(bytes, 16, 4)),
                Position = BinaryPrimitives.ReadInt32BigEndian(new Span<byte>(bytes, 20, 4)),
                Data = new byte[bytes.Length - MetaDataSizes]
            };
            new Span<byte>(bytes, 24, result.Data.Length).CopyTo(result.Data);

            return result;
        }
    }
}