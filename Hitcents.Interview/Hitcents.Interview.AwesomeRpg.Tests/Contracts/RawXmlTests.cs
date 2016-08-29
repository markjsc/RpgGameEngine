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
            Assert.AreEqual(5, playerElement.Elements.Length);

            //GameConfig/Elements/CoolGame/Player/Action(GainXP)
            Assert.IsNotNull(playerElement.Actions);
            Assert.AreEqual("GainXP", playerElement.Actions[0].Id);
            Assert.IsNotNull(playerElement.Actions[0].Setters);

            //GameConfig/Elements/CoolGame/Player/Action/GainXP/Setters
            var gainXpActionSetters = playerElement.Actions[0].Setters;
            Assert.IsInstanceOfType(gainXpActionSetters, typeof(Setter[]));
            Assert.AreEqual(1, gainXpActionSetters.Length);
            Assert.AreEqual("XP", gainXpActionSetters[0].Target);
            Assert.AreEqual("+", gainXpActionSetters[0].Operation);
            Assert.AreEqual("5", gainXpActionSetters[0].Value);

            //GameConfig/Elements/CoolGame/Player/Action(ClearHP)
            Assert.IsNotNull(playerElement.Actions);
            Assert.AreEqual("ClearHP", playerElement.Actions[1].Id);
            Assert.IsNotNull(playerElement.Actions[1].Setters);

            //GameConfig/Elements/CoolGame/Player/Action/ClearHP/Setters
            var clearHpActionSetters = playerElement.Actions[1].Setters;
            Assert.IsInstanceOfType(clearHpActionSetters, typeof(Setter[]));
            Assert.AreEqual(2, clearHpActionSetters.Length);
            Assert.AreEqual("HP", clearHpActionSetters[0].Target);
            Assert.AreEqual("=", clearHpActionSetters[0].Operation);
            Assert.AreEqual("0", clearHpActionSetters[0].Value);
            Assert.AreEqual("XP", clearHpActionSetters[1].Target);
            Assert.AreEqual("-", clearHpActionSetters[1].Operation);
            Assert.AreEqual("5", clearHpActionSetters[1].Value);

            //GameConfig/Elements/CoolGame/Player/Trigger(XP)
            Assert.IsNotNull(playerElement.Triggers);
            Assert.AreEqual("XP", playerElement.Triggers[0].Target);
            Assert.AreEqual(">=", playerElement.Triggers[0].Comparison);
            Assert.AreEqual("20", playerElement.Triggers[0].Value);
            Assert.IsNotNull(playerElement.Triggers[0].Setters);

            //GameConfig/Elements/CoolGame/Player/Trigger/XP/Setters
            var triggerXpSetters = playerElement.Triggers[0].Setters;
            Assert.IsInstanceOfType(triggerXpSetters, typeof(Setter[]));
            Assert.AreEqual(2, triggerXpSetters.Length);
            Assert.AreEqual("Level", triggerXpSetters[0].Target);
            Assert.AreEqual("=", triggerXpSetters[0].Operation);
            Assert.AreEqual("2", triggerXpSetters[0].Value);
            Assert.AreEqual("Name", triggerXpSetters[1].Target);
            Assert.AreEqual("=", triggerXpSetters[1].Operation);
            Assert.AreEqual("Super Bob", triggerXpSetters[1].Value);

            //GameConfig/Elements/CoolGame/Player/Trigger(Name)
            Assert.IsNotNull(playerElement.Triggers);
            Assert.AreEqual("Name", playerElement.Triggers[1].Target);
            Assert.AreEqual("==", playerElement.Triggers[1].Comparison);
            Assert.AreEqual("Super Bob", playerElement.Triggers[1].Value);
            Assert.IsNotNull(playerElement.Triggers[1].Setters);

            //GameConfig/Elements/CoolGame/Player/Trigger/Name/Setters
            var triggerNameSetters = playerElement.Triggers[1].Setters;
            Assert.IsInstanceOfType(triggerNameSetters, typeof(Setter[]));
            Assert.AreEqual(1, triggerNameSetters.Length);
            Assert.AreEqual("Mode", triggerNameSetters[0].Target);
            Assert.AreEqual("=", triggerNameSetters[0].Operation);
            Assert.AreEqual("Superman Mode!", triggerNameSetters[0].Value);

            //GameConfig/Elements/CoolGame/Player/Trigger(Mode)
            Assert.IsNotNull(playerElement.Triggers);
            Assert.AreEqual("Mode", playerElement.Triggers[2].Target);
            Assert.AreEqual("==", playerElement.Triggers[2].Comparison);
            Assert.AreEqual("Superman Mode!", playerElement.Triggers[2].Value);
            Assert.IsNotNull(playerElement.Triggers[2].Setters);

            //GameConfig/Elements/CoolGame/Player/Trigger/Mode/Setters
            var triggerModeSetters = playerElement.Triggers[2].Setters;
            Assert.IsInstanceOfType(triggerModeSetters, typeof(Setter[]));
            Assert.AreEqual(1, triggerModeSetters.Length);
            Assert.AreEqual("Level", triggerModeSetters[0].Target);
            Assert.AreEqual("=", triggerModeSetters[0].Operation);
            Assert.AreEqual("3", triggerModeSetters[0].Value);

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
