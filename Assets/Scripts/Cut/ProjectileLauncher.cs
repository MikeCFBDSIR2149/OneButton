using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public GameObject[] projectilePrefabs; // 发射的物体预制体数组
    public Transform launchPoint; // 发射点
    public float timer; // 发射间隔时间

    private void Update()
    {
        timer -= Time.deltaTime;
        // 当计时器小于 0 时发射物体
        if (timer < 0)
        {
            LaunchProjectile();
            timer = 1.0f; // 重置计时器
        }
    }

    private void LaunchProjectile()
    {
        // 检查是否有可用的预制体
        if (projectilePrefabs.Length == 0)
        {
            Debug.LogError("没有设置发射的物体预制体！");
            return;
        }

        // 随机选择一个预制体
        int randomIndex = Random.Range(0, projectilePrefabs.Length);
        GameObject selectedPrefab = projectilePrefabs[randomIndex];

        // 实例化发射的物体
        GameObject projectile = Instantiate(selectedPrefab, launchPoint.position, Quaternion.identity);

        /*// 计算初始速度
        float angleInRadians = launchAngle * Mathf.Deg2Rad; // 将角度转换为弧度
        Vector3 launchDirection = new Vector3(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians), 0);
        Vector3 initialVelocity = launchDirection * launchSpeed;

        // 设置物体的初始速度
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb)
        {
            rb.velocity = initialVelocity;
        }
        else
        {
            Debug.LogError("发射的物体需要附加 Rigidbody2D 组件！");
        }*/
    }
}
