using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private WeaponParameters _weaponParameters;
    [SerializeField] private GameObject _posAttack;

    public WeaponParameters WeaponParameters => _weaponParameters;
    public GameObject PosAttack => _posAttack;
    public float TimerBulletDelay { get; protected set; }

    public virtual void Attack()
    {
        Shot();
    }
    
    public void Shot()
    {
        try
        {
            if (_weaponParameters.Bullet == null)
                return;

            Bullet newBullet = Instantiate(_weaponParameters.Bullet, _posAttack.transform.position, _posAttack.transform.rotation);
            newBullet.Speed = _weaponParameters.AttackSpeed;
            newBullet.Damage = _weaponParameters.AttackDamage;

            TimerBulletDelay = _weaponParameters.AttackDelay;
            _audioSource.PlayOneShot(_audioSource.clip);
        }
        catch
        {
            Debug.Log("Bullet is null.");
            return;
        }
    }
}
