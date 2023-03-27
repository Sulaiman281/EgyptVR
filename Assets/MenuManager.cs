using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("EgyptTour");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}