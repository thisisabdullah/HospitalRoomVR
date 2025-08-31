using UnityEngine;
using UnityEngine.SceneManagement;

public class VRSceneSwitcher : MonoBehaviour
{
    [Tooltip("The name of the scene to load when A is pressed")]
    public string sceneA = "SceneA";
    
    [Tooltip("The name of the scene to load when B is pressed")]
    public string sceneB = "SceneB";

    [Tooltip("The name of the VR scene to load when Y is pressed")]
    public string vrScene = "VRScene";

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        // Check for A button (Right controller)
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            LoadScene(sceneA);
        }

        // Check for B button (Right controller)
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            LoadScene(sceneB);
        }

        // Check for Y button (Left controller)
        if (OVRInput.GetDown(OVRInput.Button.Four))
        {
            LoadScene(vrScene);
        }

    }

    private void LoadScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            // Load only if scene exists in Build Settings
            int sceneIndex = SceneUtility.GetBuildIndexByScenePath(sceneName);
            if (sceneIndex >= 0)
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
