using System.Collections.Generic;
using UnityEngine;

public class MeshGen : MonoBehaviour
{
    public float radius = 1f; // 正八边形的半径
    public List<Vector3> verts;
    public List<int> tris;
    public List<Vector2> uvs;
    private Mesh m_Mesh;
    private MeshFilter m_MeshFilter;

    public void CreateMesh()
    {
        if (!m_MeshFilter) m_MeshFilter = GetComponent<MeshFilter>();
        m_Mesh = new Mesh
        {
            name = "Octagon Mesh"
        };

        // 生成正八边形的顶点
        verts = new List<Vector3>();
        uvs = new List<Vector2>();
        int numSides = 8; // 八边形有8条边
        float angleIncrement = 360f / numSides; // 每个顶点之间的角度增量

        for (int i = 0; i < numSides; i++)
        {
            float angle = i * angleIncrement * Mathf.Deg2Rad; // 将角度转换为弧度
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            verts.Add(new Vector3(x, y, 0)); // 添加顶点
            uvs.Add(new Vector2((x + radius) / (2 * radius), (y + radius) / (2 * radius))); // 添加UV坐标
        }

        // 生成三角形
        tris = new List<int>();
        for (int i = 1; i < numSides - 1; i++)
        {
            tris.Add(0); // 中心顶点
            tris.Add(i);
            tris.Add(i + 1);
        }

        // 将数据作用到Mesh和MeshFilter里
        m_Mesh.SetVertices(verts);
        m_Mesh.triangles = tris.ToArray();
        m_Mesh.uv = uvs.ToArray();

        m_MeshFilter.mesh.Clear();
        m_MeshFilter.mesh = m_Mesh;
        m_Mesh.RecalculateNormals();
    }
}
