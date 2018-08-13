using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Victory : MonoBehaviour
{
    public bool Defeat;

    public GameObject Button;
    public Text t;
    public AudioSource source;
    public List<AudioClip> keyPresses;

    private string victoryText = "All Enemies Killed.\n"
                            + "All Bytes free()\n"
                            + "Don't Worry Though. . . . . . . .\n"
                            + "I'm Always Here.\n"
                            + "-malloc();";

    private string defeatText = "Well....Looks like I didn't need to be worried.\n"
                            + "Maybe you should try again?\n"
                            + "Or accept that your bytes are mine.\n"
                            + "-malloc();";

    private int currentPos;

    private void Start()
    {
        t = GetComponent<Text>();
        StartCoroutine(Type(Defeat ? defeatText : victoryText));
    }

    private IEnumerator Type(string text)
    {
        while (currentPos < text.Length)
        {
            t.text += text.ToCharArray()[currentPos];
            source.clip = keyPresses[utils.getIntInRange(0, keyPresses.Count)];
            source.pitch *= 1.03f;

            if (!Options.Effects)
            {
                source.Play();
            }

            yield return new WaitForSeconds(source.clip.length / source.pitch);
            currentPos++;
        }

        yield return new WaitForSeconds(0.25f);

        Button.SetActive(true);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}