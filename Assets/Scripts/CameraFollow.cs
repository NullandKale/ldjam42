using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;
    private Vector3 Origin;

    private void Awake()
    {
        Origin = transform.position;
    }

    private void Update()
    {
        if (Target == null)
        {
            Target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        var pos = Target.position + Origin;

        if (pos.x < -55.5f)
        {
            pos.x = -55.5f;
        }

        if (pos.x > 55.5f)
        {
            pos.x = 55.5f;
        }

        if (pos.y > 58.5f)
        {
            pos.y = 58.5f;
        }

        if (pos.y < -58.5f)
        {
            pos.y = -58.5f;
        }

        transform.position = pos;
    }
}