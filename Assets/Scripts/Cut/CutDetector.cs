using System;
using UnityEngine;

public class CutDetector : MonoBehaviour
{
    private Collider2D bladeCollider;
    internal bool alreadyCut;

    private void Start()
    {
        bladeCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!bladeCollider) return;
        if (alreadyCut) return;
        
        // 检测到可切割物体时触发
        if (!other.TryGetComponent<CuttableObject>(out CuttableObject cuttable)) return;
        alreadyCut = true;
        cuttable.HandleCut(bladeCollider.transform.position);
    }
}
