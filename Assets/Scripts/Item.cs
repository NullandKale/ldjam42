using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public Text Text;

    public CodeBlock Block;

    private void Start()
    {
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, PlayerController.Instance.transform.position) < 1)
        {
            if (Block == null)
            {
                Block = Level.currentLevel.GetRandomCodeBlock();
                if (Block == null)
                {
                    Destroy(gameObject);
                }
            }

            Text.gameObject.SetActive(true);
            Text.text = Block.GetName();
            if (Input.GetKeyDown(KeyCode.E))
            {
                PlayerController.Instance.AddBlock(Block);
                Destroy(gameObject);
            }
        }
        else
        {
            Text.gameObject.SetActive(false);
        }
    }
}