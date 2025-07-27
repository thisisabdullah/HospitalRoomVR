using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSaveSystem : MonoBehaviour
{
    public Transform player;
    
    // Key constants
    private const string POS_KEY = "PlayerPosition";
    private const string ROT_KEY = "PlayerRotation";

    void Start()
    {
        LoadPlayerData();
        SceneManager.activeSceneChanged += OnSceneChanged; // Subscribe to scene change event
    }

    void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged; // Unsubscribe to prevent memory leaks
    }
    
    void OnApplicationQuit()
    {
        SavePlayerData();
    }

    private void OnDisable()
    {
        SavePlayerData();
    }

    // Called automatically when scene changes
    private void OnSceneChanged(Scene current, Scene next)
    {
        SavePlayerData();
    }

    public void SavePlayerData()
    {
        // Save as JSON strings (more compact than individual floats)
        PlayerPrefs.SetString(POS_KEY, JsonUtility.ToJson(player.localPosition));
        PlayerPrefs.Save();
        
        Debug.Log($"Saved Position: {player.position} | Rotation: {player.rotation.eulerAngles}");
    }

    public void LoadPlayerData()
    {
        if (PlayerPrefs.HasKey(POS_KEY))
        {
            player.localPosition = JsonUtility.FromJson<Vector3>(PlayerPrefs.GetString(POS_KEY));
            Debug.Log($"Loaded Position: {player.position} | Rotation: {player.rotation.eulerAngles}");
        }
    }

    public void ResetData()
    {
        PlayerPrefs.DeleteKey(POS_KEY);
        PlayerPrefs.DeleteKey(ROT_KEY);
    }
}