using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenes : MonoBehaviour
{
    public string VrSceneName;
    public string MrSceneName;

    public void LaodVrScene()
    {
        SceneManager.LoadScene(VrSceneName);
    }
    
    public void LaodMrScene()
    {
        SceneManager.LoadScene(MrSceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
