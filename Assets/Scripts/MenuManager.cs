using UnityEngine;

public class MenuManager : MonoBehaviour
{

    public void LoadGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }

    public void BackToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
 
    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenHTP()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("HowToPlay"); 
    }
}   
