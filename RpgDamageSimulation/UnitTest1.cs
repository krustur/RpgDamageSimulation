using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RpgDamageSimulation
{
    public class MyAssert
    {
        public static void AreCloseTo(double expected, double actual)
        {
            var maxDiff = expected * 0.02f;
            if (actual < expected - maxDiff)
            {
                throw new AssertFailedException($"Not Close to {expected}. Actual {actual}");
            }
            if (actual > expected + maxDiff)
            {
                throw new AssertFailedException($"Not Close to {expected}. Actual {actual}");
            }
        }
    }

    public enum Dice
    {
        D6 = 6,
        D100 = 100,
    }

    [TestClass]
    public class UnitTest1
    {
        private Random _random;

        [TestMethod]
        public void TestMethod1()
        {
            const int numberOfSamples = 500000;

            MyAssert.AreCloseTo(4.9, Simulate(numberOfSamples, 70, Dice.D100, 2, Dice.D6));
            MyAssert.AreCloseTo(6.3, Simulate(numberOfSamples, 60, Dice.D100, 3, Dice.D6));
            MyAssert.AreCloseTo(7, Simulate(numberOfSamples, 50, Dice.D100, 4, Dice.D6));
            MyAssert.AreCloseTo(7, Simulate(numberOfSamples, 40, Dice.D100, 5, Dice.D6));
            MyAssert.AreCloseTo(6.3, Simulate(numberOfSamples, 30, Dice.D100, 6, Dice.D6));
            MyAssert.AreCloseTo(4.9, Simulate(numberOfSamples, 20, Dice.D100, 7, Dice.D6));
            MyAssert.AreCloseTo(2.8, Simulate(numberOfSamples, 10, Dice.D100, 8, Dice.D6));
        }

        private double Simulate(int numberOfSamples, int skillDiceSuccessSideCount, Dice skillDice,
            int numberOfDamageDice, Dice damageDiceSideCount)
        {
            _random = new Random(DateTime.Now.Millisecond);

            var totalDamage = 0;

            for (var i = 0; i < numberOfSamples; i++)
            {
                var skillSuccessRoll = RollSuccessDice(skillDiceSuccessSideCount, skillDice);
                if (skillSuccessRoll)
                {
                    var damageRoll = RollDamageDice(numberOfDamageDice, damageDiceSideCount);
                    totalDamage += damageRoll;
                }
            }

            return (double)totalDamage / numberOfSamples;
        }

        private int RollDamageDice(int numberOfDamageDice, Dice damageDice)
        {
            int damage = 0;
            for (var i = 0; i < numberOfDamageDice; i++)
            {
                var damageRoll = _random.Next((int)damageDice) + 1;
                damage += damageRoll;
            }
            return damage;
        }

        private bool RollSuccessDice(int skillDiceSuccessSideCount, Dice skillDice)
        {
            var roll = _random.Next((int)skillDice);
            return roll < skillDiceSuccessSideCount;
        }
    }
}
