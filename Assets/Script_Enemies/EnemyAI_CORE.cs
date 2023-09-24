using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent (typeof(Animator))]
[RequireComponent (typeof(Rigidbody2D))]
/// <summary> �G�L������AI�̃R�A </summary>
public class EnemyAI_CORE : MonoBehaviour
{
    Animator _anim;
    Rigidbody2D _rb2d;
    SpriteRenderer _sr;
    AudioSource _as;
    #region AI�t�B�[���h
    /// <summary>�G�f�[�^</summary>
    [SerializeField] EnemyDataContainer _eData;
    [SerializeField] Slider _hpBar;
    /// <summary>�v���C���[�I�u�W�F�N�g</summary>
    GameObject _player;
    /// <summary>�̗�</summary>
    float _hp;
    /// <summary>�ړ����x</summary>
    float _ms;
    /// <summary>����̔��a</summary>
    float _patrollRadius;
    /// <summary>�v���C���[�ߑ�����</summary>
    float _playerCaptureDistance;
    /// <summary>�U���͈�</summary>
    float _attackDistance;
    /// <summary>�_���[�W��</summary>
    float _dp;
    /// <summary>�����̒���</summary>
    int _rayLength;
    /// <summary>�Ԃ������Ƃ��Ƀ��p�X����OBJ�̃��C���[</summary>
    int _repathLayer;
    /// <summary>�L���������̃X�R�A</summary>
    int _killScore;
    /// <summary>�GAI�̈ړ����[�h</summary>
    MoveMode _moveMode;
    /// <summary>�U�����̐�</summary>
    AudioClip _attackV;
    /// <summary>���S���̐�</summary>
    AudioClip _deathV;
    /// <summary>�ڕW�̍��W�ւ́h�x�N�g���h</summary>
    Vector3 _targetPath = Vector3.zero;
    /// <summary>����̃v���C���[�ߑ��t���O</summary>
    bool _isPlayerFound = false;
    /// <summary>�ȑO�̃v���C���[�ߑ��t���O</summary>
    bool _pisPlayerFound = false;
    /// <summary>�U�����������̃t���O</summary>
    bool _attacked = false;
    /// <summary>�Q�[���������������Ă��邩�̃t���O</summary>
    bool _isGameRunning = false;
    GameManager _gm;
    #endregion
    #region �f���Q�[�g
    /// <summary>�_���[�W������������ɌĂяo�����f���Q�[�g</summary>
    public Action<Animator> damagedEvent;
    /// <summary>���񂾂Ƃ��ɌĂяo�����f���Q�[�g</summary>
    public Action<Animator> deathEvent;
    /// <summary>�v���C���[�ߑ����ɌĂяo�����f���Q�[�g</summary>
    public Action<Animator> playerCapturedEvent;
    /// <summary>�v���C���[�������������ɔ�яo�����f���Q�[�g</summary>
    public Action<Animator> playerMissedEvent;
    /// <summary>�v���C���[�U�����ɌĂяo�����f���Q�[�g</summary>
    public Action<Animator> attackingEvent;
    #endregion
    /// <summary>�ړ����[�h</summary>
    public enum MoveMode
    {
        Walk,
        Fly
    }
    private void Start()
    {
        //�G�f�[�^�̓ǂݍ���
        GetDataAndSetFromContainer();
        //�̗̓Q�[�W�̍ő�l�ݒ�
        _hpBar.maxValue = _hp;
        //�R���|�[�l���g�擾
        _sr = GetComponent<SpriteRenderer>();
        _rb2d = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _as = GetComponent<AudioSource>();
        _gm = GameObject.FindObjectOfType<GameManager>();
        //�ړ����[�h�ɂ���ďd�̓X�P�[�����ϓ�
        _rb2d.gravityScale = (_moveMode == MoveMode.Fly) ? 0 : 1;
        _rb2d.freezeRotation = true;
        //�ڕW���W�̐ݒ�[����]
        RePath();
        //RayCast�̉e�����󂯂Ȃ��悤�ɂ���
        this.gameObject.layer = 2;
        //�v���C���[�̌���
        _player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        _isGameRunning = !_gm.GetPausedFlag();
    }
    private void FixedUpdate()
    {
        Debug.Log($"�ڕW�⑫�t���O{_pisPlayerFound}");
        //�̗͕\���X�V
        _hpBar.value = (_hp / _hpBar.maxValue) * _hpBar.maxValue;
        //�t���b�v����
        _sr.flipX = _targetPath.x < 0;
        //���S����
        if (_hp <= 0)
        {
            //�f���Q�[�g�Ăяo��
            deathEvent(_anim);
            //���蔲����悤�ɂ���
            this.GetComponent<Rigidbody2D>().simulated = false;
            this.GetComponent<Collider2D>().isTrigger = true;
        }
        //�p�g���[�����������ǐՁ��U��
        //�p�g���[����
        //�P�D�i�ސ�̈ʒu�̊m��
        //�Q�D���̈ʒu�ɂ�����܂��ʂ̈ʒu�ɍs��
        //�R�D�p�g���[�����Ƀv���C���[��ߑ�����΂����ɍs��
        //�S�D�ߑ����āA�U�������Ƀv���C���[������΍U��
        /* = �����{�� = */
        //�p�g���[������
        if (_isGameRunning)
        {
            #region �v���C���[���ߑ���
            //�v���C���[���ߑ���
            if (!_isPlayerFound)
            {
                Debug.Log($"�ڕW���W{_targetPath}");
                //�����̃L���X�g
                Ray2D ray = new Ray2D(this.gameObject.transform.position, _targetPath.normalized * _rayLength);
                RaycastHit2D hit2d = Physics2D.Raycast(ray.origin, ray.direction, 1 * _rayLength);
                Debug.DrawRay(ray.origin, ray.direction, Color.green);
                if (hit2d.collider)
                {
                    Debug.Log($"���C���[�F{hit2d.collider.gameObject.layer}");
                    if (hit2d.collider.gameObject.layer == _repathLayer)
                    {
                        RePath();
                        Debug.Log($"�ڕW���W�X�V{hit2d.collider.gameObject.name}");
                    }
                }
                _rb2d.AddForce(_targetPath.normalized * _ms, ForceMode2D.Force);
            }
            else
            {
                _rb2d.AddForce(_targetPath.normalized * _ms * 2, ForceMode2D.Force);
            }
            #endregion
            #region �v���C���[��������
            //�v���C���[��������
            if (Vector2.Distance(this.gameObject.transform.position,
                _player.transform.position) < _playerCaptureDistance)
            {
                _isPlayerFound = true;
                //�f���Q�[�g�Ăяo��
                if (_isPlayerFound && !_pisPlayerFound)
                {
                    playerCapturedEvent(_anim);
                    _pisPlayerFound = true;
                }
                var this2player = _player.transform.position - this.gameObject.transform.position;
                //�ړ����[�h�ɂ���ăx�N�g����ύX
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
                //�f���Q�[�g�Ăяo��
                if (!_isPlayerFound && _pisPlayerFound)
                {
                    playerMissedEvent(_anim);
                    _pisPlayerFound = false;
                }
            }
            //�U������
            if (_isPlayerFound && Vector2.Distance(this.gameObject.transform.position,
                _player.transform.position) < _attackDistance && !_attacked)
            {
                //�f���Q�[�g�Ăяo��
                attackingEvent(_anim);
                //���ʉ�
                _as.PlayOneShot(_attackV);
                //�t���O�𗧂Ă�
                _attacked = true;
                StartCoroutine(WaitForEndOfAttack(3));
            }
            #endregion
        }
    }
    /// <summary>���S���̌��ʉ��Đ�</summary>
    public void PlayDeathVoice()
    {
        //���ʉ��Đ�
        _as.PlayOneShot(_deathV);
    }
    /// <summary>���S���ɔh���N���X����Ăяo�����</summary>
    public void AddPlayerScore()
    {
        //�Q�[���}�l�[�W���[�փX�R�A�\��
        GameObject.FindAnyObjectByType<GameManager>().AddScore(_killScore);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�����s�ړ����[�h�I�����ɂ͌����������������̃��p�X�ŏ\���B����͔�s�ړ����[�h���ǂɓ����������̃��p�X����
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
        //�_���[�W����
        if (collision.gameObject.CompareTag("PlayerWeapon"))
        {
            //�f���Q�[�g�Ăяo��
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
    /// <summary>�ڕW���W�̍X�V[�����łƂ�]</summary>
    private void RePath()
    {
        Debug.Log("�ڕW�X�V");
        //�ړ����[�h�ɂ���ĖڕW���W�X�V�����𕪂���
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
    /// <summary>�f�[�^�R���e�i����̃f�[�^�ŏ�����</summary>
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
    /// <summary>�U���C���^�[�o���R���[�`���B�U���t���O��false�ɂ��ăt���O��|��</summary>
    /// <param name="s"></param>
    /// <returns></returns>
    IEnumerator WaitForEndOfAttack(float s)
    {
        yield return new WaitForSeconds(s);
        _attacked = false;
    }
}
