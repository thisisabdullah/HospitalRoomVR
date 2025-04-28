using UnityEngine;
using UnityEngine.EventSystems;

public class CubeFaceExtender : MonoBehaviour
{
    public float ExtendAmount = 0.1f;
    private Mesh mesh;
    private Vector3[] originalVertices;
    private Vector3[] vertices;
    private int[] triangles;
    [HideInInspector] public MeshRenderer meshRenderer;

    void Start()
    {
        // Get the mesh and its vertices
        mesh = GetComponent<MeshFilter>().mesh;
        meshRenderer = GetComponent<MeshRenderer>();
        
        // Duplicate the mesh to prevent affecting other cubes
        mesh = Instantiate(mesh);
        GetComponent<MeshFilter>().mesh = mesh;

        // Store original and working vertices
        originalVertices = mesh.vertices;
        vertices = mesh.vertices;
        triangles = mesh.triangles;
    }
    
    [ContextMenu("forward")]
    public void Forward()
    {
        meshRenderer.enabled = true;
        ExtendFace(Vector3.forward, ExtendAmount);
    }
    
    [ContextMenu("backward")]
    public void Backword()
    {
        meshRenderer.enabled = true;
        ExtendFace(Vector3.back, ExtendAmount);
    }
    
    [ContextMenu("right")]
    public void Right()
    {
        meshRenderer.enabled = true;
        ExtendFace(Vector3.right, ExtendAmount);
    }
    
    [ContextMenu("left")]
    public void Left()
    {
        meshRenderer.enabled = true;
        ExtendFace(Vector3.left, ExtendAmount);
    }
    
    [ContextMenu("up")]
    public void Up()
    {
        meshRenderer.enabled = true;
        ExtendFace(Vector3.up, ExtendAmount);
    }
    
    [ContextMenu("down")]
    public void Down()
    {
        meshRenderer.enabled = true;
        ExtendFace(Vector3.down, ExtendAmount);
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
    
    public void DeExtendFace(Vector3 direction, float amount)
    {
        // Convert direction to local space if necessary
        direction = transform.InverseTransformDirection(direction);

        // Loop through all vertices and de-extend the ones on the specified face
        for (int i = 0; i < vertices.Length; i++)
        {
            if (IsOnFace(originalVertices[i], direction))
            {
                // Only de-extend if it does not go beyond the original position
                Vector3 targetPosition = vertices[i] - direction * amount;
                if (Vector3.Dot(targetPosition - originalVertices[i], direction) > 0)
                {
                    vertices[i] = targetPosition;
                }
                else
                {
                    vertices[i] = originalVertices[i];
                    meshRenderer.enabled = false;
                }
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
