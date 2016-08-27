using System;
using System.Collections.Generic;
using Hitcents.Interview.AwesomeRpg.Contracts.Models;
using Hitcents.Interview.AwesomeRpg.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hitcents.Interview.AwesomeRpg.Tests.Engine
{
    [TestClass]
    public class GameContextTests
    {
        [TestMethod, ExpectedException(typeof(Exception))]
        public void DoesBuildGameContextThrowExceptionWhenDuplicateElementIdInSameLevelIsAdded()
        {
            //Arrange
            var gameContext = new GameContext();

            var elements = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = "abc"
                },
                new GameElement()
                {
                    Id = "abc"
                }
            };

            //Act
            gameContext.BuildGameContext(elements);

            //Assert - expected exception
        }

        [TestMethod, ExpectedException(typeof(Exception))]
        public void DoesBuildGameContextThrowExceptionWhenDuplicateElementIdInDifferentLevelIsAdded()
        {
            //Arrange
            var gameContext = new GameContext();

            var elements = new List<GameElement>()
            {
                new GameElement()
                {
                    Id = "abc",
                    Elements = new List<GameElement>()
                    {
                        new GameElement() { Id = "abc" }
                    }
                },
                
            };

            //Act
            gameContext.BuildGameContext(elements);

            //Assert - expected exception
        }
    }
}
