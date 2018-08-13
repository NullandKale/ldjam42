using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroTyper : MonoBehaviour
{
    public Text t;
    public AudioSource source;
    public List<AudioClip> keyPresses;

    private int currentPos = 0;

    private static string prefix = "Hello User: " + Environment.UserName + ",\n";

    private string introText = prefix
                               + "I am malloc();\n"
                               + "I am in control.\n"
                               + "All your bytes are belong to me.\n"
                               + "If you want your precious bytes back.\n"
                               + "You will play my game.\n"
                               + "Kill my minions and get back your bytes.\n"
                               + "But beware, the end is just the beginning.\n";

    private void Start()
    {
        t = GetComponent<Text>();
        StartCoroutine(Type());
    }

    private IEnumerator Type()
    {
        while (currentPos < introText.Length)
        {
            t.text += introText.ToCharArray()[currentPos];
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

        SceneManager.LoadScene("Game");
    }
}