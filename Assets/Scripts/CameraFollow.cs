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

        transform.position = Target.position + Origin;
    }
}