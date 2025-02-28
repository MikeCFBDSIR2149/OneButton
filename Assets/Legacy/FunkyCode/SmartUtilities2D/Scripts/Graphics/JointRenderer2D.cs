using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointRenderer2D : MonoBehaviour {
	public Color color = Color.white;
	public float lineWidth = 1;
	public bool smooth = true;

	private float lineOffset = -0.001f;
	private AnchoredJoint2D joint;

	public void Start() {
		joint = GetComponent<AnchoredJoint2D> ();
	}

	public void OnRenderObject() {
		if (joint == null || joint.connectedBody == null)
			return;
		
		Max2D.SetLineWidth (lineWidth);
		Max2D.SetColor (color);
		Max2D.SetSmooth (smooth);
		Max2D.SetBorder (false);

		Max2D.DrawLine (new Vector2f (transform.TransformPoint (Vector2.zero)), new Vector2f (joint.connectedBody.transform.TransformPoint (joint.anchor)), transform.position.z + lineOffset);
	}
}
