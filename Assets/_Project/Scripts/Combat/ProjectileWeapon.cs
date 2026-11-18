using UnityEngine;

public class ProjectileWeapon : Weapon
{
    [SerializeField] Projectile projectilePrefabs;

    [SerializeField] float bulletVelocity;

    [SerializeField] Transform muzzle;

    protected override void Fire()
    {
        if (projectilePrefabs == null) return;
        Projectile nProjectile = Instantiate(projectilePrefabs, muzzle.position, Quaternion.identity);
        Vector3 direction = targeter.TargetPoint - muzzle.position;
        nProjectile.Launch(direction.normalized * bulletVelocity, damage);

    }
}

