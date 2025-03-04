using System;
using System.Collections;
using UnityEngine;

public class Knife : MonoBehaviour
{
    public Vector2 origin;
    
    public Vector2 pointA; // 起始点
    public Vector2 pointB; // 控制点
    public Vector2 pointC; // 结束点
    
    public Vector2 pointD;
    public Vector2 pointE;
    public Vector2 pointF;

    public Vector3 startRotation; // 起始旋转
    public Vector3 endRotation; // 目标旋转

    public float cutDuration; // 切割持续时间
    public AnimationCurve cutCurve; // AnimationCurve 来控制切割动画的时间进度
    
    public float longCutDuration; // 长切割持续时间
    public AnimationCurve longCutCurve; // AnimationCurve 来控制长切割动画的时间进度

    public GameObject knifeCore;
    public GameObject longKnifeCore;
    
    private CutDetector cutDetector;
    private float cutTimeElapsed;
    private TrailRenderer trailRenderer;
    
    public event Action cutEvent;

    private void Start()
    {
        cutDetector = knifeCore.GetComponent<CutDetector>();
        trailRenderer = knifeCore.GetComponent<TrailRenderer>();
        cutDetector.alreadyCut = false;
    }

    public void Cut()
    {
        cutTimeElapsed = 0f; // 重置时间
        StartCoroutine(CutCoroutine()); // 启动切割协程
    }
    
    public void LongCut()
    {
        cutTimeElapsed = 0f; // 重置时间
        StartCoroutine(CutCoroutine(1)); // 启动长切割协程
        cutEvent?.Invoke();
    }

    private IEnumerator CutCoroutine(int type = 0)
    {
        GameObject core = knifeCore;
        float duration = cutDuration;
        Vector2 point1 = pointA;
        Vector2 point2 = pointB;
        Vector2 point3 = pointC;
        AnimationCurve curve = cutCurve;

        if (type == 1)
        {
            core = longKnifeCore;
            duration = longCutDuration;
            point1 = pointD;
            point2 = pointE;
            point3 = pointF;
            curve = longCutCurve;
        }
        
        core.SetActive(true);
        
        transform.position = point1;

        trailRenderer.emitting = true;

        while (cutTimeElapsed < duration)
        {
            cutTimeElapsed += Time.deltaTime;

            var t = curve.Evaluate(cutTimeElapsed / duration);

            Vector2 currentPosition = CalculateBezierPoint(t, point1, point2, point3);
            transform.position = currentPosition;

            transform.rotation = Quaternion.Euler(Vector3.Lerp(startRotation, endRotation, t));

            yield return null;
        }

        trailRenderer.emitting = false;
        trailRenderer.Clear();

        core.SetActive(false);

        transform.position = origin;
        transform.rotation = Quaternion.Euler(startRotation);

        if (type == 0)
        {
            cutDetector.alreadyCut = false;
        }
    }

    private static Vector2 CalculateBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2)
    {
        var u = 1 - t;
        var tt = t * t;
        var uu = u * u;

        Vector2 p = uu * p0; // (1-t)^2 * P0
        p += 2 * u * t * p1; // 2 * (1-t) * t * P1
        p += tt * p2; // t^2 * P2

        return p;
    }
}
