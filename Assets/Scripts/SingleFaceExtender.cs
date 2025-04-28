using UnityEngine;

public class SingleFaceExtender : MonoBehaviour
{
    public float ExtendAmount = 0.1f;
    private Mesh mesh;
    private Vector3[] originalVertices;
    private Vector3[] vertices;
    private int[] triangles;
    
    void Start()
    {
        // Get the mesh and its vertices
        mesh = GetComponent<MeshFilter>().mesh;
        
        // Duplicate the mesh to prevent affecting other cubes
        mesh = Instantiate(mesh);
        GetComponent<MeshFilter>().mesh = mesh;

        // Store original and working vertices
        originalVertices = mesh.vertices;
        vertices = mesh.vertices;
        triangles = mesh.triangles;
    }
    
    [ContextMenu("UP")]
    public void Up()
    {
        ExtendFace(Vector3.up, ExtendAmount);
    }
    
    public void ExtendFace(Vector3 direction, float amount)
    {
        // Convert direction to local space if necessary
        direction = transform.InverseTransformDirection(direction);

        // Loop through all vertices and extend the ones on the specified face
        for (int i = 0; i < vertices.Length; i++)
        {
            if (IsOnFace(originalVertices[i], direction))
            {
                // Extend vertex in the given direction
                vertices[i] += direction * amount;
            }
        }

        // Update the mesh with the new vertices
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    // Helper method to check if a vertex is on a specific face
    private bool IsOnFace(Vector3 vertex, Vector3 direction)
    {
        float threshold = 0.01f;

        // Check alignment in the specified direction
        if (direction == Vector3.up)
            return Mathf.Abs(vertex.y - 0.5f) < threshold;
        if (direction == Vector3.down)
            return Mathf.Abs(vertex.y + 0.5f) < threshold;
        if (direction == Vector3.right)
            return Mathf.Abs(vertex.x - 0.5f) < threshold;
        if (direction == Vector3.left)
            return Mathf.Abs(vertex.x + 0.5f) < threshold;
        if (direction == Vector3.forward)
            return Mathf.Abs(vertex.z - 0.5f) < threshold;
        if (direction == Vector3.back)
            return Mathf.Abs(vertex.z + 0.5f) < threshold;

        return false;
    }
}
