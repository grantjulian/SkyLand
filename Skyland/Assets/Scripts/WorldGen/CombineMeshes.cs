using UnityEngine;
using System.Collections;

// Copy meshes from children into the parent's Mesh.
// CombineInstance stores the list of meshes.  These are combined
// and assigned to the attached Mesh.

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CombineMeshes : MonoBehaviour
{
    public void Combine(Material M)
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        if (meshFilters.Length == 1)
        {
            Destroy(gameObject);
        }
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
       
        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);

            i++;
        }
        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine, true);
        i = 1;
        while(i < meshFilters.Length)
        {

            Destroy(meshFilters[i].gameObject);
            i++;
        }
        if (i > 1)
        {
            gameObject.GetComponent<MeshRenderer>().material = M;
        }
        transform.gameObject.SetActive(true);
    }
}