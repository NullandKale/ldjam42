using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

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