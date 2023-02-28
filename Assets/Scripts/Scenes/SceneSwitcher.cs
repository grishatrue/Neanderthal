using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void GoToSceneAtName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}