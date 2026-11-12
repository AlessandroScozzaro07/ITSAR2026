using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float damage = 5;

    [SerializeField] float speed = 15f;

    

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out Health _health))
        {
            _health.TakeDamage(damage);
        }
        Destroy(this.gameObject);
    }
}
