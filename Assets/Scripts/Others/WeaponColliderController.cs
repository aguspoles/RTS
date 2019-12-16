using UnityEngine;

public delegate void OnWeaponCollision();

public class WeaponColliderController : MonoBehaviour
{
    public event OnWeaponCollision onWeaponCollisionEvent;

    private void Awake()
    {
        
    }

    private void OnCollisionEnter(Collision _collision)
    {
        
    }
}












