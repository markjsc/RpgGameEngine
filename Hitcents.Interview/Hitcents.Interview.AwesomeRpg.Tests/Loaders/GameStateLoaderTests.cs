using System;
using System.Collections.Generic;
using System.Linq;
using Hitcents.Interview.AwesomeRpg.Contracts.Models.RawXml;
using Hitcents.Interview.AwesomeRpg.Loaders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hitcents.Interview.AwesomeRpg.Tests.Loaders
{
    [TestClass]
    public class GameStateLoaderTests
    {
        [TestMethod, ExpectedException(typeof(Exception))]
        public void DoesLoadGameStateThrowExceptionWhenDuplicateElementIdIsAddedInSameLevel()
        {
            var gameStateLoader = new GameStateLoader();

            var elements = new List<Element>()
            {
                new Element()
                {
                    Id = "abc"                    
                },
                new Element()
                {
                    Id = "abc"
                }
            };

            //Act
            var actual = gameStateLoader.LoadGameState(elements);

            //Assert - expected exception
        }

        [TestMethod, ExpectedException(typeof(Exception))]
        public void DoesLoadGameStateThrowExceptionWhenDuplicateElementIdIsAddedInDifferentLevel()
        {
            var gameStateLoader = new GameStateLoader();

            var elements = new List<Element>()
            {
                new Element()
                {
                    Id = "abc",
                    Elements = new[]
                    {
                        new Element() { Id = "abc" }
                    }
                }                
            };

            //Act
            var actual = gameStateLoader.LoadGameState(elements);

            //Assert - expected exception
        }

        [TestMethod, ExpectedException(typeof(Exception))]
        public void DoesLoadGameStateThrowExceptionWhenDuplicateActionIdIsAdded()
        {
            var gameStateLoader = new GameStateLoader();

            var elements = new List<Element>()
            {
                new Element()
                {
                    Actions = new[] { new AwesomeRpg.Contracts.Models.RawXml.Action() { Id = "abc" } }
                },
                new Element()
                {
                    Actions = new[] { new AwesomeRpg.Contracts.Models.RawXml.Action() { Id = "abc" } }
                }
            };

            //Act
            var actual = gameStateLoader.LoadGameState(elements);

            //Assert - expected exception
        }

        [TestMethod]
        public void DoesLoadGameStateWorkWithHappyPath()
        {
            //Arrange
            var gameStateLoader = new GameStateLoader();

            var elements = new List<Element>()
            {
                new Element()
                {
                    Id = "CoolGame",
                    Elements = new[]
                    {
                        new Element()
                        {
                            Id = "Player",
                            Elements = new[]
                            {
                                new Element() { Id = "Level", Value = "1" },
                                new Element() { Id = "XP", Value = "0" },
                                new Element() { Id = "HP", Value = "50" },
                                new Element() { Id = "Name", Value = "Bob" },
                                new Element() { Id = "Mode", Value = "" }
                            },
                            Actions = new[]
                            {
                                new AwesomeRpg.Contracts.Models.RawXml.Action()
                                {
                                    Id = "GainXP",
                                    Setters = new[]
                                    {
                                        new Setter() { Target = "XP", Operation = "+", Value = "10" }
                                    }
                                },
                                new AwesomeRpg.Contracts.Models.RawXml.Action()
                                {
                                    Id = "ClearHP",
                                    Setters = new[]
                                    {
                                        new Setter() { Target = "HP", Operation = "=", Value = "0" },
                                        new Setter() { Target = "XP", Operation = "-", Value = "5" }
                                    }
                                }
                            },
                            Triggers = new[]
                            {
                                new Trigger()
                                {
                                    Target = "XP",
                                    Comparison = ">=",
                                    Value = "10",
                                    Setters = new[]
                                    {
                                        new Setter() { Target = "Level", Operation = "=", Value = "2" },
                                        new Setter() { Target = "Name", Operation = "=", Value = "Super Bob" }
                                    }
                                },
                                new Trigger()
                                {
                                    Target = "Name",
                                    Comparison = "==",
                                    Value = "Super Bob",
                                    Setters = new[]
                                    {
                                        new Setter() { Target = "Mode", Operation = "=", Value = "Superman Mode!" }
                                    }
                                },
                                new Trigger()
                                {
                                    Target = "Mode",
                                    Comparison = "==",
                                    Value = "Superman Mode!",
                                    Setters = new[]
                                    {
                                        new Setter() { Target = "Level", Operation ="=", Value = "3" }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            //Act
            var gameState = gameStateLoader.LoadGameState(elements);

            //Assert
            var coolGameElement = gameState[0];
            Assert.AreEqual("CoolGame", coolGameElement.Id);
            Assert.AreEqual(1, coolGameElement.Elements.Count);
            
            var playerElement = coolGameElement.Elements[0];
            Assert.AreEqual("Player", playerElement.Id);
            Assert.AreEqual(5, playerElement.Elements.Count);

            //GameConfig/Elements/CoolGame/Player/Action(GainXP)
            Assert.IsNotNull(playerElement.Actions);
            Assert.AreEqual("GainXP", playerElement.Actions[0].Id);
            Assert.IsNotNull(playerElement.Actions[0].Setters);

            //GameConfig/Elements/CoolGame/Player/Action/GainXP/Setters
            var gainXpActionSetters = playerElement.Actions[0].Setters;
            Assert.AreEqual(1, gainXpActionSetters.Count);
            Assert.AreEqual("XP", gainXpActionSetters[0].TargetId);
            Assert.AreEqual("+", gainXpActionSetters[0].Operation);
            Assert.AreEqual("10", gainXpActionSetters[0].Value);

            //GameConfig/Elements/CoolGame/Player/Action(ClearHP)
            Assert.IsNotNull(playerElement.Actions);
            Assert.AreEqual("ClearHP", playerElement.Actions[1].Id);
            Assert.IsNotNull(playerElement.Actions[1].Setters);

            //GameConfig/Elements/CoolGame/Player/Action/ClearHP/Setters
            var clearHpActionSetters = playerElement.Actions[1].Setters;
            Assert.AreEqual(2, clearHpActionSetters.Count);
            Assert.AreEqual("HP", clearHpActionSetters[0].TargetId);
            Assert.AreEqual("=", clearHpActionSetters[0].Operation);
            Assert.AreEqual("0", clearHpActionSetters[0].Value);
            Assert.AreEqual("XP", clearHpActionSetters[1].TargetId);
            Assert.AreEqual("-", clearHpActionSetters[1].Operation);
            Assert.AreEqual("5", clearHpActionSetters[1].Value);

            //GameConfig/Elements/CoolGame/Player/Trigger(XP)
            Assert.IsNotNull(playerElement.Triggers[0]);
            Assert.AreEqual("XP", playerElement.Triggers[0].TargetId);
            Assert.AreEqual(">=", playerElement.Triggers[0].Comparison);
            Assert.AreEqual("10", playerElement.Triggers[0].Value);
            Assert.IsNotNull(playerElement.Triggers[0].Setters);

            //GameConfig/Elements/CoolGame/Player/Trigger/XP/Setters
            var triggerXpSetters = playerElement.Triggers[0].Setters;
            Assert.AreEqual(2, triggerXpSetters.Count);
            Assert.AreEqual("Level", triggerXpSetters[0].TargetId);
            Assert.AreEqual("=", triggerXpSetters[0].Operation);
            Assert.AreEqual("2", triggerXpSetters[0].Value);
            Assert.AreEqual("Name", triggerXpSetters[1].TargetId);
            Assert.AreEqual("=", triggerXpSetters[1].Operation);
            Assert.AreEqual("Super Bob", triggerXpSetters[1].Value);

            //GameConfig/Elements/CoolGame/Player/Trigger(Name)
            Assert.IsNotNull(playerElement.Triggers[1]);
            Assert.AreEqual("Name", playerElement.Triggers[1].TargetId);
            Assert.AreEqual("==", playerElement.Triggers[1].Comparison);
            Assert.AreEqual("Super Bob", playerElement.Triggers[1].Value);
            Assert.IsNotNull(playerElement.Triggers[1].Setters);

            //GameConfig/Elements/CoolGame/Player/Trigger/Name/Setters
            var triggerNameSetters = playerElement.Triggers[1].Setters;
            Assert.AreEqual(1, triggerNameSetters.Count);
            Assert.AreEqual("Mode", triggerNameSetters[0].TargetId);
            Assert.AreEqual("=", triggerNameSetters[0].Operation);
            Assert.AreEqual("Superman Mode!", triggerNameSetters[0].Value);

            //GameConfig/Elements/CoolGame/Player/Trigger(Mode)
            Assert.IsNotNull(playerElement.Triggers[2]);
            Assert.AreEqual("Mode", playerElement.Triggers[2].TargetId);
            Assert.AreEqual("==", playerElement.Triggers[2].Comparison);
            Assert.AreEqual("Superman Mode!", playerElement.Triggers[2].Value);
            Assert.IsNotNull(playerElement.Triggers[2].Setters);

            //GameConfig/Elements/CoolGame/Player/Trigger/Mode/Setters
            var triggerModeSetters = playerElement.Triggers[2].Setters;
            Assert.AreEqual(1, triggerModeSetters.Count);
            Assert.AreEqual("Level", triggerModeSetters[0].TargetId);
            Assert.AreEqual("=", triggerModeSetters[0].Operation);
            Assert.AreEqual("3", triggerModeSetters[0].Value);

            //GameConfig/Elements/CoolGame/Player/Level
            Assert.IsNotNull(playerElement.Elements[0]);
            var levelElement = playerElement.Elements[0];
            Assert.AreEqual("Level", levelElement.Id);
            Assert.AreEqual("1", levelElement.Value);

            //GameConfig/Elements/CoolGame/Player/XP
            Assert.IsNotNull(playerElement.Elements.Skip(1).First());
            var xpElement = playerElement.Elements.Skip(1).First();
            Assert.AreEqual("XP", xpElement.Id);
            Assert.AreEqual("0", xpElement.Value);

            //GameConfig/Elements/CoolGame/Player/HP
            Assert.IsNotNull(playerElement.Elements.Skip(2).First());
            var hpElement = playerElement.Elements.Skip(2).First();
            Assert.AreEqual("HP", hpElement.Id);
            Assert.AreEqual("50", hpElement.Value);
        }
    }
}
