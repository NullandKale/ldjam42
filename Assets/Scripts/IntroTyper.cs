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
    }

    // Update is called once per frame
    private int counter = 1;

    private int fpc = 20;

    private void Update()
    {
        if (counter % fpc == 0)
        {
            if (currentPos < introText.Length)
            {
                t.text += introText.ToCharArray()[currentPos];
                source.clip = keyPresses[utils.getIntInRange(0, keyPresses.Count)];
                source.Play();
                currentPos++;
            }

            counter = 1;
        }
        counter++;
    }
}