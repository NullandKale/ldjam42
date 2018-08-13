using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MainMenu : MonoBehaviour
{
    public Text Music;
    public Text Effects;

    public void Play()
    {
        utils.setSeed(Random.Range(int.MinValue, int.MaxValue));
        SceneManager.LoadScene("introVid");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ToggleMusic()
    {
        Options.Music = !Options.Music;
        Music.text = (Options.Music ? "Enable" : "Disable") + " Music";
    }

    public void ToggleEffects()
    {
        Options.Effects = !Options.Effects;
        Effects.text = (Options.Effects ? "Enable" : "Disable") + " Effects";
    }
}