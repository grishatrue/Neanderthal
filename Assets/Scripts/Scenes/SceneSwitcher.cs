using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{

    private void Start()
    {
        if (GetComponent<AudioListener>().enabled == false)
        {
            GetComponent<AudioListener>().enabled = true;
        }
    }

    public void SwitchToScene(string sceneName)
    {
        if (GetComponent<AudioListener>().enabled == true)
        {
            GetComponent<AudioListener>().enabled = false;
        }

        SceneManager.LoadScene(sceneName);
    }
}