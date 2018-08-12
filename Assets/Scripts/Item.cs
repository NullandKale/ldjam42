using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public GameObject Text;

    public CodeBlock Block;

    private void Start()
    {
        Block = Level.currentLevel.getRandomCodeBlock();
        if(Block == null)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, PlayerController.Instance.transform.position) < 1)
        {
            Text.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                PlayerController.Instance.AddBlock(Block);
                Destroy(gameObject);
            }
        }
        else
        {
            Text.SetActive(false);
        }
    }
}