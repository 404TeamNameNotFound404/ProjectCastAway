using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private int firstPlayableSceneIndex = 1;

    public void PlayGame()
    {

        SceneManager.LoadScene(firstPlayableSceneIndex); 
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
