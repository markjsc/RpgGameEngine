using System.Collections.Generic;
using Hitcents.Interview.AwesomeRpg.Contracts.Models;
using Hitcents.Interview.AwesomeRpg.Engine;
using NUnit.Framework;

namespace Hitcents.Interview.AwesomeRpg.Tests.Engine
{
    /// <summary>
    /// These tests exercise the GameStateNavigator
    /// </summary>
    [TestFixture]
    public class GameStateNavigatorTests
    {
        #region Test Data
        private List<GameElement> TestGameState = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = "CoolGame",
                    Elements = new List<GameElement>()
                    {
                        new GameElement()
                        {
                            Id = "Player",
                            Elements = new List<GameElement>()
                            {
                                new GameElement() { Id = "Level", Value = "1" },
                                new GameElement() { Id = "XP", Value = "0" },
                                new GameElement() { Id = "HP", Value = "50" },
                                new GameElement() { Id = "Name", Value = "Bob" },
                                new GameElement()
                                {
                                    Id = "Mode",
                                    Value = "",
                                    Triggers = new List<GameTrigger>()
                                    {
                                        new GameTrigger()
                                        {
                                            TargetId = "Mode",
                                            Comparison = "==",
                                            Value = "Superman Mode!",
                                            Setters = new List<GameSetter>()
                                            {
                                                new GameSetter() { TargetId = "Level", Operation = "+", Value = "1" }
                                            }
                                        }
                                    }
                                }
                            },
                            Actions = new List<GameAction>()
                            {
                                new GameAction()
                                {
                                    Id = "GainXP",
                                    Setters = new List<GameSetter>()
                                    {
                                        new GameSetter() { TargetId = "XP", Operation = "+", Value = "10" }
                                    }
                                },
                                new GameAction()
                                {
                                    Id = "ClearHP",
                                    Setters = new List<GameSetter>()
                                    {
                                        new GameSetter() { TargetId = "HP", Operation = "=", Value = "0" },
                                        new GameSetter() { TargetId = "XP", Operation = "-", Value = "5" }
                                    }
                                }
                            },
                            Triggers = new List<GameTrigger>()
                            {
                                new GameTrigger()
                                {
                                    TargetId = "XP",
                                    Comparison = ">=",
                                    Value = "10",
                                    Setters = new List<GameSetter>()
                                    {
                                        new GameSetter() { TargetId = "Level", Operation = "=", Value = "2" },
                                        new GameSetter() { TargetId = "Name", Operation = "=", Value = "Super Bob" }
                                    }
                                },
                                new GameTrigger()
                                {
                                    TargetId = "Name",
                                    Comparison = "==",
                                    Value = "Super Bob",
                                    Setters = new List<GameSetter>()
                                    {
                                        new GameSetter() { TargetId = "Mode", Operation = "=", Value = "Superman Mode!" }
                                    }
                                },
                                new GameTrigger()
                                {
                                    TargetId = "Mode",
                                    Comparison = "==",
                                    Value = "Superman Mode!",
                                    Setters = new List<GameSetter>()
                                    {
                                        new GameSetter() { TargetId = "Level", Operation ="=", Value = "3" }
                                    }
                                }
                            }
                        }
                    }
                }
            };

        #endregion

        [Test]
        public void DoesGetElementByIdReturnExpectedElement()
        {
            //Arrange
            var navigator = new GameStateNavigator();

            //Act
            var actual = navigator.GetElementById(this.TestGameState, "Level");

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual("1", actual.Value);
        }

        [Test]
        public void DoesGetElementByIdReturnExpectedElementRegardlessOfCase()
        {
            //Arrange
            var navigator = new GameStateNavigator();

            //Act
            var actual = navigator.GetElementById(this.TestGameState, "LEVEL");

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual("1", actual.Value);
        }

        [Test]
        public void DoesGetElementByIdReturnNullWhenElementIdDoesNotExist()
        {
            //Arrange
            var navigator = new GameStateNavigator();

            //Act
            var actual = navigator.GetElementById(this.TestGameState, "Unknown Element");

            //Assert
            Assert.IsNull(actual);
        }

        [Test]
        public void DoesGetActionByIdReturnExpectedAction()
        {
            //Arrange
            var navigator = new GameStateNavigator();

            //Act
            var actual = navigator.GetActionById(this.TestGameState, "ClearHP");

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.Id, "ClearHP");
        }

        [Test]
        public void DoesGetActionByIdReturnExpectedActionRegardlessOfCase()
        {
            //Arrange
            var navigator = new GameStateNavigator();

            //Act
            var actual = navigator.GetActionById(this.TestGameState, "CLEARHP");

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.Id, "ClearHP");
        }

        [Test]
        public void DoesGetActionByIdReturnNullWhenActionDoesNotExist()
        {
            //Arrange
            var navigator = new GameStateNavigator();

            //Act
            var actual = navigator.GetActionById(this.TestGameState, "Unknown Action");

            //Assert
            Assert.IsNull(actual);
        }

        [Test]
        public void DoesGetTriggersByTargetReturnExpectedTriggers()
        {
            //Arrange
            var navigator = new GameStateNavigator();

            //Act
            var actual = navigator.GetTriggersByTarget(this.TestGameState, "Mode");

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual("Mode", actual[0].TargetId);
            Assert.AreEqual("Mode", actual[1].TargetId);
        }

        [Test]
        public void DoesGetTriggersByTargetReturnExpectedTriggersRegardlessOfCase()
        {
            //Arrange
            var navigator = new GameStateNavigator();

            //Act
            var actual = navigator.GetTriggersByTarget(this.TestGameState, "mode");

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual("Mode", actual[0].TargetId);
            Assert.AreEqual("Mode", actual[1].TargetId);
        }
    }
}
