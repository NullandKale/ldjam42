using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroTyper : MonoBehaviour
{
    public Text t;
    public AudioSource source;
    public List<AudioClip> keyPresses;

    private int currentPos = 0;

    private string introText = "Hello /root,\n"
                               + "I am malloc();\n"
                               + "I am in control.\n"
                               + " All your Storage is belong to me.\n"
                               + "You will play my game....\n";

    // Use this for initialization
    private void Start()
    {
        t = GetComponent<Text>();
        StartCoroutine(Type());
    }

    // Update is called once per frame
    private int counter = 1;

    private int fpc = 80;

    private IEnumerator Type()
    {
        while (currentPos < introText.Length)
        {
            t.text += introText.ToCharArray()[currentPos];
            source.clip = keyPresses[utils.getIntInRange(0, keyPresses.Count)];
            source.Play();
            yield return new WaitForSeconds(source.clip.length);
            currentPos++;
        }

        yield return 0;
    }
}