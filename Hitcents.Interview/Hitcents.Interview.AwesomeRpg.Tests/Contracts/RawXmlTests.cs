using System.IO;
using Hitcents.Interview.AwesomeRpg.Contracts.Models.RawXml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            var deserializer = new System.Xml.Serialization.XmlSerializer(typeof(GameConfig));
            //In keeping with the hacky opening and closing tags in the XML Loader code:
            var completeXml = string.Format("<GameConfig><Elements>{0}</Elements></GameConfig>", Constants.SampleXml);

            //Act
            var xmlRoot = (GameConfig)deserializer.Deserialize(new StringReader(completeXml));

            //Assert
            Assert.IsNotNull(xmlRoot);
            Assert.IsInstanceOfType(xmlRoot, typeof(GameConfig));
            Assert.IsNotNull(xmlRoot.Elements);
            Assert.AreEqual(1, xmlRoot.Elements.Length);

            //GameConfig/Elements/CoolGame
            var coolGameElement = xmlRoot.Elements[0];
            Assert.IsInstanceOfType(coolGameElement, typeof(Element));
            Assert.AreEqual("CoolGame", coolGameElement.Id);
            Assert.AreEqual(1, coolGameElement.Elements.Length);

            //GameConfig/Elements/CoolGame/Player
            var playerElement = coolGameElement.Elements[0];
            Assert.IsInstanceOfType(playerElement, typeof(Element));
            Assert.AreEqual("Player", playerElement.Id);
            Assert.AreEqual(3, playerElement.Elements.Length);

            //GameConfig/Elements/CoolGame/Player/Action
            Assert.IsNotNull(playerElement.Action);
            Assert.AreEqual("GainXP", playerElement.Action[0].Id);
            Assert.IsNotNull(playerElement.Action[0].Setters);

            //GameConfig/Elements/CoolGame/Player/Action/Setter
            var actionSetter = playerElement.Action[0].Setters;
            Assert.IsInstanceOfType(actionSetter, typeof(Setter[]));
            Assert.AreEqual(1, actionSetter.Length);
            Assert.AreEqual("XP", actionSetter[0].Target);
            Assert.AreEqual("+", actionSetter[0].Operation);
            Assert.AreEqual("10", actionSetter[0].Value);

            //GameConfig/Elements/CoolGame/Player/Trigger
            Assert.IsNotNull(playerElement.Trigger);
            Assert.AreEqual("XP", playerElement.Trigger[0].Target);
            Assert.AreEqual(">=", playerElement.Trigger[0].Comparison);
            Assert.AreEqual("10", playerElement.Trigger[0].Value);
            Assert.IsNotNull(playerElement.Trigger[0].Setters);

            //GameConfig/Elements/CoolGame/Player/Trigger/Setter
            var triggerSetter = playerElement.Trigger[0].Setters;
            Assert.IsInstanceOfType(triggerSetter, typeof(Setter[]));
            Assert.AreEqual(1, triggerSetter.Length);
            Assert.AreEqual("Level", triggerSetter[0].Target);
            Assert.AreEqual("=", triggerSetter[0].Operation);
            Assert.AreEqual("2", triggerSetter[0].Value);

            //GameConfig/Elements/CoolGame/Player/Level
            Assert.IsNotNull(playerElement.Elements[0]);
            var levelElement = playerElement.Elements[0];
            Assert.AreEqual("Level", levelElement.Id);
            Assert.AreEqual("1", levelElement.Value);

            //GameConfig/Elements/CoolGame/Player/XP
            Assert.IsNotNull(playerElement.Elements[1]);
            var xpElement = playerElement.Elements[1];
            Assert.AreEqual("XP", xpElement.Id);
            Assert.AreEqual("0", xpElement.Value);

            //GameConfig/Elements/CoolGame/Player/HP
            Assert.IsNotNull(playerElement.Elements[2]);
            var hpElement = playerElement.Elements[2];
            Assert.AreEqual("HP", hpElement.Id);
            Assert.AreEqual("50", hpElement.Value);
        }
    }
}
