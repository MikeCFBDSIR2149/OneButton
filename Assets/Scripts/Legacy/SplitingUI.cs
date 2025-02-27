using UnityEngine;
using UnityEngine.UI;

public class SplitingUI : MonoBehaviour
{
    public static SplitingUI instance;

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        FindFirstObjectByType<MeshGen>().CreateMesh();
        transform.Find("RestartBtn").GetComponent<Button>().onClick.AddListener(() =>
        {
            var sliceObjects = FindObjectsByType<MeshGen>(FindObjectsSortMode.None);
            for (var i = 1; i < sliceObjects.Length; i++) Destroy(sliceObjects[i].gameObject);
            sliceObjects[0].CreateMesh();
            sliceObjects[0].transform.position = new Vector3(0, 0, 0);
        });
    }
}
