using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        utils.setSeed(Random.Range(int.MinValue, int.MaxValue));
        SceneManager.LoadScene("introVid");
    }

    public void Quit()
    {
        Application.Quit();
    }
}