using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Weapon")]
public class Weapon : ScriptableObject
{
    public GameObject Projectile;

    public float FireRate = 1.0f;
    private float currentFireRate;

    public void Shoot(Transform trans)
    {
        if (Input.GetMouseButton(0))
        {
            if (currentFireRate > FireRate)
            {
                Instantiate(Projectile, trans.position, trans.rotation);
                currentFireRate = 0;
            }
            else
            {
                currentFireRate += 1;
            }
        }
    }
}