using UnityEngine;

public class WeaponComponent : MonoBehaviour
{
    public float weaponActiveTime;

    WeaponColliderController weaponCollider;
    MeleeWeaponTrail weaponTrail;

    float weaponActiveTimer;

    private void Start()
    {
        weaponCollider = GetComponentInChildren<WeaponColliderController>();
        weaponTrail = GetComponentInChildren<MeleeWeaponTrail>();


        if (weaponCollider != null)
        {
            weaponCollider.onWeaponCollisionEvent += OnWeaponCollision;
        }
    }

    private void Update()
    {
        if (weaponActiveTimer > 0)
        {
            if (!weaponTrail._emit)
            {
                weaponTrail._emit = true;
            }
            weaponActiveTimer -= Time.deltaTime;
        }
        else if (weaponTrail._emit)
        {
            weaponTrail._emit = false;
        }
    }

    public void SetWeaponActive()
    {
        weaponActiveTimer = weaponActiveTime;
    }

    public void OnWeaponCollision()
    {
        if (weaponActiveTimer > 0)
        {

        }
    }

    private void OnDestroy()
    {
        if (weaponCollider != null)
        {
            weaponCollider.onWeaponCollisionEvent -= OnWeaponCollision;
        }
    }
}
