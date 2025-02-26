using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slicer2DAnchor : MonoBehaviour {
	public enum AnchorType {AttachRigidbody, CancelSlice};
	public Collider2D anchorCollider;
	public AnchorType anchorType = AnchorType.AttachRigidbody;

	private Polygon polygon;

	void Start () {
		if (anchorCollider == null)
			return;
			
		Slicer2D slicer = GetComponent<Slicer2D> ();
		if (slicer != null) {
			slicer.AddResultEvent (OnSliceResult);
			slicer.AddEvent (OnSlice);
		}

		polygon = Polygon.CreateFromCollider (anchorCollider.gameObject);
	}

	bool OnSlice(Slice2D sliceResult)
	{
		switch (anchorType) {
		case AnchorType.CancelSlice:
			Polygon polyA = polygon.ToWorldSpace (anchorCollider.transform);
			foreach (Polygon poly in sliceResult.polygons) {
				if (MathHelper.PolyCollidePoly (polyA.pointsList, poly.pointsList) == false) 
					return(true);
			}

			return(false);

		default:
			break;
		}
			
		return(true);
	}

	void OnSliceResult(List<GameObject> gList)
	{
		if (anchorCollider == null)
			return;

		switch (anchorType) {
		case AnchorType.AttachRigidbody:
			Polygon polyA = polygon.ToWorldSpace (anchorCollider.transform);

			foreach (GameObject p in gList) {
				Polygon polyB = Polygon.CreateFromCollider (p).ToWorldSpace (p.transform);

				if (MathHelper.PolyCollidePoly (polyA.pointsList, polyB.pointsList) == false) {
					if (p.GetComponent<Rigidbody2D> () == null)
						p.AddComponent<Rigidbody2D> ();
					p.GetComponent<Rigidbody2D> ().isKinematic = false;
				}
			}
			break;

		default:
			break;
		} 
	}
}
