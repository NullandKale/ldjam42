using UnityEngine;
using UnityEngine.UI;

public class DestroyAfterDelay : MonoBehaviour
{
    public float timeout;
    public Text text;

    private Vector2 rand()
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
    }

    private void Start()
    {
        GetComponent<Rigidbody2D>().velocity = rand().normalized * 4 * Time.fixedDeltaTime;
        Destroy(gameObject, timeout);
    }
}