using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>{XฬAI</summary>
public class BringerofDeath : MonoBehaviour
{
    //UnityR|[lg
    Animator _anim;
    Rigidbody2D _rb2d;
    SpriteRenderer _sr;
    AudioSource _as;
    //๖JtB[h
    [SerializeField] float _health;
    [SerializeField] float _playerCapturedDistance;
    [SerializeField] float _attackRange;
    [SerializeField] float _spellRange;
    [SerializeField] float _damageFromPlayer;
    //๑๖JtB[h
    GameObject _player;
    public bool _captured;
    public bool _insideRange;
    //vC[฿จAา@Xe[g
    //สํUA@U
    //ํUAS
    private void Start()
    {
        //UnityR|[lgฬๆพ
        _anim = GetComponent<Animator>();
        _rb2d = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();   
        _as = GetComponent<AudioSource>();
        //vC[ฬ๕
        _player = GameObject.FindGameObjectWithTag("Player");
    }
    private void FixedUpdate()
    {
        #region ฿จ
        Debug.Log($"{this.gameObject.name} : vC[฿จ");
        //฿จ๓ิXV
        _captured = Vector2.Distance(_player.transform.position,
        this.transform.position) < _playerCapturedDistance;
        //Uออเษ้ฉฬป่
        _insideRange = Vector2.Distance(_player.transform.position,
        this.transform.position) < _attackRange && _captured;
        #endregion
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //_[W
        if (collision.CompareTag("PlayerWeapon"))
        {
            _health -= _damageFromPlayer;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.gameObject.transform.position, _playerCapturedDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.gameObject.transform.position, _attackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.gameObject.transform.position, _spellRange);
    }
}
