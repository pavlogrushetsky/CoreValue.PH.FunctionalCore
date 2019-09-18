using NUnit.Framework;

namespace FunctionalCore.Tests
{
    public class DiscriminatedUnionsTests
    {
        [Test]
        public void Union2_TwoUnionsWithTheSameTypeAndCaseAndValueShouldBeEqual() 
            => Assert.AreEqual(
                Union2<int, string>.Case1(1),
                Union2<int, string>.Case1(1));

        [Test]
        public void Union2_TwoUnionsWithTheSameTypeAndCaseButDifferentValueShouldNotBeEqual() 
            => Assert.AreNotEqual(
                Union2<int, string>.Case1(1),
                Union2<int, string>.Case1(2));

        [Test]
        public void Union2_TwoUnionsWithTheSameTypeButDifferentCaseShouldNotBeEqual()
            => Assert.AreNotEqual(
                Union2<int, string>.Case1(1),
                Union2<int, string>.Case2("1"));

        [Test]
        public void Union2_TwoUnionsWithTheDifferentTypesShouldNotBeEqual() 
            => Assert.AreNotEqual(
                Union2<int, string>.Case1(1),
                Union2<string, int>.Case2(1));

        [Test]
        public void Union2_MatchShouldInvokeTheFunctionForTheFirstCase()
            => Assert.IsTrue(
                Union2<int, string>.Case1(1)
                    .Match(
                        case1 => true, 
                        case2 => false));

        [Test]
        public void Union2_MatchShouldInvokeTheFunctionForTheSecondCase()
            => Assert.IsTrue(
                Union2<int, string>.Case2("1")
                    .Match(
                        case1 => false,
                        case2 => true));

        [Test]
        public void Union3_TwoUnionsWithTheSameTypeAndCaseAndValueShouldBeEqual()
            => Assert.AreEqual(
                Union3<int, string, bool>.Case1(1),
                Union3<int, string, bool>.Case1(1));

        [Test]
        public void Union3_TwoUnionsWithTheSameTypeAndCaseButDifferentValueShouldNotBeEqual()
            => Assert.AreNotEqual(
                Union3<int, string, bool>.Case1(1),
                Union3<int, string, bool>.Case1(2));

        [Test]
        public void Union3_TwoUnionsWithTheSameTypeButDifferentCaseShouldNotBeEqual()
            => Assert.AreNotEqual(
                Union3<int, string, bool>.Case1(1),
                Union3<int, string, bool>.Case2("1"));

        [Test]
        public void Union3_TwoUnionsWithTheDifferentTypesShouldNotBeEqual()
            => Assert.AreNotEqual(
                Union3<int, string, bool>.Case1(1),
                Union3<string, bool, int>.Case2(true));

        [Test]
        public void Union3_MatchShouldInvokeTheFunctionForTheFirstCase()
            => Assert.IsTrue(
                Union3<int, string, bool>.Case1(1)
                    .Match(
                        case1 => true,
                        case2 => false,
                        case3 => false));

        [Test]
        public void Union3_MatchShouldInvokeTheFunctionForTheSecondCase()
            => Assert.IsTrue(
                Union3<int, string, bool>.Case2("1")
                    .Match(
                        case1 => false,
                        case2 => true,
                        case3 => false));

        [Test]
        public void Union3_MatchShouldInvokeTheFunctionForTheThirdCase()
            => Assert.IsTrue(
                Union3<int, string, bool>.Case3(false)
                    .Match(
                        case1 => false,
                        case2 => false,
                        case3 => true));
    }
}