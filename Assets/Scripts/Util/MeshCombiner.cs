using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MeshCombiner : MonoBehaviour
{
    public void Start() 
    {
        Quaternion oldRotation = transform.rotation;
        Vector3 oldPosition = transform.position;

        transform.rotation = Quaternion.identity;
        transform.position = Vector3.zero;


        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combiners = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++) 
        {
            combiners[i].mesh = meshFilters[i].mesh;
            combiners[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
        }

        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combiners);
        transform.gameObject.SetActive(true);
        transform.rotation = oldRotation;
        transform.position = oldPosition;
    }
}
