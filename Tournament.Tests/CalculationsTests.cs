using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Domain.Calculation;

namespace Tournament.Tests
{
    public class CalculationsTests
    {
        [Fact]
        public void GetCountOfRounds_WithTwoPlayers_ReturnsOne()
        {
            // Arrange
            int playerCount = 2;

            // Act
            int result = Calculations.GetCountOfRounds(playerCount);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void GetCountOfRounds_WithFourPlayers_ReturnsTwo()
        {
            // Arrange
            int playerCount = 4;

            // Act
            int result = Calculations.GetCountOfRounds(playerCount);

            // Assert
            Assert.Equal(2, result);
        }

        [Fact]
        public void GetCountOfRounds_WithEightPlayers_ReturnsThree()
        {
            // Arrange
            int playerCount = 8;

            // Act
            int result = Calculations.GetCountOfRounds(playerCount);

            // Assert
            Assert.Equal(3, result);
        }

        [Fact]
        public void SortSeeds_WithFourSeeds_ReturnsCorrectOrder()
        {
            // Arrange
            int seedCount = 4;

            // Act
            List<int> result = Calculations.SortSeeds(seedCount);

            // Assert
            Assert.Equal(new List<int> { 1, 4, 3, 2 }, result);
        }

        [Fact]
        public void SortSeeds_WithEightSeeds_ReturnsCorrectOrder()
        {
            // Arrange
            int seedCount = 8;

            // Act
            List<int> result = Calculations.SortSeeds(seedCount);

            // Assert
            Assert.Equal(new List<int> { 1, 8, 4, 5, 6, 3, 7, 2 }, result);
        }

        [Fact]
        public void DisplayPlayerAgeGroup_WithChildBirthDate_ReturnsU11()
        {
            // Arrange
            DateTime birthDate = new DateTime(2013, 1, 1);

            // Act
            string result = Calculations.DisplayPlayerAgeGroup(birthDate);

            // Assert
            Assert.Equal("U11", result);
        }

        [Fact]
        public void DisplayPlayerAgeGroup_WithTeenBirthDate_ReturnsU19()
        {
            // Arrange
            DateTime birthDate = new DateTime(2005, 1, 1);

            // Act
            string result = Calculations.DisplayPlayerAgeGroup(birthDate);

            // Assert
            Assert.Equal("U19", result);
        }

        [Fact]
        public void DisplayPlayerAgeGroup_WithAdultBirthDate_ReturnsAdult()
        {
            // Arrange
            DateTime birthDate = new DateTime(1996, 1, 1);

            // Act
            string result = Calculations.DisplayPlayerAgeGroup(birthDate);

            // Assert
            Assert.Equal("Adult", result);
        }

        [Fact]
        public void DisplayPlayerAgeGroup_WithVeteranBirthDate_ReturnsVeteran()
        {
            // Arrange
            DateTime birthDate = new DateTime(1975, 1, 1);

            // Act
            string result = Calculations.DisplayPlayerAgeGroup(birthDate);

            // Assert
            Assert.Equal("Veteran", result);
        }
    }
}