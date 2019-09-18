using NUnit.Framework;

namespace FunctionalCore.Tests
{
    public class Tests
    {
        [Test]
        public void TwoUnionsWithTheSameTypeAndCaseAndValueShouldBeEqual() 
            => Assert.AreEqual(
                Union2<int, string>.Case1(1),
                Union2<int, string>.Case1(1));

        [Test]
        public void TwoUnionsWithTheSameTypeAndCaseButDifferentValueShouldNotBeEqual() 
            => Assert.AreNotEqual(
                Union2<int, string>.Case1(1),
                Union2<int, string>.Case1(2));

        [Test]
        public void TwoUnionCasesWithTheDifferentTypesShouldNotBeEqual() 
            => Assert.AreNotEqual(
                Union2<int, string>.Case1(1),
                Union2<string, int>.Case2(1));
    }
}