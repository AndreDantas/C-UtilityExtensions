namespace UtilityExtensions.Core.Geometry.Interfaces
{
    public interface IShapesCollision<T1, T2> where T1 : Shape2D where T2 : Shape2D
    {
        bool CheckCollision(T1 shape1, T2 shape2);
    }
}