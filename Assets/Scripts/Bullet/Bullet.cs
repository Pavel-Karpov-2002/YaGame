using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private LayerMask _blocks;
    [SerializeField] private LayerMask _enemies;
    [SerializeField] private float _triggerRadius = 0.2f;
    [SerializeField] private float _lifetime = 7;

    public float Speed { get; set; }
    public int Damage { get; set; }
    public LayerMask enemyMask { get; set; }

    private int _minTriggerColliders = 0;

    private void Start()
    {
        Destroy(gameObject, _lifetime);
    }

    private void Update()
    {
        transform.Translate(Vector3.back * Speed * Time.deltaTime);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        bool isEnemy = Physics.OverlapSphere(transform.position, _triggerRadius, _enemies).Length > _minTriggerColliders;

        if (other.GetComponent<Unit>() != null && isEnemy)
        {
            other.GetComponent<Unit>().TakeDamage(Damage);
            Destroy(gameObject);
        }

        bool isBlock = Physics.OverlapSphere(transform.position, _triggerRadius, _blocks).Length > _minTriggerColliders;

        if (isBlock)
            Destroy(gameObject);
    }
}