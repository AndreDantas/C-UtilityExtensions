using NUnit.Framework;
using UtilityExtensions.Core;
using UtilityExtensions.Extensions;

namespace UtilityExtensions_Test
{
    public class NumericTest
    {
        [Test]
        public void NumericTest_Constructor()
        {
            Numeric n = 3.932f;

            Assert.IsTrue(n.Long == 3);
            Assert.IsTrue(n.Int == 3);
            Assert.IsTrue(n.Double.CloseTo(3.932));
            Assert.IsTrue(n.Float.CloseTo(3.932f));
            Assert.IsTrue(n.WithPrecision(3).Decimal == 3.932m);
            Assert.IsTrue(n.WithPrecision(2).Decimal == 3.93m);
        }

        [Test]
        public void NumericTest_Add()
        {
            Numeric n1 = 3.932;
            Numeric n2 = -2f;
            Numeric n = n1 + n2;

            Assert.IsTrue(n.Long == 1);
            Assert.IsTrue(n.Int == 1);
            Assert.IsTrue(n.Double.CloseTo(1.932));
            Assert.IsTrue(n.Float.CloseTo(1.932f));
            Assert.IsTrue(n.Decimal == 1.932m);
        }

        [Test]
        public void NumericTest_Sub()
        {
            Numeric n1 = 3.932;
            Numeric n2 = 2f;
            Numeric n = n1 - n2;

            Assert.IsTrue(n.Long == 1);
            Assert.IsTrue(n.Int == 1);
            Assert.IsTrue(n.Double.CloseTo(1.932));
            Assert.IsTrue(n.Float.CloseTo(1.932f));
            Assert.IsTrue(n.Decimal == 1.932m);
        }

        [Test]
        public void NumericTest_Mult()
        {
            Numeric n1 = 3.932;
            Numeric n2 = 2f;
            Numeric n = n1 * n2;

            Assert.IsTrue(n.Long == 7);
            Assert.IsTrue(n.Int == 7);
            Assert.IsTrue(n.Double.CloseTo(7.864));
            Assert.IsTrue(n.Float.CloseTo(7.864f));
            Assert.IsTrue(n.Decimal == 7.864m);
        }

        [Test]
        public void NumericTest_Div()
        {
            Numeric n1 = 7.864;
            Numeric n2 = 2f;
            Numeric n = n1 / n2;

            Assert.IsTrue(n.Long == 3);
            Assert.IsTrue(n.Int == 3);
            Assert.IsTrue(n.Double.CloseTo(3.932));
            Assert.IsTrue(n.Float.CloseTo(3.932f));
            Assert.IsTrue(n.Decimal == 3.932m);
        }
    }
}