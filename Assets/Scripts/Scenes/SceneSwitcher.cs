using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void SwitchToSceneAtName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}