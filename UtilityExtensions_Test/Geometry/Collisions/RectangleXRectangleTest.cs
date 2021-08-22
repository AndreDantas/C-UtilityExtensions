using NUnit.Framework;
using UtilityExtensions.Core.Geometry;
using UtilityExtensions.Core.Geometry.Collision;

namespace UtilityExtensions_Test.Geometry.Collisions
{
    public class RectangleXRectangleTest
    {
        private RectangleCollidesRectangle col;

        [SetUp]
        public void Setup()
        {
            col = new RectangleCollidesRectangle();
        }

        [Test]
        public void RectangleXRectangleCollision_NotIntersectingAndZeroRotation_ReturnsFalse()
        {
            Rectangle rect1 = new Rectangle { Width = 1, Height = 2, Center = new Vector2() };
            Rectangle rect2 = new Rectangle { Width = 1, Height = 2, Center = new Vector2(3, 4) };
            Assert.IsFalse(col.CheckCollision(rect1, rect2));
        }

        [Test]
        public void RectangleXRectangleCollision_IntersectingAndZeroRotation_ReturnsTrue()
        {
            Rectangle rect1 = new Rectangle { Width = 1, Height = 2, Center = new Vector2() };
            Rectangle rect2 = new Rectangle { Width = 2, Height = 1, Center = new Vector2(1, 0) };
            Assert.IsTrue(col.CheckCollision(rect1, rect2));
        }

        [Test]
        public void RectangleXRectangleCollision_NotIntersectingWithRotation_ReturnsFalse()
        {
            Rectangle rect1 = new Rectangle { Width = 1, Height = 1, Center = new Vector2() };
            Rectangle rect2 = new Rectangle { Width = 7, Height = 1, Rotation = 45, Center = new Vector2(0, 3) };
            Assert.IsFalse(col.CheckCollision(rect1, rect2));
        }

        [Test]
        public void RectangleXRectangleCollision_IntersectingWithRotation_ReturnsTrue()
        {
            Rectangle rect1 = new Rectangle { Width = 7, Height = 1, Center = new Vector2() };
            Rectangle rect2 = new Rectangle { Width = 7, Height = 1, Rotation = 45, Center = new Vector2(0, 3) };
            Assert.IsTrue(col.CheckCollision(rect1, rect2));
        }
    }
}