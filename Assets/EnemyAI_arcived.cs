using UnityEngine;
//sdasdadada
/// <summary>飛ぶ目玉の敵のクラス</summary>
class EnemyAI_arcived : MonoBehaviour
{
    Animator _anim;
    Rigidbody2D _rb2d;
    SpriteRenderer _sr;
    //公開フィールド
    [SerializeField] float _health;
    [SerializeField] float _moveSpeed;
    [SerializeField] EMoveMode _moveMode;
    //AIフィールド
    [SerializeField] float _patrollRadius;
    [SerializeField] float _playerCaptureDistance;
    [SerializeField] float _attackDistance;
    bool _isFound = false;
    /// <summary>ステータス</summary>
    public enum EnemyStat
    {
        Patrol,
        Chasing,
        Death
    }
    /// <summary>移動モード</summary>
    public enum EMoveMode
    {
        Walk,
        Fly
    }
    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        if (_moveMode == EMoveMode.Fly)
            _rb2d.gravityScale = 0;
        else
            _rb2d.gravityScale = 1;
        _rb2d.freezeRotation = true;
        _anim = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
    }
    private void FixedUpdate()
    {
        //死亡判定
        if (_health < 0) Destroy(this.gameObject);
        //各AI処理
        MoveSequence();
        IMGFlipSequence();
    }
    void ActionTakeDamage()
    {
        //アニメーション再生
        _anim.SetTrigger("actTakeHit");
        _health -= 10;//体力の減少処理
        if (_health < 0) { _anim.SetBool("isDeath", true); }
    }
    void ActionAttack1() { _anim.SetTrigger("actAttack1"); }
    void ActionAttack2() { _anim.SetTrigger("actAttack2"); }
    /// <summary>プレイヤーを追跡する</summary>
    void MoveSequence()
    {
        Debug.Log($"{this.gameObject.name}:プレイヤ補足{_isFound}");
        //平常→発見→追跡→攻撃
        //平常ステート処理
        if (!_isFound)//パトロール
        {
            _rb2d.AddForce(
                new Vector2(Random.Range(-_patrollRadius, _patrollRadius),
                Random.Range(-_patrollRadius / 2, _patrollRadius / 2)).normalized,
                ForceMode2D.Force);
            if (Vector2.Distance(this.gameObject.transform.position,
                GameObject.FindGameObjectWithTag("Player").transform.position)
                < _playerCaptureDistance)
            {
                _isFound = true;
            }
            else
            {
                _isFound = false;
            }
        }
        else
        {
            _rb2d.AddForce((GameObject.FindGameObjectWithTag("Player").transform.position
            - this.gameObject.transform.position).normalized,
            ForceMode2D.Force);
            if (Vector2.Distance(this.gameObject.transform.position,
                GameObject.FindGameObjectWithTag("Player").transform.position)
                < _attackDistance)
            {
                ActionAttack1();
                //ActionAttack2();
            }
        }
    }
    /// <summary>画像のフリップ処理</summary>
    void IMGFlipSequence()
    {
        _sr.flipX = 
            (GameObject.FindGameObjectWithTag("Player").transform.position.x
            - this.gameObject.transform.position.x
            < 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerWeapon"))
        {
            Debug.Log("ENEMY:HURT!");
            ActionTakeDamage();
        }
    }
}
