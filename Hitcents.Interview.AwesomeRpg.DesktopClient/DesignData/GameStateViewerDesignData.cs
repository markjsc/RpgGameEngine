using System.Collections.Generic;
using Hitcents.Interview.AwesomeRpg.Contracts.Models;

namespace Hitcents.Interview.AwesomeRpg.DesktopClient.DesignData
{
    /// <summary>
    /// This is some hard-coded sample data that is used to display in the GameStateViewer designer.
    /// </summary>
    public class GameStateViewerDesignData
    {
        private readonly List<GameElement> _gameState;

        public GameStateViewerDesignData()
        {
            this._gameState = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = "CoolGame",
                    Elements = new List<GameElement>()
                    {
                        new GameElement()
                        {
                            Id = "Player",
                            Actions = new List<GameAction>()
                            {
                                new GameAction()
                                {
                                    Id = "GainXP",
                                    Setters = new List<GameSetter>()
                                    {
                                        new GameSetter()
                                        {
                                            TargetId = "XP",
                                            Operation = "+",
                                            Value = "10"
                                        }
                                    }
                                },
                                new GameAction()
                                {
                                    Id = "ClearHP",
                                    Setters = new List<GameSetter>()
                                    {
                                        new GameSetter()
                                        {
                                            TargetId = "HP",
                                            Operation = "=",
                                            Value = "0"
                                        },
                                        new GameSetter()
                                        {
                                            TargetId = "XP",
                                            Operation = "-",
                                            Value = "5"
                                        }
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
                                        new GameSetter()
                                        {
                                            TargetId = "Level",
                                            Operation = "=",
                                            Value = "2"
                                        },
                                        new GameSetter()
                                        {
                                            TargetId = "HP",
                                            Operation = "+",
                                            Value = "5"
                                        }
                                    }
                                },
                                new GameTrigger()
                                {
                                    TargetId = "Name",
                                    Comparison = "=",
                                    Value = "Super Bob",
                                    Setters = new List<GameSetter>()
                                    {
                                        new GameSetter()
                                        {
                                            TargetId = "Mode",
                                            Operation = "=",
                                            Value = "Superman Mode!"
                                        }
                                    }
                                }
                            },
                            Elements = new List<GameElement>()
                            {
                                new GameElement() { Id = "Level", Value = "1" },
                                new GameElement() { Id = "XP", Value = "0"},
                                new GameElement() { Id = "HP", Value = "50" },
                                new GameElement() { Id = "Mode", Value = null }
                            }
                        }
                    }
                }
            };
        }

        public List<GameElement> GameState
        {
            get
            {
                return this._gameState;
            }
        }

    }
}
