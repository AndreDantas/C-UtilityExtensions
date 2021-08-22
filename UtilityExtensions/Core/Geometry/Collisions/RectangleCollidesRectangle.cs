using UtilityExtensions.Core.Geometry.Interfaces;

namespace UtilityExtensions.Core.Geometry.Collision
{
    public class RectangleCollidesRectangle : IShapesCollision<Rectangle, Rectangle>
    {
        public bool CheckCollision(Rectangle rect1, Rectangle rect2)
        {
            return GeometryExtensions.IsShapes2DIntersecting(rect1, rect2);
        }
    }
}