using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyAfterDelay : MonoBehaviour
{
    public float timeout;
    public Text text;
	// Use this for initialization
	void Start ()
    {
        Destroy(this, timeout);
	}
}
