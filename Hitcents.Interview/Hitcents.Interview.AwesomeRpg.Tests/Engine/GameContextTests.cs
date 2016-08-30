using System;
using System.Collections.Generic;
using System.Linq;
using Hitcents.Interview.AwesomeRpg.Contracts.Interfaces;
using Hitcents.Interview.AwesomeRpg.Contracts.Models;
using Hitcents.Interview.AwesomeRpg.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hitcents.Interview.AwesomeRpg.Tests.Engine
{
    [TestClass]
    public class GameContextTests
    {
        private class BadGameStateNavigator : IGameStateNavigator
        {
            public GameAction GetActionById(List<GameElement> elements, string actionId)
            {
                throw new Exception("Fake error!");
            }

            public GameElement GetElementById(List<GameElement> elements, string id)
            {
                throw new Exception("Fake error!");
            }

            public List<GameTrigger> GetTriggersByTarget(List<GameElement> elements, string targetId)
            {
                throw new Exception("Fake error!");
            }
        }

        private IGameContext GetGameContext()
        {
            var gameStateNavigator = new GameStateNavigator();
            return new GameContext(gameStateNavigator);
        }

        [TestMethod]
        public void DoesBuildGameContextSetGameState()
        {
            //Arrange
            var gameContext = this.GetGameContext();
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
            var gameContext = this.GetGameContext();

            //Act
            gameContext.RunAction("abc");

            //Assert - expected exception
        }

        [TestMethod, ExpectedException(typeof(Exception))]
        public void DoesRunActionFailWhenGameStateNavigatorThrowsException()
        {
            //Arrange
            var gameContext = new GameContext(new BadGameStateNavigator());

            //Act
            gameContext.RunAction("abc");

            //Assert - expected exception
        }

        [TestMethod]
        public void DoesRunActionIncrementRunCount()
        {
            //Arrange
            const string ActionId = "TestActionId";

            var gameContext = this.GetGameContext();
            var gameState = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = "Level",
                    Value = "5",
                    Actions = new List<GameAction>()
                    {
                        new GameAction()
                        {
                            Id = ActionId
                        }
                    }
                }
            };
            gameContext.BuildGameContext(gameState);

            //Act
            gameContext.RunAction(ActionId);
            var action = gameContext.GameState.First().Actions.First();

            //Assert
            Assert.IsNotNull(action);
            Assert.AreEqual(1, action.RunCount);
        }

        [TestMethod]
        public void DoesRunActionPerformSetterWithAssignOperationWithNumerics()
        {
            //Arrange
            const string ActionId = "TestActionId";
            const string ExpectedValue = "10";
            const string TestOperation = SetterOperations.Assign;

            var gameContext = this.GetGameContext();
            var gameState = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = "Level",
                    Value = "5",
                    Actions = new List<GameAction>()
                    {
                        new GameAction()
                        {
                            Id = ActionId,
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = "Level", Operation = TestOperation, Value = "10" }
                            }
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
        public void DoesRunActionPerformSetterWithAssignOperationWithAlphas()
        {
            //Arrange
            const string ActionId = "TestActionId";
            const string ExpectedValue = "abc";
            const string TestOperation = SetterOperations.Assign;

            var gameContext = this.GetGameContext();
            var gameState = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = "Level",
                    Value = "5",
                    Actions = new List<GameAction>()
                    {
                        new GameAction()
                        {
                            Id = ActionId,
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = "Level", Operation = TestOperation, Value = "abc" }
                            }
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
        public void DoesRunActionPerformSetterWithAddOperationWithNumerics()
        {
            //Arrange
            const string ActionId = "TestActionId";
            const string ExpectedValue = "15";
            const string TestOperation = SetterOperations.Add;

            var gameContext = this.GetGameContext();
            var gameState = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = "Level",
                    Value = "5",
                    Actions = new List<GameAction>()
                    {
                        new GameAction()
                        {
                            Id = ActionId,
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = "Level", Operation = TestOperation, Value = "10" }
                            }
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
        public void DoesRunActionPerformSetterWithAddOperationWithAlphaSetter()
        {
            //Arrange
            const string ActionId = "TestActionId";
            const string ExpectedValue = "5";
            const string TestOperation = SetterOperations.Add;

            var gameContext = this.GetGameContext();
            var gameState = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = "Level",
                    Value = "5",
                    Actions = new List<GameAction>()
                    {
                        new GameAction()
                        {
                            Id = ActionId,
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = "Level", Operation = TestOperation, Value = "a" }
                            }
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
        public void DoesRunActionPerformSetterWithAddOperationWithAlphaValue()
        {
            //Arrange
            const string ActionId = "TestActionId";
            const string ExpectedValue = "10";
            const string TestOperation = SetterOperations.Add;

            var gameContext = this.GetGameContext();
            var gameState = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = "Level",
                    Value = "a",
                    Actions = new List<GameAction>()
                    {
                        new GameAction()
                        {
                            Id = ActionId,
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = "Level", Operation = TestOperation, Value = "10" }
                            }
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
        public void DoesRunActionPerformSetterWithDivideOperationWithNonZeroNumerics()
        {
            //Arrange
            const string ActionId = "TestActionId";
            const string ExpectedValue = "5";
            const string TestOperation = SetterOperations.Divide;

            var gameContext = this.GetGameContext();
            var gameState = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = "Level",
                    Value = "15",
                    Actions = new List<GameAction>()
                    {
                        new GameAction()
                        {
                            Id = ActionId,
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = "Level", Operation = TestOperation, Value = "3" }
                            }
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
        public void DoesRunActionPerformSetterWithDivideOperationWithZeroSetter()
        {
            //Arrange
            const string ActionId = "TestActionId";
            const string ExpectedValue = "15";
            const string TestOperation = SetterOperations.Divide;

            var gameContext = this.GetGameContext();
            var gameState = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = "Level",
                    Value = "15",
                    Actions = new List<GameAction>()
                    {
                        new GameAction()
                        {
                            Id = ActionId,
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = "Level", Operation = TestOperation, Value = "0" }
                            }
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
        public void DoesRunActionPerformSetterWithDivideOperationWithAlphaSetter()
        {
            //Arrange
            const string ActionId = "TestActionId";
            const string ExpectedValue = "15";
            const string TestOperation = SetterOperations.Divide;

            var gameContext = this.GetGameContext();
            var gameState = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = "Level",
                    Value = "15",
                    Actions = new List<GameAction>()
                    {
                        new GameAction()
                        {
                            Id = ActionId,
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = "Level", Operation = TestOperation, Value = "a" }
                            }
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
        public void DoesRunActionPerformSetterWithDivideOperationWithAlphaValue()
        {
            //Arrange
            const string ActionId = "TestActionId";
            const string ExpectedValue = "0";
            const string TestOperation = SetterOperations.Divide;

            var gameContext = this.GetGameContext();
            var gameState = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = "Level",
                    Value = "a",
                    Actions = new List<GameAction>()
                    {
                        new GameAction()
                        {
                            Id = ActionId,
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = "Level", Operation = TestOperation, Value = "15" }
                            }
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
        public void DoesRunActionPerformSetterWithMultiplyOperationWithNumerics()
        {
            //Arrange
            const string ActionId = "TestActionId";
            const string ExpectedValue = "15";
            const string TestOperation = SetterOperations.Multiply;

            var gameContext = this.GetGameContext();
            var gameState = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = "Level",
                    Value = "5",
                    Actions = new List<GameAction>()
                    {
                        new GameAction()
                        {
                            Id = ActionId,
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = "Level", Operation = TestOperation, Value = "3" }
                            }
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
        public void DoesRunActionPerformSetterWithMultiplyOperationWithAlphaSetter()
        {
            //Arrange
            const string ActionId = "TestActionId";
            const string ExpectedValue = "0";
            const string TestOperation = SetterOperations.Multiply;

            var gameContext = this.GetGameContext();
            var gameState = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = "Level",
                    Value = "5",
                    Actions = new List<GameAction>()
                    {
                        new GameAction()
                        {
                            Id = ActionId,
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = "Level", Operation = TestOperation, Value = "a" }
                            }
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
        public void DoesRunActionPerformSetterWithMultiplyOperationWithAlphaValue()
        {
            //Arrange
            const string ActionId = "TestActionId";
            const string ExpectedValue = "0";
            const string TestOperation = SetterOperations.Multiply;

            var gameContext = this.GetGameContext();
            var gameState = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = "Level",
                    Value = "a",
                    Actions = new List<GameAction>()
                    {
                        new GameAction()
                        {
                            Id = ActionId,
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = "Level", Operation = TestOperation, Value = "15" }
                            }
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
        public void DoesRunActionPerformSetterWithSubtractOperationWithNumerics()
        {
            //Arrange
            const string ActionId = "TestActionId";
            const string ExpectedValue = "2";
            const string TestOperation = SetterOperations.Subtract;

            var gameContext = this.GetGameContext();
            var gameState = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = "Level",
                    Value = "5",
                    Actions = new List<GameAction>()
                    {
                        new GameAction()
                        {
                            Id = ActionId,
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = "Level", Operation = TestOperation, Value = "3" }
                            }
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
        public void DoesRunActionPerformSetterWithSubtractOperationWithAlphaSetter()
        {
            //Arrange
            const string ActionId = "TestActionId";
            const string ExpectedValue = "5";
            const string TestOperation = SetterOperations.Subtract;

            var gameContext = this.GetGameContext();
            var gameState = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = "Level",
                    Value = "5",
                    Actions = new List<GameAction>()
                    {
                        new GameAction()
                        {
                            Id = ActionId,
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = "Level", Operation = TestOperation, Value = "a" }
                            }
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
        public void DoesRunActionPerformSetterWithSubtractOperationWithAlphaValue()
        {
            //Arrange
            const string ActionId = "TestActionId";
            const string ExpectedValue = "-5";
            const string TestOperation = SetterOperations.Subtract;

            var gameContext = this.GetGameContext();
            var gameState = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = "Level",
                    Value = "a",
                    Actions = new List<GameAction>()
                    {
                        new GameAction()
                        {
                            Id = ActionId,
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = "Level", Operation = TestOperation, Value = "5" }
                            }
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
        public void DoesRunActionFireTriggerWhenComparisonIsEqualWithNumerics()
        {
            //Arrange
            const string TestComparison = TargetComparisons.Equal;
            const string TriggerTarget = "Shield";
            const string ActionId = "TestActionId";
            const string ExpectedValue = "50";

            var gameContext = this.GetGameContext();
            var gameState = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = TriggerTarget,
                    Value = "0",
                    Actions = new List<GameAction>()
                    {
                        new GameAction()
                        {
                            Id = ActionId,
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = "Level", Operation = SetterOperations.Assign, Value = "10" }
                            }
                        }
                    },
                    Triggers = new List<GameTrigger>()
                    {
                        new GameTrigger()
                        {
                            TargetId = "Level",
                            Comparison = TestComparison,
                            Value = "10",
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = TriggerTarget, Operation = SetterOperations.Assign, Value = "50" }
                            }
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
        public void DoesRunActionFireTriggerWhenComparisonIsEqualWithAlphas()
        {
            //Arrange
            const string TestComparison = TargetComparisons.Equal;
            const string TriggerTarget = "Shield";
            const string ActionId = "TestActionId";
            const string ExpectedValue = "50";

            var gameContext = this.GetGameContext();
            var gameState = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = TriggerTarget,
                    Value = "0",
                    Actions = new List<GameAction>()
                    {
                        new GameAction()
                        {
                            Id = ActionId,
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = "Level", Operation = SetterOperations.Assign, Value = "abc" }
                            }
                        }
                    },
                    Triggers = new List<GameTrigger>()
                    {
                        new GameTrigger()
                        {
                            TargetId = "Level",
                            Comparison = TestComparison,
                            Value = "abc",
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = TriggerTarget, Operation = SetterOperations.Assign, Value = "50" }
                            }
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
        public void DoesRunActionFireTriggerWhenComparisonIsNotEqualWithNumerics()
        {
            //Arrange
            const string TestComparison = TargetComparisons.NotEqual;
            const string TriggerTarget = "Shield";
            const string ActionId = "TestActionId";
            const string ExpectedValue = "50";

            var gameContext = this.GetGameContext();
            var gameState = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = TriggerTarget,
                    Value = "0",
                    Actions = new List<GameAction>()
                    {
                        new GameAction()
                        {
                            Id = ActionId,
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = "Level", Operation = SetterOperations.Assign, Value = "9" }
                            }
                        }
                    },
                    Triggers = new List<GameTrigger>()
                    {
                        new GameTrigger()
                        {
                            TargetId = "Level",
                            Comparison = TestComparison,
                            Value = "10",
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = TriggerTarget, Operation = SetterOperations.Assign, Value = "50" }
                            }
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
        public void DoesRunActionFireTriggerWhenComparisonIsNotEqualWithAlphas()
        {
            //Arrange
            const string TestComparison = TargetComparisons.NotEqual;
            const string TriggerTarget = "Shield";
            const string ActionId = "TestActionId";
            const string ExpectedValue = "50";

            var gameContext = this.GetGameContext();
            var gameState = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = TriggerTarget,
                    Value = "0",
                    Actions = new List<GameAction>()
                    {
                        new GameAction()
                        {
                            Id = ActionId,
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = "Level", Operation = SetterOperations.Assign, Value = "abc" }
                            }
                        }
                    },
                    Triggers = new List<GameTrigger>()
                    {
                        new GameTrigger()
                        {
                            TargetId = "Level",
                            Comparison = TestComparison,
                            Value = "cba",
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = TriggerTarget, Operation = SetterOperations.Assign, Value = "50" }
                            }
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
        public void DoesRunActionFireTriggerWhenComparisonIsGreaterThanWithNumerics()
        {
            //Arrange
            const string TestComparison = TargetComparisons.GreaterThan;
            const string TriggerTarget = "Shield";
            const string ActionId = "TestActionId";
            const string ExpectedValue = "50";

            var gameContext = this.GetGameContext();
            var gameState = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = TriggerTarget,
                    Value = "0",
                    Actions = new List<GameAction>()
                    {
                        new GameAction()
                        {
                            Id = ActionId,
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = "Level", Operation = SetterOperations.Assign, Value = "11" }
                            }
                        }
                    },
                    Triggers = new List<GameTrigger>()
                    {
                        new GameTrigger()
                        {
                            TargetId = "Level",
                            Comparison = TestComparison,
                            Value = "10",
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = TriggerTarget, Operation = SetterOperations.Assign, Value = "50" }
                            }
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
        public void DoesRunActionFireTriggerWhenComparisonIsGreaterThanWithAlphaTrigger()
        {
            //Arrange
            const string TestComparison = TargetComparisons.GreaterThan;
            const string TriggerTarget = "Shield";
            const string ActionId = "TestActionId";
            const string ExpectedValue = "50";

            var gameContext = this.GetGameContext();
            var gameState = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = TriggerTarget,
                    Value = "0",
                    Actions = new List<GameAction>()
                    {
                        new GameAction()
                        {
                            Id = ActionId,
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = "Level", Operation = SetterOperations.Assign, Value = "11" }
                            }
                        }
                    },
                    Triggers = new List<GameTrigger>()
                    {
                        new GameTrigger()
                        {
                            TargetId = "Level",
                            Comparison = TestComparison,
                            Value = "abc",
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = TriggerTarget, Operation = SetterOperations.Assign, Value = "50" }
                            }
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
        public void DoesRunActionFireTriggerWhenComparisonIsGreaterThanWithAlphaValue()
        {
            //Arrange
            const string TestComparison = TargetComparisons.GreaterThan;
            const string TriggerTarget = "Shield";
            const string ActionId = "TestActionId";
            const string ExpectedValue = "50";

            var gameContext = this.GetGameContext();
            var gameState = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = TriggerTarget,
                    Value = "abc",
                    Actions = new List<GameAction>()
                    {
                        new GameAction()
                        {
                            Id = ActionId,
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = "Level", Operation = SetterOperations.Assign, Value = "11" }
                            }
                        }
                    },
                    Triggers = new List<GameTrigger>()
                    {
                        new GameTrigger()
                        {
                            TargetId = "Level",
                            Comparison = TestComparison,
                            Value = "10",
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = TriggerTarget, Operation = SetterOperations.Assign, Value = "50" }
                            }
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
        public void DoesRunActionFireTriggerWhenComparisonIsGreaterThanOrEqualWithNumerics()
        {
            //Arrange
            const string TestComparison = TargetComparisons.GreaterThanOrEqual;
            const string TriggerTarget = "Shield";
            const string ActionId = "TestActionId";
            const string ExpectedValue = "50";

            var gameContext = this.GetGameContext();
            var gameState = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = TriggerTarget,
                    Value = "0",
                    Actions = new List<GameAction>()
                    {
                        new GameAction()
                        {
                            Id = ActionId,
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = "Level", Operation = SetterOperations.Assign, Value = "10" }
                            }
                        }
                    },
                    Triggers = new List<GameTrigger>()
                    {
                        new GameTrigger()
                        {
                            TargetId = "Level",
                            Comparison = TestComparison,
                            Value = "10",
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = TriggerTarget, Operation = SetterOperations.Assign, Value = "50" }
                            }
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
        public void DoesRunActionFireTriggerWhenComparisonIsGreaterThanOrEqualWithAlphaTrigger()
        {
            //Arrange
            const string TestComparison = TargetComparisons.GreaterThanOrEqual;
            const string TriggerTarget = "Shield";
            const string ActionId = "TestActionId";
            const string ExpectedValue = "50";

            var gameContext = this.GetGameContext();
            var gameState = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = TriggerTarget,
                    Value = "0",
                    Actions = new List<GameAction>()
                    {
                        new GameAction()
                        {
                            Id = ActionId,
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = "Level", Operation = SetterOperations.Assign, Value = "10" }
                            }
                        }
                    },
                    Triggers = new List<GameTrigger>()
                    {
                        new GameTrigger()
                        {
                            TargetId = "Level",
                            Comparison = TestComparison,
                            Value = "abc",
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = TriggerTarget, Operation = SetterOperations.Assign, Value = "50" }
                            }
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
        public void DoesRunActionFireTriggerWhenComparisonIsGreaterThanOrEqualWithAlphaValue()
        {
            //Arrange
            const string TestComparison = TargetComparisons.GreaterThanOrEqual;
            const string TriggerTarget = "Shield";
            const string ActionId = "TestActionId";
            const string ExpectedValue = "50";

            var gameContext = this.GetGameContext();
            var gameState = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = TriggerTarget,
                    Value = "abc",
                    Actions = new List<GameAction>()
                    {
                        new GameAction()
                        {
                            Id = ActionId,
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = "Level", Operation = SetterOperations.Assign, Value = "10" }
                            }
                        }
                    },
                    Triggers = new List<GameTrigger>()
                    {
                        new GameTrigger()
                        {
                            TargetId = "Level",
                            Comparison = TestComparison,
                            Value = "10",
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = TriggerTarget, Operation = SetterOperations.Assign, Value = "50" }
                            }
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
        public void DoesRunActionFireTriggerWhenComparisonIsLessThanWithNumerics()
        {
            //Arrange
            const string TestComparison = TargetComparisons.LessThan;
            const string TriggerTarget = "Shield";
            const string ActionId = "TestActionId";
            const string ExpectedValue = "50";

            var gameContext = this.GetGameContext();
            var gameState = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = TriggerTarget,
                    Value = "0",
                    Actions = new List<GameAction>()
                    {
                        new GameAction()
                        {
                            Id = ActionId,
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = "Level", Operation = SetterOperations.Assign, Value = "9" }
                            }
                        }
                    },
                    Triggers = new List<GameTrigger>()
                    {
                        new GameTrigger()
                        {
                            TargetId = "Level",
                            Comparison = TestComparison,
                            Value = "10",
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = TriggerTarget, Operation = SetterOperations.Assign, Value = "50" }
                            }
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
        public void DoesRunActionFireTriggerWhenComparisonIsLessThanWithAlphaTrigger()
        {
            //Arrange
            const string TestComparison = TargetComparisons.LessThan;
            const string TriggerTarget = "Shield";
            const string ActionId = "TestActionId";
            const string ExpectedValue = "0";

            var gameContext = this.GetGameContext();
            var gameState = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = TriggerTarget,
                    Value = "0",
                    Actions = new List<GameAction>()
                    {
                        new GameAction()
                        {
                            Id = ActionId,
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = "Level", Operation = SetterOperations.Assign, Value = "9" }
                            }
                        }
                    },
                    Triggers = new List<GameTrigger>()
                    {
                        new GameTrigger()
                        {
                            TargetId = "Level",
                            Comparison = TestComparison,
                            Value = "abc",
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = TriggerTarget, Operation = SetterOperations.Assign, Value = "50" }
                            }
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
        public void DoesRunActionFireTriggerWhenComparisonIsLessThanWithAlphaValue()
        {
            //Arrange
            const string TestComparison = TargetComparisons.LessThan;
            const string TriggerTarget = "Shield";
            const string ActionId = "TestActionId";
            const string ExpectedValue = "50";

            var gameContext = this.GetGameContext();
            var gameState = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = TriggerTarget,
                    Value = "abc",
                    Actions = new List<GameAction>()
                    {
                        new GameAction()
                        {
                            Id = ActionId,
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = "Level", Operation = SetterOperations.Assign, Value = "9" }
                            }
                        }
                    },
                    Triggers = new List<GameTrigger>()
                    {
                        new GameTrigger()
                        {
                            TargetId = "Level",
                            Comparison = TestComparison,
                            Value = "10",
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = TriggerTarget, Operation = SetterOperations.Assign, Value = "50" }
                            }
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
        public void DoesRunActionFireTriggerWhenComparisonIsLessThanOrEqualWithNumerics()
        {
            //Arrange
            const string TestComparison = TargetComparisons.LessThanOrEqual;
            const string TriggerTarget = "Shield";
            const string ActionId = "TestActionId";
            const string ExpectedValue = "50";

            var gameContext = this.GetGameContext();
            var gameState = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = TriggerTarget,
                    Value = "0",
                    Actions = new List<GameAction>()
                    {
                        new GameAction()
                        {
                            Id = ActionId,
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = "Level", Operation = SetterOperations.Assign, Value = "10" }
                            }
                        }
                    },
                    Triggers = new List<GameTrigger>()
                    {
                        new GameTrigger()
                        {
                            TargetId = "Level",
                            Comparison = TestComparison,
                            Value = "10",
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = TriggerTarget, Operation = SetterOperations.Assign, Value = "50" }
                            }
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
        public void DoesRunActionFireTriggerWhenComparisonIsLessThanOrEqualWithAlphaTrigger()
        {
            //Arrange
            const string TestComparison = TargetComparisons.LessThanOrEqual;
            const string TriggerTarget = "Shield";
            const string ActionId = "TestActionId";
            const string ExpectedValue = "0";

            var gameContext = this.GetGameContext();
            var gameState = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = TriggerTarget,
                    Value = "0",
                    Actions = new List<GameAction>()
                    {
                        new GameAction()
                        {
                            Id = ActionId,
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = "Level", Operation = SetterOperations.Assign, Value = "10" }
                            }
                        }
                    },
                    Triggers = new List<GameTrigger>()
                    {
                        new GameTrigger()
                        {
                            TargetId = "Level",
                            Comparison = TestComparison,
                            Value = "abc",
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = TriggerTarget, Operation = SetterOperations.Assign, Value = "50" }
                            }
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
        public void DoesRunActionFireTriggerWhenComparisonIsLessThanOrEqualWithAlphaValue()
        {
            //Arrange
            const string TestComparison = TargetComparisons.LessThanOrEqual;
            const string TriggerTarget = "Shield";
            const string ActionId = "TestActionId";
            const string ExpectedValue = "50";

            var gameContext = this.GetGameContext();
            var gameState = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = TriggerTarget,
                    Value = "abc",
                    Actions = new List<GameAction>()
                    {
                        new GameAction()
                        {
                            Id = ActionId,
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = "Level", Operation = SetterOperations.Assign, Value = "10" }
                            }
                        }
                    },
                    Triggers = new List<GameTrigger>()
                    {
                        new GameTrigger()
                        {
                            TargetId = "Level",
                            Comparison = TestComparison,
                            Value = "10",
                            Setters = new List<GameSetter>()
                            {
                                new GameSetter() { TargetId = TriggerTarget, Operation = SetterOperations.Assign, Value = "50" }
                            }
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
