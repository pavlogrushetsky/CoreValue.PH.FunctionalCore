using NUnit.Framework;

namespace FunctionalCore.Tests
{
    public class DiscriminatedUnionsTests
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
        public void TwoUnionsWithTheSameTypeButDifferentCaseShouldNotBeEqual()
            => Assert.AreNotEqual(
                Union2<int, string>.Case1(1),
                Union2<int, string>.Case2("1"));

        [Test]
        public void TwoUnionsWithTheDifferentTypesShouldNotBeEqual() 
            => Assert.AreNotEqual(
                Union2<int, string>.Case1(1),
                Union2<string, int>.Case2(1));

        [Test]
        public void MatchShouldInvokeTheFunctionForTheFirstCase()
            => Assert.IsTrue(
                Union2<int, string>.Case1(1)
                    .Match(
                        case1 => true, 
                        case2 => false));

        [Test]
        public void MatchShouldInvokeTheFunctionForTheSecondCase()
            => Assert.IsTrue(
                Union2<int, string>.Case2("1")
                    .Match(
                        case1 => false,
                        case2 => true));
    }
}