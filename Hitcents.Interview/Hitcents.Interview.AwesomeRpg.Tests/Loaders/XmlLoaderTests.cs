using Hitcents.Interview.AwesomeRpg.Contracts.Models.RawXml;
using Hitcents.Interview.AwesomeRpg.Loaders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hitcents.Interview.AwesomeRpg.Tests.Loaders
{
    /// <summary>
    /// Summary description for XmlLoaderTests
    /// </summary>
    [TestClass]
    public class XmlLoaderTests
    {
        [TestMethod]
        public void DoesLoadXmlReturnExpectedListOfElements()
        {
            //Arrange
            var loader = new XmlLoader();

            //Act
            var elements = loader.LoadXml(Constants.SampleXml);

            //Assert
            Assert.AreEqual(1, elements.Count);

            //GameConfig/Elements/CoolGame
            var coolGameElement = elements[0];
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
            Assert.AreEqual("GainXP", playerElement.Action.Id);
            Assert.IsNotNull(playerElement.Action.Setters);

            //GameConfig/Elements/CoolGame/Player/Action/Setter
            var actionSetter = playerElement.Action.Setters;
            Assert.IsInstanceOfType(actionSetter, typeof(Setter[]));
            Assert.AreEqual(1, actionSetter.Length);
            Assert.AreEqual("XP", actionSetter[0].Target);
            Assert.AreEqual("+", actionSetter[0].Operation);
            Assert.AreEqual("10", actionSetter[0].Value);

            //GameConfig/Elements/CoolGame/Player/Trigger
            Assert.IsNotNull(playerElement.Trigger);
            Assert.AreEqual("XP", playerElement.Trigger.Target);
            Assert.AreEqual(">=", playerElement.Trigger.Comparison);
            Assert.AreEqual("10", playerElement.Trigger.Value);
            Assert.IsNotNull(playerElement.Trigger.Setters);

            //GameConfig/Elements/CoolGame/Player/Trigger/Setter
            var triggerSetter = playerElement.Trigger.Setters;
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
