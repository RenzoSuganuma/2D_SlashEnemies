using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Animator))]
[RequireComponent (typeof(Rigidbody2D))]
/// <summary> 敵キャラのAIのコア </summary>
public class EnemyAI_CORE : MonoBehaviour
{
    Animator _anim;
    Rigidbody2D _rb2d;
    SpriteRenderer _sr;
    AudioSource _as;
    #region AIフィールド
    /// <summary>敵データ</summary>
    [SerializeField] EnemyDataContainer _eData;
    [SerializeField] Slider _hpBar;
    /// <summary>プレイヤーオブジェクト</summary>
    GameObject _player;
    /// <summary>体力</summary>
    float _hp;
    /// <summary>移動速度</summary>
    float _ms;
    /// <summary>巡回の半径</summary>
    float _patrollRadius;
    /// <summary>プレイヤー捕捉距離</summary>
    float _playerCaptureDistance;
    /// <summary>攻撃範囲</summary>
    float _attackDistance;
    /// <summary>ダメージ量</summary>
    float _dp;
    /// <summary>光線の長さ</summary>
    int _rayLength;
    /// <summary>ぶつかったときにリパスするOBJのレイヤー</summary>
    int _repathLayer;
    /// <summary>キルした時のスコア</summary>
    int _killScore;
    /// <summary>敵AIの移動モード</summary>
    MoveMode _moveMode;
    /// <summary>攻撃時の声</summary>
    AudioClip _attackV;
    /// <summary>死亡時の声</summary>
    AudioClip _deathV;
    /// <summary>目標の座標への”ベクトル”</summary>
    Vector3 _targetPath = Vector3.zero;
    /// <summary>現状のプレイヤー捕捉フラグ</summary>
    bool _isPlayerFound = false;
    /// <summary>以前のプレイヤー捕捉フラグ</summary>
    bool _pisPlayerFound = false;
    /// <summary>攻撃をしたかのフラグ</summary>
    bool _attacked = false;
    #endregion
    #region デリゲート
    /// <summary>ダメージをくらった時に呼び出されるデリゲート</summary>
    public Action<Animator> damagedEvent;
    /// <summary>死んだときに呼び出されるデリゲート</summary>
    public Action<Animator> deathEvent;
    /// <summary>プレイヤー捕捉時に呼び出されるデリゲート</summary>
    public Action<Animator> playerCapturedEvent;
    /// <summary>プレイヤーを見失った時に飛び出されるデリゲート</summary>
    public Action<Animator> playerMissedEvent;
    /// <summary>プレイヤー攻撃時に呼び出されるデリゲート</summary>
    public Action<Animator> attackingEvent;
    #endregion
    /// <summary>移動モード</summary>
    public enum MoveMode
    {
        Walk,
        Fly
    }
    private void Start()
    {
        //敵データの読み込み
        GetDataAndSetFromContainer();
        //体力ゲージの最大値設定
        _hpBar.maxValue = _hp;
        //コンポーネント取得
        _sr = GetComponent<SpriteRenderer>();
        _rb2d = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _as = GetComponent<AudioSource>();
        //移動モードによって重力スケールが変動
        _rb2d.gravityScale = (_moveMode == MoveMode.Fly) ? 0 : 1;
        _rb2d.freezeRotation = true;
        //目標座標の設定[乱数]
        RePath();
        //RayCastの影響を受けないようにする
        this.gameObject.layer = 2;
        //プレイヤーの検索
        _player = GameObject.FindGameObjectWithTag("Player");
    }
    private void FixedUpdate()
    {
        Debug.Log($"目標補足フラグ{_pisPlayerFound}");
        //体力表示更新
        _hpBar.value = (_hp / _hpBar.maxValue) * _hpBar.maxValue;
        //フリップ処理
        _sr.flipX = _targetPath.x < 0;
        //死亡判定
        if (_hp <= 0)
        {
            //デリゲート呼び出し
            deathEvent(_anim);
            //すり抜けるようにする
            this.GetComponent<Rigidbody2D>().simulated = false;
            this.GetComponent<Collider2D>().isTrigger = true;
        }
        //パトロール→発見→追跡→攻撃
        //パトロール↓
        //１．進む先の位置の確定
        //２．その位置についたらまた別の位置に行く
        //３．パトロール中にプレイヤーを捕捉すればそこに行く
        //４．捕捉して、攻撃圏内にプレイヤーがいれば攻撃
        /* = 処理本体 = */
        //パトロール処理
        #region プレイヤー未捕捉時
        //プレイヤー未捕捉時
        if (!_isPlayerFound)
        {
            Debug.Log($"目標座標{_targetPath}");
            //光線のキャスト
            Ray2D ray = new Ray2D(this.gameObject.transform.position, _targetPath.normalized * _rayLength);
            RaycastHit2D hit2d = Physics2D.Raycast(ray.origin, ray.direction, 1 * _rayLength);
            Debug.DrawRay(ray.origin, ray.direction, Color.green);
            if (hit2d.collider)
            {
                Debug.Log($"レイヤー：{hit2d.collider.gameObject.layer}");
                if (hit2d.collider.gameObject.layer == _repathLayer)
                {
                    RePath();
                    Debug.Log($"目標座標更新{hit2d.collider.gameObject.name}");
                }
            }
            _rb2d.AddForce(_targetPath.normalized * _ms, ForceMode2D.Force);
        }
        else
        {
            _rb2d.AddForce(_targetPath.normalized * _ms * 2, ForceMode2D.Force);
        }
        #endregion
        #region プレイヤー発見処理
        //プレイヤー発見処理
        if (Vector2.Distance(this.gameObject.transform.position,
            _player.transform.position) < _playerCaptureDistance)
        {
            _isPlayerFound = true;
            //デリゲート呼び出し
            if (_isPlayerFound && !_pisPlayerFound)
            {
                playerCapturedEvent(_anim);
                _pisPlayerFound = true;
            }
            var this2player = _player.transform.position - this.gameObject.transform.position;
            //移動モードによってベクトルを変更
            switch (_moveMode)
            {
                case MoveMode.Walk:
                    {
                        _targetPath = new Vector2(this2player.x, 0);
                        break;
                    }
                case MoveMode.Fly:
                    {
                        _targetPath = this2player;
                        break;
                    }
            }
        }
        else
        {
            _isPlayerFound = false;
            //デリゲート呼び出し
            if (!_isPlayerFound && _pisPlayerFound)
            {
                playerMissedEvent(_anim);
                _pisPlayerFound = false;
            }
        }
        //攻撃処理
        if (_isPlayerFound && Vector2.Distance(this.gameObject.transform.position,
            _player.transform.position) < _attackDistance && !_attacked)
        {
            //デリゲート呼び出し
            attackingEvent(_anim);
            //効果音
            _as.PlayOneShot(_attackV);
            //フラグを立てる
            _attacked = true;
            StartCoroutine(WaitForEndOfAttack(3));
        }
        #endregion
    }
    /// <summary>死亡時の効果音再生</summary>
    public void PlayDeathVoice()
    {
        //効果音再生
        _as.PlayOneShot(_deathV);
    }
    /// <summary>死亡時に派生クラスから呼び出される</summary>
    public void AddPlayerScore()
    {
        //ゲームマネージャーへスコア申請
        GameObject.FindAnyObjectByType<GameManager>().AddScore(_killScore);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //↓歩行移動モード選択時には光線が当たった時のリパスで十分。これは飛行移動モード時壁に当たった時のリパス処理
        if (collision.gameObject.layer == _repathLayer && !_isPlayerFound && _moveMode == MoveMode.Fly)
        {
            RePath();
        }
        if (collision.gameObject.CompareTag("Monster"))
        {
            RePath();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //ダメージ処理
        if (collision.gameObject.CompareTag("PlayerWeapon"))
        {
            //デリゲート呼び出し
            damagedEvent(_anim);
            _hp -= _dp;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.gameObject.transform.position, _playerCaptureDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.gameObject.transform.position, _attackDistance);
    }
    /// <summary>目標座標の更新[乱数でとる]</summary>
    private void RePath()
    {
        Debug.Log("目標更新");
        //移動モードによって目標座標更新処理を分ける
        switch (_moveMode)
        {
            case MoveMode.Fly:
                {
                    _targetPath = new Vector2(
                                UnityEngine.Random.Range(-_patrollRadius, _patrollRadius),
                                UnityEngine.Random.Range(-_patrollRadius, _patrollRadius)
                                );
                    break;
                }
            case MoveMode.Walk:
                {
                    _targetPath = new Vector2(
                                    UnityEngine.Random.Range(-_patrollRadius, _patrollRadius),
                                    0
                                    );
                    break;
                }
        }
    }
    /// <summary>データコンテナからのデータで初期化</summary>
    void GetDataAndSetFromContainer()
    {
        _hp = _eData._hp;
        _ms = _eData._ms;
        _patrollRadius = _eData._patrollRadius;
        _playerCaptureDistance = _eData._playerCaptureDistance;
        _attackDistance = _eData._attackDistance;
        _dp = _eData._dp;
        _rayLength = _eData._rayLength;
        _repathLayer = _eData._repathLayer;
        _killScore = _eData._killScore;
        _moveMode = _eData._moveMode;
        _attackV = _eData._attackV;
        _deathV = _eData._deathV;
    }
    /// <summary>攻撃インターバルコルーチン。攻撃フラグをfalseにしてフラグを倒す</summary>
    /// <param name="s"></param>
    /// <returns></returns>
    IEnumerator WaitForEndOfAttack(float s)
    {
        yield return new WaitForSeconds(s);
        _attacked = false;
    }
}
