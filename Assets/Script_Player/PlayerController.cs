using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
/// <summary>
/// プレイヤーの物理的挙動と特定のオブジェクト操作クラス
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour, PlayerInputs.IPlayerActions
{
    //各デフォルトコンポーネント
    /// <summary>物理挙動に必要</summary>
    Rigidbody2D _rb2d = null;
    /// <summary>画像出力に必要</summary>
    SpriteRenderer _sr = null;
    /// <summary>アニメーション操作に必要</summary>
    Animator _anim = null;
    /// <summary>デバイス入力受け取りに必要</summary>
    PlayerInput _input = null;
    /// <summary>アニメーション操作に必要なアニメーション操作クラス</summary>
    PlayerMotionController _mc = null;
    /// <summary>ゲームマネージャー</summary>
    GameManager _gm = null;
    //各入力値格納変数
    /// <summary>移動入力値</summary>
    Vector2 _iMove = Vector2.zero;
    /// <summary>視点移動入力値</summary>
    Vector2 _iLook = Vector2.zero;
    /// <summary>プレイヤーがジャンプしている時のフラグ</summary>
    bool _isJumping = false;
    /// <summary>接地判定フラグ</summary>
    bool _isGrounded = false;
    /// <summary>壁張り付きフラグ</summary>
    bool _isGrabbingWall = false;
    /// <summary>無敵モードのフラグ</summary>
    bool _isGodMode = false;
    /// <summary>プレイヤーにかかっているヴェロシティ</summary>
    Vector2 _pVel = Vector2.zero;
    /// <summary>移動速度値</summary>
    [SerializeField] float _playerMoveSpeed;
    /// <summary>ジャンプ力値</summary>
    [SerializeField] float _playerJumpForce;
    /// <summary>ジャンプ力値</summary>
    [SerializeField] float _playerDashForce;
    /// <summary>デフォルトのマテリアル</summary>
    [SerializeField] Material _pDefaultMat;
    /// <summary>無敵モードのマテリアル</summary>
    [SerializeField] Material _pGodMat;
    /// <summary>月牙天衝のオブジェクト</summary>
    [SerializeField] GameObject _getsuga;
    /// <summary>月牙天衝の上限</summary>
    [SerializeField] int _getsugaLim;
    /// <summary>月牙天衝の残数テキスト</summary>
    [SerializeField] Text _getsugaText;
    /// <summary>月牙天衝の残数</summary>
    int _getsugaCnt;

    private void Awake()
    {
        //デバイス入力プロバイダーを取得
        _input = GetComponent<PlayerInput>();
    }
    private void Start()
    {
        //各コンポーネントの取得
        _rb2d = GetComponent<Rigidbody2D>();
        //無意味な回転を禁止
        _rb2d.freezeRotation = true;
        _sr = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        //ゲームマネージャーを取得
        _gm = FindAnyObjectByType<GameManager>();
        //アニメーション操作クラスの実体化
        _mc = new PlayerMotionController(_anim);
        //無敵モード起動
        StartCoroutine(Godmode(3));
        //月牙天衝の残数の初期化
        _getsugaCnt = _getsugaLim * 2;
    }
    private void OnEnable()
    {
        //デリゲート登録[デバイス入力]
        _input.onActionTriggered += OnDash;
        _input.onActionTriggered += OnFire;
        _input.onActionTriggered += OnJump;
        _input.onActionTriggered += OnLook;
        _input.onActionTriggered += OnMove;
        _input.onActionTriggered += OnPause;
        _input.onActionTriggered += OnAim;
    }
    private void OnDisable()
    {
        //デリゲート登録解除[デバイス入力]
        _input.onActionTriggered -= OnDash;
        _input.onActionTriggered -= OnFire;
        _input.onActionTriggered -= OnJump;
        _input.onActionTriggered -= OnLook;
        _input.onActionTriggered -= OnMove;
        _input.onActionTriggered -= OnPause;
        _input.onActionTriggered -= OnAim;
    }
    private void FixedUpdate()
    {
        PlayerMoveSequence();
        PlayerJumpSequence();
        //月牙天衝残数表示
        _getsugaText.text = (_getsugaCnt / 2).ToString();
    }
    private void PlayerMoveSequence()
    {
        Debug.Log($"PAST VEL:{_pVel}");
        //物理的に移動処理
        if (!_isGrabbingWall && _isGrounded && _iMove != Vector2.zero)
        {
            _rb2d.AddForce(_iMove.normalized * _playerMoveSpeed, ForceMode2D.Force);
            _pVel = _iMove.normalized * _playerMoveSpeed;
        }
    }
    /// <summary>アニメーションイベントで呼び出しされる</summary>
    private void PlayerJumpSequence()
    {
        //物理的に打ち上げる
        if (_isGrounded && _isJumping || _isGrabbingWall && _isJumping)
        {
            if(!_isGrabbingWall)
            _rb2d.AddForce((transform.up.normalized + new Vector3(_iMove.x,0, 0).normalized) * _playerJumpForce, ForceMode2D.Impulse);
            else
                _rb2d.AddForce((transform.up.normalized + new Vector3(_iMove.x, _iMove.y, 0).normalized) * _playerJumpForce * 1.5f, ForceMode2D.Impulse);
            this.gameObject.transform.parent = null;
            _anim.SetBool("isGrabingWall", false);
            _isGrabbingWall = false;
        }
    }
    /// <summary>無敵モード</summary>
    /// <param name="t"></param>
    /// <returns></returns>
    IEnumerator Godmode(float t)
    {
        _isGodMode = true;
        _sr.material = _pGodMat;
        yield return new WaitForSeconds(t);
        _isGodMode = false;
        _sr.material = _pDefaultMat;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //接地判定
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
            _isGrabbingWall = !_isGrounded;
            _mc.SetGroundedCondition(_isGrounded);
            this.gameObject.transform.parent = null;
        }
        //壁張り付き処理
        if (collision.gameObject.CompareTag("Wall"))
        {
            this.gameObject.transform.parent = collision.gameObject.transform;
            if (!_isGrounded)
                _isGrabbingWall = true;
            _anim.SetBool("isGrabingWall", true);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        //壁張り付き処理
        if (collision.gameObject.CompareTag("Wall"))
        {
            this.gameObject.transform.parent = collision.gameObject.transform;
            if (!_isGrounded)
                _isGrabbingWall = true;
            _anim.SetBool("isGrabingWall", true);
        }
        //接地判定
        if (collision.gameObject.CompareTag("Ground"))
        {
            this.gameObject.transform.parent = null;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        //接地判定
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = false;
            _mc.SetGroundedCondition(_isGrounded);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //壁張り付き画像フリップ処理
        if (collision.gameObject.CompareTag("WallL"))
        {
            _sr.flipX = true;
        }
        else if (collision.gameObject.CompareTag("WallR"))
        {
            _sr.flipX = false;
        }
        //ダメージ判定 [無敵モードじゃないとき]
        if (collision.gameObject.CompareTag("Damager") && !_isGodMode)
        {
            Debug.Log("ダメージ");
            //体力の更新
            _gm.ModifyHealth(UnityEngine.Random.Range(-30,-10));
            //ノックバック処理
            var v = (collision.gameObject.transform.position - this.gameObject.transform.position).normalized;
            Vector2 damageVec = new Vector2(v.x, 0);
            _rb2d.AddForce(-damageVec, ForceMode2D.Impulse);
            Debug.Log($"SIGN:{-damageVec}");
            //アニメーション処理
            _mc.ActionHurt();
        }
    }
    #region デバイス入力
    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.action.name == "Dash")
        {
            Debug.Log("Dash");
        }
    }
    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.action.name == "Fire")
        {
            Debug.Log("Fire");
            //アニメーション再生
            if (!_isGrounded)
                _mc.ActionJumpAttack();
            else _mc.ActionAttack();
        }
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.action.name == "Jump")
        {
            Debug.Log("Jump");
            //アニメーションの再生
            _mc.ActionJump();
            //ジャンピングフラグを更新
            _isJumping = context.ReadValueAsButton();
        }
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        if (context.action.name == "Look")
        {
            Debug.Log("Look");
            _iLook = context.ReadValue<Vector2>();
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.action.name == "Move")
        {
            Debug.Log("Move");
            //アニメーションのコンディション初期化
            _mc.SetMoveCondition(context.ReadValue<Vector2>() != Vector2.zero);
            //画像のフリップ処理
            if (context.ReadValue<Vector2>().x > 0 && !_isGrabbingWall) _sr.flipX = false;
            else if (context.ReadValue<Vector2>().x != 0 && !_isGrabbingWall) _sr.flipX = true;
            //入力値の代入
            _iMove = new Vector2(context.ReadValue<Vector2>().x, 0);
        }
    }
    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.action.name == "Pause")
        {
            if (context.ReadValueAsButton())
            {
                Debug.Log("一時停止");
                GameObject.FindAnyObjectByType<GameManager>().Pause();
            }
        }
    }
    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.action.name == "Aim")
        {
            if (context.ReadValueAsButton() && 0 < _getsugaCnt)
            {
                Debug.Log("シフト攻撃");
                _mc.ActionShiftAttack();
                //オブジェクトの生成
                var go = Instantiate(_getsuga);
                var component = go.GetComponent<GetsugaController>();
                go.transform.position = this.transform.position;
                //目標ベクトルの初期化
                component._targetv = new Vector2(!_sr.flipX ? 1 : -1, 0);
                _getsugaCnt--;
            }
        }
    }
    #endregion
}