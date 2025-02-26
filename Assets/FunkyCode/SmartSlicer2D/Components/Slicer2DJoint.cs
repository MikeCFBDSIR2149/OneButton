using System.Collections.Generic;
using UnityEngine;

public class Slicer2DJoint : MonoBehaviour {
	private AnchoredJoint2D anchoredJoint = null;
	private Rigidbody2D body2D;

	void Start () {
		Slicer2D slicer = GetComponent<Slicer2D> ();
		slicer.AddResultEvent(OnSlice);

		body2D = GetComponent<Rigidbody2D> ();

		GetJoint ();
	}

	void OnSlice(List<GameObject> gList)
	{
		foreach (GameObject g in gList) {
			AnchoredJoint2D joint = g.GetComponent<AnchoredJoint2D> ();
			if (joint != null && Polygon.CreateFromCollider (g).PointInPoly (new Vector2f (joint.anchor)) == false)
				Destroy (joint);
		}

		if (body2D == null || anchoredJoint == null)
			return;
		
		foreach (GameObject g in gList)
			if (Polygon.CreateFromCollider (g).PointInPoly (new Vector2f (Vector2.zero))) //anchoredJoint.connectedAnchor
				anchoredJoint.connectedBody = g.GetComponent<Rigidbody2D> ();
	}

	void GetJoint() {
		foreach (AnchoredJoint2D joint in FindObjectsOfType<AnchoredJoint2D> ()) 
			if (joint.connectedBody == body2D) {
				anchoredJoint = joint;
				return;
			}
	}
}