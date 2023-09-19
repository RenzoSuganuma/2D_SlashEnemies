using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>ボスのAI</summary>
public class BringerofDeath : MonoBehaviour
{
    //Unityコンポーネント
    Animator _anim;
    Rigidbody2D _rb2d;
    SpriteRenderer _sr;
    AudioSource _as;
    //公開フィールド
    [SerializeField] float _health;
    [SerializeField] float _playerCapturedDistance;
    [SerializeField] float _attackRange;
    [SerializeField] float _spellRange;
    [SerializeField] float _damageFromPlayer;
    //非公開フィールド
    GameObject _player;
    public bool _captured;
    public bool _insideRange;
    //プレイヤー捕捉、待機ステート
    //通常攻撃、魔法攻撃
    //被攻撃、死亡
    private void Start()
    {
        //Unityコンポーネントの取得
        _anim = GetComponent<Animator>();
        _rb2d = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();   
        _as = GetComponent<AudioSource>();
        //プレイヤーの検索
        _player = GameObject.FindGameObjectWithTag("Player");
    }
    private void FixedUpdate()
    {
        #region 捕捉処理
        Debug.Log($"{this.gameObject.name} : プレイヤー捕捉");
        //捕捉状態更新
        _captured = Vector2.Distance(_player.transform.position,
        this.transform.position) < _playerCapturedDistance;
        //攻撃範囲内に居るかの判定
        _insideRange = Vector2.Distance(_player.transform.position,
        this.transform.position) < _attackRange && _captured;
        #endregion
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //ダメージ処理
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
