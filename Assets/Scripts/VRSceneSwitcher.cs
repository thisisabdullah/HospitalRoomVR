using UnityEngine;
using UnityEngine.SceneManagement;

public class VRSceneSwitcher : MonoBehaviour
{
    [Tooltip("The name of the scene to load when A is pressed")]
    public string sceneA = "SceneA";
    
    [Tooltip("The name of the scene to load when B is pressed")]
    public string sceneB = "SceneB";

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        // Check for left controller A button press
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch) || 
            OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            LoadScene(sceneA);
        }
        
        // Check for left controller B button press
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch) || 
            OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            LoadScene(sceneB);
        }
    }
    
    private void LoadScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            // Check if the scene exists in build settings
            if (SceneUtility.GetBuildIndexByScenePath(sceneName) >= 0)
            {
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                Debug.LogError($"Scene '{sceneName}' not found in build settings!");
            }
        }
    }
}