using Hitcents.Interview.AwesomeRpg.Contracts.Models.RawXml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Hitcents.Interview.AwesomeRpg.Tests.Contracts
{
    [TestClass]
    public class RawXmlTests
    {
        /// <summary>
        /// This validates that the Raw XML is correctly deserialized into the expected object set.
        /// There are a large number of asserts here just because breaking it into separate smaller
        /// tests didn't seem worthwhile for a single deserialize operation.
        /// This will give us a gut check in case the XML Schema or Raw XML classes happen to change.
        /// </summary>
        [TestMethod]
        public void DoesRawXmlDeserializeIntoContractsUsingBasicXml()
        {
            //Arrange
            var rawXml = "<GameConfig>" +
"<Elements>" +
    "<Element Id=\"CoolGame\">" +
        "<Element Id=\"Player\">" +
           "<Element Id=\"Level\" Value=\"1\" />" +
              "<Element Id=\"XP\" Value=\"0\" />" +
                "<Element Id=\"HP\" Value=\"50\" />" +
                "<Action Id=\"GainXP\">" +
                      "<Setter Target=\"XP\" Operation=\"+\" Value=\"10\" />" +
                "</Action>" +
                "<Trigger Target=\"XP\" Comparison=\">=\" Value=\"10\">" +
                    "<Setter Target=\"Level\" Operation=\"=\" Value=\"2\" />" +
                "</Trigger>" +
            "</Element>" +
        "</Element>" +
    "</Elements>" +
"</GameConfig>";
            var deserializer = new System.Xml.Serialization.XmlSerializer(typeof(GameConfig));
            
            //Act
            var elements = (GameConfig)deserializer.Deserialize(new StringReader(rawXml));

            //Assert
            Assert.IsNotNull(elements);
            Assert.IsInstanceOfType(elements, typeof(GameConfig));
            Assert.IsNotNull(elements.Elements);
            Assert.AreEqual(1, elements.Elements.Length);

            var coolGameElement = elements.Elements[0];
            Assert.IsInstanceOfType(coolGameElement, typeof(Element));
            Assert.AreEqual("CoolGame", coolGameElement.Id);
            Assert.AreEqual(1, coolGameElement.Elements.Length);

            var playerElement = coolGameElement.Elements[0];
            Assert.IsInstanceOfType(playerElement, typeof(Element));
            Assert.AreEqual("Player", playerElement.Id);
            Assert.AreEqual(3, playerElement.Elements.Length);

            Assert.IsNotNull(playerElement.Action);
            Assert.AreEqual("GainXP", playerElement.Action.Id);
            Assert.IsNotNull(playerElement.Action.Setter);
            
            var actionSetter = playerElement.Action.Setter;
            Assert.IsInstanceOfType(actionSetter, typeof(Setter));
            Assert.AreEqual("XP", actionSetter.Target);
            Assert.AreEqual("+", actionSetter.Operation);
            Assert.AreEqual("10", actionSetter.Value);

            Assert.IsNotNull(playerElement.Trigger);
            Assert.AreEqual("XP", playerElement.Trigger.Target);
            Assert.AreEqual(">=", playerElement.Trigger.Comparison);
            Assert.AreEqual("10", playerElement.Trigger.Value);
            Assert.IsNotNull(playerElement.Trigger.Setter);

            var triggerSetter = playerElement.Trigger.Setter;
            Assert.IsInstanceOfType(triggerSetter, typeof(Setter));
            Assert.AreEqual("Level", triggerSetter.Target);
            Assert.AreEqual("=", triggerSetter.Operation);
            Assert.AreEqual("2", triggerSetter.Value);

        }
    }
}
