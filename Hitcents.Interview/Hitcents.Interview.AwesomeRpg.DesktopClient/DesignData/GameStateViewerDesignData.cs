using System.Collections.Generic;
using Hitcents.Interview.AwesomeRpg.Contracts.Models;

namespace Hitcents.Interview.AwesomeRpg.DesktopClient.DesignData
{
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
                            Action = new GameAction()
                            {
                                Id = "GainXP",
                                Setters = new List<GameSetter>()
                                {
                                    new GameSetter()
                                    {
                                        TargetId = "XP",
                                        Operation = "+",
                                        Value = 10
                                    }
                                }
                            },
                            Trigger = new GameTrigger()
                            {
                                TargetId = "XP",
                                Comparison = ">=",
                                Value = 10,
                                Setters = new List<GameSetter>()
                                {
                                    new GameSetter()
                                    {
                                        TargetId = "Level",
                                        Operation = "=",
                                        Value = 2
                                    },
                                    new GameSetter()
                                    {
                                        TargetId = "HP",
                                        Operation = "+",
                                        Value = 5
                                    }
                                }
                            },
                            Elements = new List<GameElement>()
                            {
                                new GameElement() { Id = "Level", Value = 1 },
                                new GameElement() { Id = "XP", Value = 0 },
                                new GameElement() { Id = "HP", Value = 50 }
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
