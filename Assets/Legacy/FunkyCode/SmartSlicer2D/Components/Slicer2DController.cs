using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

// Controller
public class Slicer2DController : MonoBehaviour {
	public enum SliceType {Linear, Complex, Point, Polygon, Explode, Create};
	public enum SliceRotation {Random, Vertical, Horizontal}
	public enum CreateType {Slice, PolygonType}

	public bool addForce = true;
	public float addForceAmount = 5f;

	[Tooltip("Slice type represents algorithm complexity")]
	public SliceType sliceType = SliceType.Complex;
	public Slice2DLayer sliceLayer = Slice2DLayer.Create();

	public Polygon slicePolygon = Polygon.Create (Polygon.PolygonType.Pentagon);

	[Tooltip("Minimum distance between points (SliceType: Complex")]
	private float minVertsDistance = 1f;

	// Polygon Destroyer type settings
	public Polygon.PolygonType polygonType = Polygon.PolygonType.Circle;
	public float polygonSize = 1;
	public bool polygonDestroy = false;

	// Polygon Creator
	public Material material;
	public CreateType createType = CreateType.Slice;

	// Complex Slicer
	public Slicer2D.SliceType complexSliceType = Slicer2D.SliceType.SliceHole;

	// Slicer Visuals
	public bool drawSlicer = true;
	public float lineWidth = 1.0f;
	public float zPosition = 0f;
	public Color slicerColor = Color.black;

	// Point Slicer
	public SliceRotation sliceRotation = SliceRotation.Random;

	// Events Handler
	private static List<List<Vector2f>> complexEvents = new List<List<Vector2f>>();
	private static List<Pair2f> linearEvents = new List<Pair2f>();

	// Events Input Handler
	private static List<Vector2f> complexPairs = new List<Vector2f>();
	private static Pair2f linearPair = Pair2f.Zero();

	public static Slicer2DController instance;
	private bool mouseDown = false;

	public static Color[] slicerColors = {Color.black, Color.green, Color.yellow , Color.red, new Color(1f, 0.25f, 0.125f)};

	private static KnifeController knifeController;

	public void Awake()
	{
		instance = this;
		knifeController = FindFirstObjectByType<KnifeController>();
	}

	public static Vector2f GetMousePosition()
	{
		//Vector3 pos = Input.mousePosition; //pos.z = Camera.main.transform.position.z;
		return(knifeController.currentPosition);
	}

	public void SetSliceType(int type)
	{
		
		sliceType = (SliceType)type;
	}

	public void SetLayerType(int type)
	{
		if (type == 0) 
			sliceLayer.SetLayerType((Slice2DLayer.Type)0);
		else {
			sliceLayer.SetLayerType((Slice2DLayer.Type)1);
			sliceLayer.DisableLayers ();
			sliceLayer.SetLayer (type - 1, true);
		}
	}

	public void SetSlicerColor(int colorInt)
	{
		slicerColor = slicerColors [colorInt];
	}

	public void OnRenderObject() {
		Vector2f pos = GetMousePosition();

		if (drawSlicer == false)
			return;

		Max2D.SetBorder (true);
		Max2D.SetSmooth(true);
		Max2D.SetLineWidth (lineWidth * .5f);

		if (mouseDown) {
			Max2D.SetColor (slicerColor);

			switch (sliceType) {
				case SliceType.Complex:
					if (complexPairs.Count > 0) {
						Max2D.DrawStrippedLine (complexPairs, minVertsDistance, zPosition);
						Max2D.DrawLineSquare (complexPairs.Last(), 0.5f, zPosition);
						Max2D.DrawLineSquare (complexPairs.First (), 0.5f, zPosition);
					}
					break;

				case SliceType.Create:
					if (createType == CreateType.Slice) {
						if (complexPairs.Count > 0) {
							Max2D.DrawStrippedLine (complexPairs, minVertsDistance, zPosition, true);
							Max2D.DrawLineSquare (complexPairs.Last(), 0.5f, zPosition);
							Max2D.DrawLineSquare (complexPairs.First (), 0.5f, zPosition);
						}
					} else {
						Max2D.DrawStrippedLine (Polygon.Create(polygonType, polygonSize).pointsList, minVertsDistance, zPosition, true, pos);
					}
					break;
				
				case SliceType.Linear:
					Max2D.DrawLine (linearPair.A, linearPair.B, zPosition);
					Max2D.DrawLineSquare (linearPair.A, 0.5f, zPosition);
					Max2D.DrawLineSquare (linearPair.B, 0.5f, zPosition);
					break;

				case SliceType.Point:
					break;

				case SliceType.Explode:
					break;

				case SliceType.Polygon:
					slicePolygon = Polygon.Create (polygonType, polygonSize);
					Max2D.DrawStrippedLine (slicePolygon.pointsList, minVertsDistance, zPosition, false, pos);
					break;
				
				default:
					break; 
			}
		}
	}

	public void LateUpdate()
	{
		Vector2f pos = GetMousePosition ();

		complexEvents.Clear ();
		linearEvents.Clear ();

		switch (sliceType) {	
			case SliceType.Complex:
				UpdateComplex (pos);
				break;

			default:
				break; 
		}
	}

	private void UpdateComplex(Vector2f pos)
	{
		if (mouseDown == false && knifeController.isCutting) {
			complexPairs.Clear ();
			complexPairs.Add(pos);
		}

		if (knifeController.isCutting) {
			Vector2f posMove = new Vector2f (complexPairs.Last ());
			while ((Vector2f.Distance (posMove, pos) > minVertsDistance)) {
				float direction = Vector2f.Atan2 (pos, posMove);
				posMove.Push (direction, minVertsDistance);
				complexPairs.Add (new Vector2f (posMove));
			}

			mouseDown = true;
		}

		if (mouseDown == true && !knifeController.isCutting) {
			mouseDown = false;
			Slicer2D.complexSliceType = complexSliceType;
			ComplexSlice (complexPairs);
			complexEvents.Add (complexPairs);
		}
	}

	private void ComplexSlice(List <Vector2f> slice)
	{
		List<Slice2D> results = Slicer2D.ComplexSliceAll (slice, sliceLayer);
		
		if (addForce != true) 
			return;
		foreach (Slice2D id in results)
		foreach (GameObject gameObject in id.gameObjects) {
			Rigidbody2D rigidBody2D = gameObject.GetComponent<Rigidbody2D> ();
			if (rigidBody2D) {
				List<Pair2f> list = Pair2f.GetList (id.collisions);
				float forceVal = 1.0f / list.Count;
				foreach (Pair2f p in list) {
					float sliceRotation = -Vector2f.Atan2 (p.B, p.A);
					Vector2 force = new Vector2 (Mathf.Cos (sliceRotation) * addForceAmount, Mathf.Sin (sliceRotation) * addForceAmount);
					rigidBody2D.AddForceAtPosition (forceVal * force, (p.A.Get () + p.B.Get ()) / 2f);
				}
			}
		}
	}
}