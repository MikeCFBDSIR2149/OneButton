using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeController : MonoBehaviour
{
    public bool isCutting;
    public bool isCD;
    public Vector2f startPosition = new Vector2f(0f, 6f); // 初始位置
    public Vector2f endPosition = new Vector2f(0f, 1f);   // 目标位置
    public Vector2f currentPosition;

    private Coroutine _moveCoroutine; // 用于控制协程

    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MoveKnife();
        }
    }
    
    // 外部调用此函数触发移动
    public void MoveKnife()
    {
        if (isCD)
        {
            return;
        }
        StartCoroutine(MoveRoutine());
    }

    // 移动协程
    private IEnumerator MoveRoutine()
    {
        // 初始化位置
        transform.position = new Vector3(startPosition.GetX(), startPosition.GetY(), 0f);
        currentPosition = startPosition;

        // 调用切割开始
        StartCutting();

        float duration = 0.15f; // 持续时间
        float elapsed = 0f;
        Vector3 start3D = new Vector3(startPosition.GetX(), startPosition.GetY(), 0f);
        Vector3 end3D = new Vector3(endPosition.GetX(), endPosition.GetY(), 0f);

        // 移动过程
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            
            // 更新位置
            transform.position = Vector3.Lerp(start3D, end3D, t);
            currentPosition = new Vector2f(transform.position.x, transform.position.y);
            
            yield return null;
        }

        // 确保最终位置精确
        transform.position = end3D;
        currentPosition = endPosition;
        
        // 调用切割结束
        EndCutting();
        
        yield return new WaitForSeconds(0.08f);
        
        isCD = false;
    }

    public void StartCutting()
    {
        isCutting = true;
        isCD = true;
        Debug.Log("切割开始");
    }

    public void EndCutting()
    {
        isCutting = false;
        transform.position = new Vector3(startPosition.GetX(), startPosition.GetY(), 0f);
        Debug.Log("切割结束");
    }
}
