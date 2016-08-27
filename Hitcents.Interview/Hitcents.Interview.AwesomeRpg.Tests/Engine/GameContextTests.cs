using System;
using System.Collections.Generic;
using System.Linq;
using Hitcents.Interview.AwesomeRpg.Contracts.Models;
using Hitcents.Interview.AwesomeRpg.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hitcents.Interview.AwesomeRpg.Tests.Engine
{
    [TestClass]
    public class GameContextTests
    {
        [TestMethod]
        public void DoesBuildGameContextSetGameState()
        {
            //Arrange
            var gameContext = new GameContext();
            var gameState = new List<GameElement>()
            {
                new GameElement(),
                new GameElement(),
                new GameElement()
            };

            //Act
            gameContext.BuildGameContext(gameState);

            //Assert
            Assert.AreEqual(3, gameContext.GameState.Count);
        }

        [TestMethod, ExpectedException(typeof(Exception))]
        public void DoesRunActionFailWhenActionIdIsNotRecognized()
        {
            //Arrange
            var gameContext = new GameContext();

            //Act
            gameContext.RunAction("abc");

            //Assert - expected exception
        }

        [TestMethod]
        public void DoesRunActionPerformSetterWithAddOperation()
        {
            //Arrange
            const string ActionId = "TestActionId";
            const int ExpectedValue = 15;
            const string TestOperation = SetterOperations.Add;

            var gameContext = new GameContext();
            var gameState = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = "Level",
                    Value = 5,
                    Action = new GameAction()
                    {
                        Id = ActionId,
                        Setters = new List<GameSetter>()
                        {
                            new GameSetter() { TargetId = "Level", Operation = TestOperation, Value = 10 }
                        }
                    }
                }
            };
            gameContext.BuildGameContext(gameState);

            //Act
            gameContext.RunAction(ActionId);

            //Assert
            Assert.AreEqual(ExpectedValue, gameContext.GameState.First().Value);
        }

        [TestMethod]
        public void DoesRunActionPerformSetterWithAssignOperation()
        {
            //Arrange
            const string ActionId = "TestActionId";
            const int ExpectedValue = 10;
            const string TestOperation = SetterOperations.Assign;

            var gameContext = new GameContext();
            var gameState = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = "Level",
                    Value = 5,
                    Action = new GameAction()
                    {
                        Id = ActionId,
                        Setters = new List<GameSetter>()
                        {
                            new GameSetter() { TargetId = "Level", Operation = TestOperation, Value = 10 }
                        }
                    }
                }
            };
            gameContext.BuildGameContext(gameState);

            //Act
            gameContext.RunAction(ActionId);

            //Assert
            Assert.AreEqual(ExpectedValue, gameContext.GameState.First().Value);
        }

        [TestMethod]
        public void DoesRunActionPerformSetterWithDivideOperation()
        {
            //Arrange
            const string ActionId = "TestActionId";
            const int ExpectedValue = 5;
            const string TestOperation = SetterOperations.Divide;

            var gameContext = new GameContext();
            var gameState = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = "Level",
                    Value = 15,
                    Action = new GameAction()
                    {
                        Id = ActionId,
                        Setters = new List<GameSetter>()
                        {
                            new GameSetter() { TargetId = "Level", Operation = TestOperation, Value = 3 }
                        }
                    }
                }
            };
            gameContext.BuildGameContext(gameState);

            //Act
            gameContext.RunAction(ActionId);

            //Assert
            Assert.AreEqual(ExpectedValue, gameContext.GameState.First().Value);
        }

        [TestMethod]
        public void DoesRunActionPerformSetterWithMultiplyOperation()
        {
            //Arrange
            const string ActionId = "TestActionId";
            const int ExpectedValue = 15;
            const string TestOperation = SetterOperations.Multiply;

            var gameContext = new GameContext();
            var gameState = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = "Level",
                    Value = 5,
                    Action = new GameAction()
                    {
                        Id = ActionId,
                        Setters = new List<GameSetter>()
                        {
                            new GameSetter() { TargetId = "Level", Operation = TestOperation, Value = 3 }
                        }
                    }
                }
            };
            gameContext.BuildGameContext(gameState);

            //Act
            gameContext.RunAction(ActionId);

            //Assert
            Assert.AreEqual(ExpectedValue, gameContext.GameState.First().Value);
        }

        [TestMethod]
        public void DoesRunActionPerformSetterWithSubtractOperation()
        {
            //Arrange
            const string ActionId = "TestActionId";
            const int ExpectedValue = 2;
            const string TestOperation = SetterOperations.Subtract;

            var gameContext = new GameContext();
            var gameState = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = "Level",
                    Value = 5,
                    Action = new GameAction()
                    {
                        Id = ActionId,
                        Setters = new List<GameSetter>()
                        {
                            new GameSetter() { TargetId = "Level", Operation = TestOperation, Value = 3 }
                        }
                    }
                }
            };
            gameContext.BuildGameContext(gameState);

            //Act
            gameContext.RunAction(ActionId);

            //Assert
            Assert.AreEqual(ExpectedValue, gameContext.GameState.First().Value);
        }

        [TestMethod]
        public void DoesRunActionFireTriggerWhenComparisonIsEqual()
        {
            //Arrange
            const string TestComparison = TargetComparisons.Equal;
            const string TriggerTarget = "Shield";
            const string ActionId = "TestActionId";
            const int ExpectedValue = 50;

            var gameContext = new GameContext();
            var gameState = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = TriggerTarget,
                    Value = 0,
                    Action = new GameAction()
                    {
                        Id = ActionId,
                        Setters = new List<GameSetter>()
                        {
                            new GameSetter() { TargetId = "Level", Operation = SetterOperations.Assign, Value = 10 }
                        }
                    },
                    Trigger = new GameTrigger()
                    {
                        TargetId = "Level",
                        Comparison = TestComparison,
                        Value = 10,
                        Setters = new List<GameSetter>()
                        {
                            new GameSetter() { TargetId = TriggerTarget, Operation = SetterOperations.Assign, Value = 50 }
                        }
                    }
                },
                new GameElement() { Id = "Level" }
            };
            gameContext.BuildGameContext(gameState);

            //Act
            gameContext.RunAction(ActionId);

            //Assert
            Assert.AreEqual(ExpectedValue, gameContext.GameState.First().Value);
        }

        [TestMethod]
        public void DoesRunActionFireTriggerWhenComparisonIsGreaterThan()
        {
            //Arrange
            const string TestComparison = TargetComparisons.GreaterThan;
            const string TriggerTarget = "Shield";
            const string ActionId = "TestActionId";
            const int ExpectedValue = 50;

            var gameContext = new GameContext();
            var gameState = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = TriggerTarget,
                    Value = 0,
                    Action = new GameAction()
                    {
                        Id = ActionId,
                        Setters = new List<GameSetter>()
                        {
                            new GameSetter() { TargetId = "Level", Operation = SetterOperations.Assign, Value = 11 }
                        }
                    },
                    Trigger = new GameTrigger()
                    {
                        TargetId = "Level",
                        Comparison = TestComparison,
                        Value = 10,
                        Setters = new List<GameSetter>()
                        {
                            new GameSetter() { TargetId = TriggerTarget, Operation = SetterOperations.Assign, Value = 50 }
                        }
                    }
                },
                new GameElement() { Id = "Level" }
            };
            gameContext.BuildGameContext(gameState);

            //Act
            gameContext.RunAction(ActionId);

            //Assert
            Assert.AreEqual(ExpectedValue, gameContext.GameState.First().Value);
        }

        [TestMethod]
        public void DoesRunActionFireTriggerWhenComparisonIsGreaterThanOrEqual()
        {
            //Arrange
            const string TestComparison = TargetComparisons.GreaterThanOrEqual;
            const string TriggerTarget = "Shield";
            const string ActionId = "TestActionId";
            const int ExpectedValue = 50;

            var gameContext = new GameContext();
            var gameState = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = TriggerTarget,
                    Value = 0,
                    Action = new GameAction()
                    {
                        Id = ActionId,
                        Setters = new List<GameSetter>()
                        {
                            new GameSetter() { TargetId = "Level", Operation = SetterOperations.Assign, Value = 10 }
                        }
                    },
                    Trigger = new GameTrigger()
                    {
                        TargetId = "Level",
                        Comparison = TestComparison,
                        Value = 10,
                        Setters = new List<GameSetter>()
                        {
                            new GameSetter() { TargetId = TriggerTarget, Operation = SetterOperations.Assign, Value = 50 }
                        }
                    }
                },
                new GameElement() { Id = "Level" }
            };
            gameContext.BuildGameContext(gameState);

            //Act
            gameContext.RunAction(ActionId);

            //Assert
            Assert.AreEqual(ExpectedValue, gameContext.GameState.First().Value);
        }

        [TestMethod]
        public void DoesRunActionFireTriggerWhenComparisonIsLessThan()
        {
            //Arrange
            const string TestComparison = TargetComparisons.LessThan;
            const string TriggerTarget = "Shield";
            const string ActionId = "TestActionId";
            const int ExpectedValue = 50;

            var gameContext = new GameContext();
            var gameState = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = TriggerTarget,
                    Value = 0,
                    Action = new GameAction()
                    {
                        Id = ActionId,
                        Setters = new List<GameSetter>()
                        {
                            new GameSetter() { TargetId = "Level", Operation = SetterOperations.Assign, Value = 9 }
                        }
                    },
                    Trigger = new GameTrigger()
                    {
                        TargetId = "Level",
                        Comparison = TestComparison,
                        Value = 10,
                        Setters = new List<GameSetter>()
                        {
                            new GameSetter() { TargetId = TriggerTarget, Operation = SetterOperations.Assign, Value = 50 }
                        }
                    }
                },
                new GameElement() { Id = "Level" }
            };
            gameContext.BuildGameContext(gameState);

            //Act
            gameContext.RunAction(ActionId);

            //Assert
            Assert.AreEqual(ExpectedValue, gameContext.GameState.First().Value);
        }

        [TestMethod]
        public void DoesRunActionFireTriggerWhenComparisonIsLessThanOrEqual()
        {
            //Arrange
            const string TestComparison = TargetComparisons.LessThanOrEqual;
            const string TriggerTarget = "Shield";
            const string ActionId = "TestActionId";
            const int ExpectedValue = 50;

            var gameContext = new GameContext();
            var gameState = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = TriggerTarget,
                    Value = 0,
                    Action = new GameAction()
                    {
                        Id = ActionId,
                        Setters = new List<GameSetter>()
                        {
                            new GameSetter() { TargetId = "Level", Operation = SetterOperations.Assign, Value = 10 }
                        }
                    },
                    Trigger = new GameTrigger()
                    {
                        TargetId = "Level",
                        Comparison = TestComparison,
                        Value = 10,
                        Setters = new List<GameSetter>()
                        {
                            new GameSetter() { TargetId = TriggerTarget, Operation = SetterOperations.Assign, Value = 50 }
                        }
                    }
                },
                new GameElement() { Id = "Level" }
            };
            gameContext.BuildGameContext(gameState);

            //Act
            gameContext.RunAction(ActionId);

            //Assert
            Assert.AreEqual(ExpectedValue, gameContext.GameState.First().Value);
        }

        [TestMethod]
        public void DoesRunActionFireTriggerWhenComparisonIsNotEqual()
        {
            //Arrange
            const string TestComparison = TargetComparisons.NotEqual;
            const string TriggerTarget = "Shield";
            const string ActionId = "TestActionId";
            const int ExpectedValue = 50;

            var gameContext = new GameContext();
            var gameState = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = TriggerTarget,
                    Value = 0,
                    Action = new GameAction()
                    {
                        Id = ActionId,
                        Setters = new List<GameSetter>()
                        {
                            new GameSetter() { TargetId = "Level", Operation = SetterOperations.Assign, Value = 9 }
                        }
                    },
                    Trigger = new GameTrigger()
                    {
                        TargetId = "Level",
                        Comparison = TestComparison,
                        Value = 10,
                        Setters = new List<GameSetter>()
                        {
                            new GameSetter() { TargetId = TriggerTarget, Operation = SetterOperations.Assign, Value = 50 }
                        }
                    }
                },
                new GameElement() { Id = "Level" }
            };
            gameContext.BuildGameContext(gameState);

            //Act
            gameContext.RunAction(ActionId);

            //Assert
            Assert.AreEqual(ExpectedValue, gameContext.GameState.First().Value);
        }
    }
}
