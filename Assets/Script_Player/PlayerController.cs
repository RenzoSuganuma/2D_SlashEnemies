using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// �v���C���[�̕����I�����Ɠ���̃I�u�W�F�N�g����N���X
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour, PlayerInputs.IPlayerActions
{
    //�e�f�t�H���g�R���|�[�l���g
    /// <summary>���������ɕK�v</summary>
    Rigidbody2D _rb2d = null;
    /// <summary>�摜�o�͂ɕK�v</summary>
    SpriteRenderer _sr = null;
    /// <summary>�A�j���[�V��������ɕK�v</summary>
    Animator _anim = null;
    /// <summary>�f�o�C�X���͎󂯎��ɕK�v</summary>
    PlayerInput _input = null;
    /// <summary>�A�j���[�V��������ɕK�v�ȃA�j���[�V��������N���X</summary>
    PlayerMotionController _mc = null;
    /// <summary>�Q�[���}�l�[�W���[</summary>
    GameManager _gm = null;
    //�e���͒l�i�[�ϐ�
    /// <summary>�ړ����͒l</summary>
    Vector2 _iMove = Vector2.zero;
    /// <summary>���_�ړ����͒l</summary>
    Vector2 _iLook = Vector2.zero;
    /// <summary>�v���C���[���W�����v���Ă��鎞�̃t���O</summary>
    bool _isJumping = false;
    /// <summary>�ڒn����t���O</summary>
    bool _isGrounded = false;
    /// <summary>�ǒ���t���t���O</summary>
    bool _isGrabbingWall = false;
    /// <summary>���G���[�h�̃t���O</summary>
    bool _isGodMode = false;
    /// <summary>�v���C���[�ɂ������Ă��郔�F���V�e�B</summary>
    Vector2 _pVel = Vector2.zero;
    /// <summary>�ړ����x�l</summary>
    [SerializeField] float _playerMoveSpeed;
    /// <summary>�W�����v�͒l</summary>
    [SerializeField] float _playerJumpForce;
    /// <summary>�W�����v�͒l</summary>
    [SerializeField] float _playerDashForce;
    /// <summary>�f�t�H���g�̃}�e���A��</summary>
    [SerializeField] Material _pDefaultMat;
    /// <summary>���G���[�h�̃}�e���A��</summary>
    [SerializeField] Material _pGodMat;
    private void Awake()
    {
        //�f�o�C�X���̓v���o�C�_�[���擾
        _input = GetComponent<PlayerInput>();
    }
    private void Start()
    {
        //�e�R���|�[�l���g�̎擾
        _rb2d = GetComponent<Rigidbody2D>();
        //���Ӗ��ȉ�]���֎~
        _rb2d.freezeRotation = true;
        _sr = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        //�Q�[���}�l�[�W���[���擾
        _gm = FindAnyObjectByType<GameManager>();
        //�A�j���[�V��������N���X�̎��̉�
        _mc = new PlayerMotionController(_anim);
        //���G���[�h�N��
        StartCoroutine(Godmode(3));
    }
    private void OnEnable()
    {
        //�f���Q�[�g�o�^[�f�o�C�X����]
        _input.onActionTriggered += OnDash;
        _input.onActionTriggered += OnFire;
        _input.onActionTriggered += OnJump;
        _input.onActionTriggered += OnLook;
        _input.onActionTriggered += OnMove;
        _input.onActionTriggered += OnPause;
    }
    private void OnDisable()
    {
        //�f���Q�[�g�o�^����[�f�o�C�X����]
        _input.onActionTriggered -= OnDash;
        _input.onActionTriggered -= OnFire;
        _input.onActionTriggered -= OnJump;
        _input.onActionTriggered -= OnLook;
        _input.onActionTriggered -= OnMove;
        _input.onActionTriggered -= OnPause;
    }
    private void FixedUpdate()
    {
        PlayerMoveSequence();
        PlayerJumpSequence();
    }
    private void PlayerMoveSequence()
    {
        Debug.Log($"PAST VEL:{_pVel}");
        //�����I�Ɉړ�����
        if (!_isGrabbingWall && _isGrounded && _iMove != Vector2.zero)
        {
            _rb2d.AddForce(_iMove.normalized * _playerMoveSpeed, ForceMode2D.Force);
            _pVel = _iMove.normalized * _playerMoveSpeed;
        }
    }
    /// <summary>�A�j���[�V�����C�x���g�ŌĂяo�������</summary>
    private void PlayerJumpSequence()
    {
        //�����I�ɑł��グ��
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
    /// <summary>���G���[�h</summary>
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
        //�ڒn����
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
            _isGrabbingWall = !_isGrounded;
            _mc.SetGroundedCondition(_isGrounded);
            this.gameObject.transform.parent = null;
        }
        //�ǒ���t������
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
        //�ǒ���t������
        if (collision.gameObject.CompareTag("Wall"))
        {
            this.gameObject.transform.parent = collision.gameObject.transform;
            if (!_isGrounded)
                _isGrabbingWall = true;
            _anim.SetBool("isGrabingWall", true);
        }
        //�ڒn����
        if (collision.gameObject.CompareTag("Ground"))
        {
            this.gameObject.transform.parent = null;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        //�ڒn����
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = false;
            _mc.SetGroundedCondition(_isGrounded);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�ǒ���t���摜�t���b�v����
        if (collision.gameObject.CompareTag("WallL"))
        {
            _sr.flipX = true;
        }
        else if (collision.gameObject.CompareTag("WallR"))
        {
            _sr.flipX = false;
        }
        //�_���[�W���� [���G���[�h����Ȃ��Ƃ�]
        if (collision.gameObject.CompareTag("Damager") && !_isGodMode)
        {
            Debug.Log("�_���[�W");
            //�̗͂̍X�V
            _gm.ModifyHealth(-10);
            //�m�b�N�o�b�N����
            var v = (collision.gameObject.transform.position - this.gameObject.transform.position).normalized;
            Vector2 damageVec = new Vector2(v.x, 0);
            _rb2d.AddForce(-damageVec, ForceMode2D.Impulse);
            Debug.Log($"SIGN:{-damageVec}");
            //�A�j���[�V��������
            _mc.ActionHurt();
        }
    }
    #region �f�o�C�X����
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
            //�A�j���[�V�����Đ�
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
            //�A�j���[�V�����̍Đ�
            _mc.ActionJump();
            //�W�����s���O�t���O���X�V
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
            //�A�j���[�V�����̃R���f�B�V����������
            _mc.SetMoveCondition(context.ReadValue<Vector2>() != Vector2.zero);
            //�摜�̃t���b�v����
            if (context.ReadValue<Vector2>().x > 0 && !_isGrabbingWall) _sr.flipX = false;
            else if (context.ReadValue<Vector2>().x != 0 && !_isGrabbingWall) _sr.flipX = true;
            //���͒l�̑��
            _iMove = new Vector2(context.ReadValue<Vector2>().x, 0);
        }
    }
    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.action.name == "Pause")
        {
            if (context.ReadValueAsButton())
            {
                Debug.Log("�ꎞ��~");
                GameObject.FindAnyObjectByType<GameManager>().Pause();
            }
        }
    }
    #endregion
}