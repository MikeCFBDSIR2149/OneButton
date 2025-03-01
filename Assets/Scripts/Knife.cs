using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    public Vector2 pointA; // 起始点
    public Vector2 pointB; // 控制点
    public Vector2 pointC; // 结束点
    
    public Vector3 startRotation; // 起始旋转
    public Vector3 endRotation; // 目标旋转

    public float cutDuration; // 切割持续时间
    public AnimationCurve cutCurve; // AnimationCurve 来控制切割动画的时间进度

    public GameObject knifeCore;
    private CutDetector cutDetector;
    private TrailRenderer trailRenderer;
    private float cutTimeElapsed;

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
    
    private IEnumerator CutCoroutine()
    {
        knifeCore.SetActive(true);
        
        trailRenderer.emitting = true;
        
        while (cutTimeElapsed < cutDuration)
        {
            cutTimeElapsed += Time.deltaTime;
            
            float t = cutCurve.Evaluate(cutTimeElapsed / cutDuration);
            
            Vector2 currentPosition = CalculateBezierPoint(t, pointA, pointB, pointC);
            transform.position = currentPosition;
            
            transform.rotation = Quaternion.Euler(Vector3.Lerp(startRotation, endRotation, t));

            yield return null;
        }
        
        trailRenderer.emitting = false;
        trailRenderer.Clear();
        
        knifeCore.SetActive(false);
        
        transform.position = pointA;
        transform.rotation = Quaternion.Euler(startRotation);

        cutDetector.alreadyCut = false;
    }
    
    private static Vector2 CalculateBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector2 p = uu * p0; // (1-t)^2 * P0
        p += 2 * u * t * p1; // 2 * (1-t) * t * P1
        p += tt * p2; // t^2 * P2

        return p;
    }
}
