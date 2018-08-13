using UnityEngine;

public class Options : MonoBehaviour
{
    public static bool Music = false;
    public static bool Effects = false;

    private void Awake()
    {
        if (FindObjectsOfType<Options>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}