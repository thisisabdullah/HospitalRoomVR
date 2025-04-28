using UnityEngine;
using System.Linq;
using System.Collections;
using Meta.XR.MRUtilityKit;
using UnityEngine.SceneManagement;

public class BathroomPlacement : MonoBehaviour
{
    public string VrSceneName;
    public GameObject bathroomPrefab; // Assign the bathroom prefab in the Inspector

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(3f);
        // Ensure MRUK is initialized
        if (MRUK.Instance != null)
        {
            // Wait for the scene to be fully loaded
            MRUK.Instance.RegisterSceneLoadedCallback(OnRoomScanComplete);
        }
        else
        {
            Debug.LogError("MRUK Instance is not initialized!");
        }
    }

    private void OnRoomScanComplete()
    {
        Debug.Log("Room Scan Complete! Searching for the floor...");

        // Get all EffectMeshes (they represent scanned surfaces)
        var effectMeshes = FindObjectsOfType<EffectMesh>();

        if (effectMeshes.Length == 0)
        {
            Debug.LogError("No EffectMeshes found!");
            return;
        }

        // Find the lowest EffectMesh (assuming it's the floor)
        EffectMesh floorMesh = effectMeshes.OrderBy(mesh => mesh.transform.position.y).FirstOrDefault();

        if (floorMesh != null)
        {
            Debug.Log("Floor detected at Y position: " + floorMesh.transform.position.y);

            // Move the bathroom prefab to align with the floor
            Vector3 newPosition = new Vector3(floorMesh.transform.position.x, floorMesh.transform.position.y, floorMesh.transform.position.z);
            bathroomPrefab.transform.position = newPosition;

            // Optional: Rotate prefab to match floor orientation
            bathroomPrefab.transform.rotation = Quaternion.Euler(0, floorMesh.transform.eulerAngles.y, 0);

            // Enable the prefab
            bathroomPrefab.SetActive(true);
        }
        else
        {
            Debug.LogError("No floor detected in scanned data!");
        }
    }

    public void LoadVRScene()
    {
        SceneManager.LoadScene(VrSceneName);
    }
}
