using UnityEngine;

public static class Consts2D
{
    ///<summary>计算一个点是否在一个多边形内</summary>
    ///<param name="point">点的坐标</param>
    ///<param name="polygon">多边形的各个顶点坐标</param>
    ///<returns>如果在多边形内返回True，否则返回False</returns>
    public static bool InnerGraphByAngle(in Vector2 point, params Vector2[] polygon)
    {
        // 思路：从给定点绘制一条射线，然后计算该射线与多边形边界的交点数量
        var intersectCount = 0;
        var vertexCount = polygon.Length;

        for (int i = 0, j = vertexCount - 1; i < vertexCount; j = i++)
            if (polygon[i].y > point.y != polygon[j].y > point.y &&
                point.x < (polygon[j].x - polygon[i].x) * (point.y - polygon[i].y) / (polygon[j].y - polygon[i].y) +
                polygon[i].x)
                intersectCount++;

        return intersectCount % 2 == 1;
    }

    /// <summary>计算坐标系中的任意两条线段是否相交</summary>
    /// <param name="a">第一条线段的一个端点坐标</param>
    /// <param name="b">第一条线段的另一个端点坐标</param>
    /// <param name="c">第二条线段的一个端点坐标</param>
    /// <param name="d">第二条线段的另一个端点坐标</param>
    /// <param name="crossPoint">输出交点的坐标，如果没有交点输出(0,0)</param>
    /// <returns>如果有交点返回True，否则返回False</returns>
    public static bool HasCrossPoint(Vector2 a, Vector2 b, Vector2 c, Vector2 d, out Vector2 crossPoint)
    {
        crossPoint = Vector2.zero;
        double denominator = (b.y - a.y) * (d.x - c.x) - (a.x - b.x) * (c.y - d.y);
        if (denominator == 0) return false;
        var x = ((b.x - a.x) * (d.x - c.x) * (c.y - a.y)
                 + (b.y - a.y) * (d.x - c.x) * a.x
                 - (d.y - c.y) * (b.x - a.x) * c.x) / denominator;
        var y = -((b.y - a.y) * (d.y - c.y) * (c.x - a.x)
                  + (b.x - a.x) * (d.y - c.y) * a.y
                  - (d.x - c.x) * (b.y - a.y) * c.y) / denominator;
        if ((x - a.x) * (x - b.x) <= 0 && (y - a.y) * (y - b.y) <= 0
                                       && (x - c.x) * (x - d.x) <= 0 && (y - c.y) * (y - d.y) <= 0)
        {
            crossPoint = new Vector2((float)x, (float)y);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 判断一个点是否在某条线段（直线）的左侧
    /// </summary>
    /// <param name="point">点的坐标</param>
    /// <param name="linePA">线段起点</param>
    /// <param name="linePB">线段终点</param>
    public static bool IsPointOnLeftSideOfLine(in Vector2 point, in Vector2 linePA, in Vector2 linePB)
    {
        // 计算直线上的向量
        Vector2 lineVector = linePB - linePA;
        // 计算起始点到点的向量
        Vector2 pointVector = point - linePA;
        // 使用叉乘判断点是否在直线的左侧
        var crossProduct = lineVector.x * pointVector.y - lineVector.y * pointVector.x;
        // 如果叉乘结果大于0，则点在直线的左侧
        return crossProduct > 0;
    }
}
