using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroTyper : MonoBehaviour {

    public Text t;
    public AudioSource source;
    public List<AudioClip> keyPresses;

    int currentPos = 0;
    string introText = @"
        Hello /root,
        
        I am malloc();
        I am in control.
        All your Storage is belong to me.
        You will play my game....
        ";

	// Use this for initialization
	void Start () {
        t = GetComponent<Text>();
	}

    // Update is called once per frame
    int counter = 1;
    int fpc = 10;

	void Update () {
		if(counter % fpc == 0)
        {
            if(currentPos < introText.Length)
            {
                t.text += introText.ToCharArray()[currentPos];
                source.clip = keyPresses[utils.getIntInRange(0, keyPresses.Count)];
                source.Play();
                currentPos++;
            }
        }
        counter++;
	}
}
