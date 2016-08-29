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
                    Action = new[] { new AwesomeRpg.Contracts.Models.RawXml.Action() { Id = "abc" } }
                },
                new Element()
                {
                    Action = new[] { new AwesomeRpg.Contracts.Models.RawXml.Action() { Id = "abc" } }
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
                                new Element() { Id = "HP", Value = "50" }
                            },
                            Action =new[]
                            {
                                new AwesomeRpg.Contracts.Models.RawXml.Action()
                                {
                                    Id = "GainXP",
                                    Setters = new[]
                                    {
                                        new Setter() { Target = "XP", Operation = "+", Value = "10" }
                                    }
                            }
                            },
                            Trigger = new[]
                            {
                                new Trigger()
                                {
                                    Target = "XP",
                                    Comparison = ">=",
                                    Value = "10",
                                    Setters = new[]
                                    {
                                        new Setter() { Target = "Level", Operation = "=", Value = "2" }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            //Act
            var actual = gameStateLoader.LoadGameState(elements);

            //Assert
            var coolGameElement = actual.First();
            Assert.AreEqual("CoolGame", coolGameElement.Id);
            Assert.AreEqual(1, coolGameElement.Elements.Count);
            
            var playerElement = coolGameElement.Elements.First();
            Assert.AreEqual("Player", playerElement.Id);
            Assert.AreEqual(3, playerElement.Elements.Count);

            //GameConfig/Elements/CoolGame/Player/Action
            Assert.IsNotNull(playerElement.Actions);
            Assert.AreEqual("GainXP", playerElement.Actions[0].Id);
            Assert.IsNotNull(playerElement.Actions[0].Setters);

            //GameConfig/Elements/CoolGame/Player/Action/Setter
            var actionSetter = playerElement.Actions[0].Setters;
            Assert.AreEqual(1, actionSetter.Count);
            Assert.AreEqual("XP", actionSetter.First().TargetId);
            Assert.AreEqual("+", actionSetter.First().Operation);
            Assert.AreEqual("10", actionSetter.First().Value);

            //GameConfig/Elements/CoolGame/Player/Trigger
            Assert.IsNotNull(playerElement.Triggers);
            Assert.AreEqual("XP", playerElement.Triggers[0].TargetId);
            Assert.AreEqual(">=", playerElement.Triggers[0].Comparison);
            Assert.AreEqual("10", playerElement.Triggers[0].Value);
            Assert.IsNotNull(playerElement.Triggers[0].Setters);

            //GameConfig/Elements/CoolGame/Player/Trigger/Setter
            var triggerSetter = playerElement.Triggers[0].Setters;
            Assert.AreEqual(1, triggerSetter.Count);
            Assert.AreEqual("Level", triggerSetter.First().TargetId);
            Assert.AreEqual("=", triggerSetter.First().Operation);
            Assert.AreEqual("2", triggerSetter.First().Value);

            //GameConfig/Elements/CoolGame/Player/Level
            Assert.IsNotNull(playerElement.Elements.First());
            var levelElement = playerElement.Elements.First();
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
