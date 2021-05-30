using System.Collections.Generic;

namespace Common
{
    public class Constants
    {
        public const string ExchangeName = "CentralServiceExchange";

        public static List<string> FileFormats = new List<string> { ".pdf", ".mp4" };

        public const string Host = "localhost";

        public const int MaxMessageSize = 8 * 1024;
    }
}