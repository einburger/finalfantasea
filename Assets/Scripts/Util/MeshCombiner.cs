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
        if (meshFilters.Length == 0) 
        {
            return;
        }
        CombineInstance[] combiners = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++) 
        {
            combiners[i].mesh = meshFilters[i].sharedMesh;
            combiners[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
        }

        if (combiners != null) {
            GetComponent<MeshFilter>().sharedMesh = new Mesh();
            GetComponent<MeshFilter>().sharedMesh.CombineMeshes(combiners);
            gameObject.SetActive(true);
            transform.rotation = oldRotation;
            transform.position = oldPosition;
        }
    }
}
