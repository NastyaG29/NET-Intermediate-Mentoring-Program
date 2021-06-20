using ExpressionTrees.Task2.ExpressionMapping.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ExpressionTrees.Task2.ExpressionMapping.Tests
{
    [TestClass]
    public class ExpressionMappingTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var recepient = new Recipient()
            {
                Id = 1,
                Age = 22,
                FullName = "Alexander Smirnov",
                HomeAddress = "Minsk, Nezavisimosty 15"
            };

            var customNameMapping = new Dictionary<string, string>()
            {
                [$"{nameof(Recipient.FullName)}"] = $"{nameof(Sender.Name)}",
                [$"{nameof(Recipient.HomeAddress)}"] = $"{nameof(Sender.Address)}",
            };

            var mapGenerator = new MappingGenerator().AddCustomMapping(customNameMapping);
            var mapper = mapGenerator.Generate<Recipient, Sender>();

            var sender = mapper.Map(recepient);

            Assert.AreEqual(recepient.Id, sender.Id);
            Assert.AreEqual(recepient.FullName, sender.Name);
            Assert.AreEqual(recepient.HomeAddress, sender.Address);
        }
    }
}