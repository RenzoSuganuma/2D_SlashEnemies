using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
/// <summary>ボスのAI</summary>
public class BossAI_alpha : MonoBehaviour
{
    //Unityコンポーネント
    Animator _anim;
    Rigidbody2D _rb2d;
    SpriteRenderer _sr;
    AudioSource _as;
    //公開フィールド
    /// <summary>体力</summary>
    [SerializeField] float _health;
    /// <summary>最大体力</summary>
    [SerializeField] float _maxHealth;
    /// <summary>移動速度</summary>
    [SerializeField] float _moveSpeed;
    /// <summary>プレイヤー捕捉距離</summary>
    [SerializeField] float _playerCapturedDistance;
    /// <summary>プレイヤー攻撃範囲</summary>
    [SerializeField] float _attackRange;
    /// <summary>呪文攻撃範囲</summary>
    [SerializeField] float _spellRange;
    /// <summary>プレイヤーからのダメージ量</summary>
    [SerializeField] float _damageFromPlayer;
    /// <summary>呪文攻撃上限</summary>
    [SerializeField] int _spellLim;
    /// <summary>呪文攻撃のオブジェクト</summary>
    [SerializeField] GameObject _spellObj;
    /// <summary>死亡時オブジェクト</summary>
    [SerializeField] GameObject _deathObj;
    /// <summary>ワープ地点</summary>
    [SerializeField] Transform[] _warpPos;
    /// <summary>BGMのオーディオソース</summary>
    [SerializeField] AudioSource _bgmSource;
    [SerializeField] AudioClip _attackV;
    [SerializeField] AudioClip _deathV;
    [SerializeField] AudioClip _bossMid;
    [SerializeField] AudioClip _bossEnd;
    public bool _ismiddleHP = false;
    public bool _isendHP = false;
    //非公開フィールド
    /// <summary>プレイヤーオブジェクト</summary>
    GameObject _player;
    /// <summary>捕捉フラグ</summary>
    bool _captured = false;
    /// <summary>攻撃範囲内捕捉フラグ</summary>
    bool _insideRange = false;
    /// <summary>攻撃フラグ</summary>
    bool _attacked = false;
    /// <summary>呪文攻撃フラグ</summary>
    bool _spelled = false;
    /// <summary>呪文攻撃カウント</summary>
    int _spellCnt = 0;
    /// <summary>累積ダメージ量</summary>
    float _totalDamages = 0;
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
    private void Update()
    {
        if (_health < 0)
        {
            DeathBehaviour();
        }
        if (_health < _maxHealth * .7f && !_ismiddleHP)
        {
            _ismiddleHP = true;
            _bgmSource.Stop();
            _bgmSource.clip = _bossMid;
            _bgmSource.Play();
        }
        if (_health < _maxHealth * .3f && !_isendHP)
        {
            _isendHP = true;
            _bgmSource.Stop();
            _bgmSource.clip = _bossEnd;
            _bgmSource.Play();
        }
    }
    /// <summary>死亡時処理</summary>
    void DeathBehaviour()
    {
        //死亡処理
        var obj = Instantiate(_deathObj);
        obj.transform.position = this.transform.position;
        this.gameObject.SetActive(false);
        Destroy(this);
        Destroy(obj, 2);
        _as.PlayOneShot(_deathV);
    }
    /// <summary>プレイヤーの近くへワープする</summary>
    void WarpBehaviour()
    {
        var obj = Instantiate(_deathObj);
        obj.transform.position = this.transform.position;
        this.gameObject.SetActive(false);
        //ワープ処理
        this.gameObject.transform.position = 
            _warpPos[UnityEngine.Random.Range(0, _warpPos.Length)].transform.position;
        this.gameObject.SetActive(true);
        Destroy(obj, 2);
    }
    /// <summary>体力の取得メソッド</summary>
    /// <returns></returns>
    public float GetHP()
    {
        return _health;
    }
    private void FixedUpdate()
    {
        Debug.Log($"{this.gameObject.name} : プレイヤー捕捉");
        //捕捉状態更新
        _captured = Vector2.Distance(_player.transform.position,
        this.transform.position) < _playerCapturedDistance;
        //攻撃範囲内に居るかの判定
        _insideRange = Vector2.Distance(_player.transform.position,
        this.transform.position) < _attackRange && _captured;
        //アニメーター
        _anim.SetBool("Moving", _captured);
        if (_captured)
        {
            //目標へのベクトルの算出
            var tv = (_player.transform.position - this.transform.position).normalized;
            //画像フリップ処理
            _sr.flipX = tv.x > 0;
            //その方向へ向かう
            _rb2d.AddForce(tv * _moveSpeed, ForceMode2D.Force);
        }
        if (_insideRange && !_attacked)
        {
            //攻撃処理
            StartCoroutine(AttackRoutine(3));
        }//攻撃圏内から外れ、魔法の射程内にいるとき
        else if (_captured && Vector2.Distance(_player.transform.position,
            this.transform.position) < _spellRange && !_spelled && _spellCnt < _spellLim)
        {
            //呪文処理
            StartCoroutine(SpellRoutine(3));
        }
    }
    /// <summary>通常攻撃ルーチン</summary>
    /// <param name="t"></param>
    /// <returns></returns>
    IEnumerator AttackRoutine(float t)
    {
        //アニメーター
        _anim.SetTrigger("actAtk");
        _as.PlayOneShot(_attackV);
        Debug.Log($"{this.gameObject.name}攻撃");
        _attacked = true;
        yield return new WaitForSeconds(t);
        _attacked = false;
    }
    /// <summary>呪文攻撃ルーチン</summary>
    /// <param name="t"></param>
    /// <returns></returns>
    IEnumerator SpellRoutine(float t)
    {
        //アニメーター
        _anim.SetTrigger("actSpl");
        Debug.Log($"{this.gameObject.name}呪文");
        _spelled = true;
        //オブジェクト生成
        var so = Instantiate(_spellObj);
        _spellCnt++;
        //破棄
        Destroy(so, 1);
        //座標更新
        so.transform.position = new Vector2(_player.transform.position.x, _player.transform.position.y + 1);
        yield return new WaitForSeconds(t);
        _spelled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //ダメージ処理
        if (collision.CompareTag("PlayerWeapon"))
        {
            //アニメーター
            _anim.SetTrigger("actHrt");
            //ダメージ処理
            _health -= _damageFromPlayer;
            _totalDamages += _damageFromPlayer;
            //累積ダメージが一定水準超えたら
            if (_totalDamages > 50)
            {
                WarpBehaviour();
                _totalDamages = 0;
            }
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
