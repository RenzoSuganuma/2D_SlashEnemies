using System;
using UnityEditor;
using UnityEngine;
[RequireComponent (typeof(Animator))]
[RequireComponent (typeof(Rigidbody2D))]
/// <summary> 敵キャラのAIのコア </summary>
public class EnemyAI_CORE : MonoBehaviour
{
    Rigidbody2D _rb2d;
    SpriteRenderer _sr;
    //公開フィールド
    /// <summary>体力</summary>
    [SerializeField] float _hp;
    /// <summary>移動速度</summary>
    [SerializeField] float _ms;
    //AIフィールド
    /// <summary>巡回の半径</summary>
    [SerializeField] float _patrollRadius;
    /// <summary>プレイヤー捕捉距離</summary>
    [SerializeField] float _playerCaptureDistance;
    /// <summary>攻撃範囲</summary>
    [SerializeField] float _attackDistance;
    /// <summary>ぶつかったときにリパスするOBJのレイヤー</summary>
    [SerializeField] int _repathLayer;
    /// <summary>敵AIの移動モード</summary>
    [SerializeField] MoveMode _moveMode;
    /// <summary>目標の座標への”ベクトル”</summary>
    Vector3 _targetPath = Vector3.zero;
    /// <summary>現状のプレイヤー捕捉フラグ</summary>
    public bool _isPlayerFound = false;
    /// <summary>以前のプレイヤー捕捉フラグ</summary>
    bool _pisPlayerFound = false;
    //デリゲート
    /// <summary>ダメージをくらった時に呼び出されるデリゲート</summary>
    public Action damagedEvent = () => Debug.Log("DAMAGED");
    /// <summary>死んだときに呼び出されるデリゲート</summary>
    public Action deathEvent = () => Debug.Log("DEATH");
    /// <summary>プレイヤー捕捉時に呼び出されるデリゲート</summary>
    public Action playerCapturedEvent = () => Debug.Log("CAPTURED");
    /// <summary>プレイヤーを見失った時に飛び出されるデリゲート</summary>
    public Action playerMissedEvent = () => Debug.Log("MISSED");
    /// <summary>プレイヤー攻撃時に呼び出されるデリゲート</summary>
    public Action attackingEvent = () => Debug.Log("ATTACK");
    /// <summary>移動モード</summary>
    public enum MoveMode
    {
        Walk,
        Fly
    }
    private void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        _rb2d = GetComponent<Rigidbody2D>();
        //移動モードによって重力スケールが変動
        _rb2d.gravityScale = (_moveMode == MoveMode.Fly) ? 0 : 1;
        _rb2d.freezeRotation = true;
        //目標座標の設定[乱数]
        RePath();
        //RayCastの影響を受けないようにする
        this.gameObject.layer = 2;
    }
    private void FixedUpdate()
    {
        Debug.Log($"目標補足フラグ{_pisPlayerFound}");
        //フリップ処理
        _sr.flipX = _targetPath.x < 0;
        //死亡判定
        if (_hp <= 0)
        {
            //デリゲート呼び出し
            deathEvent();
            //破棄
            Destroy(this.gameObject);
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
            Ray2D ray = new Ray2D(this.gameObject.transform.position, _targetPath.normalized);
            RaycastHit2D hit2d = Physics2D.Raycast(ray.origin, ray.direction, 1);
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
            GameObject.FindGameObjectWithTag("Player").transform.position) < _playerCaptureDistance)
        {
            _isPlayerFound = true;
            //デリゲート呼び出し
            if (_isPlayerFound && !_pisPlayerFound)
            {
                playerCapturedEvent();
                _pisPlayerFound = true;
            }
            _sr.color = Color.red;
            var this2player = GameObject.FindGameObjectWithTag("Player").transform.position - this.gameObject.transform.position;
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
            _sr.color = Color.white;
            _isPlayerFound = false;
            //デリゲート呼び出し
            if (!_isPlayerFound && _pisPlayerFound)
            {
                playerMissedEvent();
                _pisPlayerFound = false;
            }
        }
        //攻撃処理
        if (_isPlayerFound && Vector2.Distance(this.gameObject.transform.position,
            GameObject.FindGameObjectWithTag("Player").transform.position) < _attackDistance)
        {
            //デリゲート呼び出し
            attackingEvent();
        }
        #endregion
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //↓歩行移動モード選択時には光線が当たった時のリパスで十分。これは飛行移動モード時壁に当たった時のリパス処理
        if (collision.gameObject.layer == _repathLayer && !_isPlayerFound && _moveMode == MoveMode.Fly)
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
            damagedEvent();
            _hp -= 10f;
        }
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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.gameObject.transform.position, _playerCaptureDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.gameObject.transform.position, _attackDistance);
    }
}
