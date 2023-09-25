using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
/// <summary>�{�X��AI</summary>
public class BossAI_alpha : MonoBehaviour
{
    //Unity�R���|�[�l���g
    Animator _anim;
    Rigidbody2D _rb2d;
    SpriteRenderer _sr;
    AudioSource _as;
    //���J�t�B�[���h
    /// <summary>�̗�</summary>
    [SerializeField] float _health;
    /// <summary>�ő�̗�</summary>
    [SerializeField] float _maxHealth;
    /// <summary>�ړ����x</summary>
    [SerializeField] float _moveSpeed;
    /// <summary>�v���C���[�ߑ�����</summary>
    [SerializeField] float _playerCapturedDistance;
    /// <summary>�v���C���[�U���͈�</summary>
    [SerializeField] float _attackRange;
    /// <summary>�����U���͈�</summary>
    [SerializeField] float _spellRange;
    /// <summary>�v���C���[����̃_���[�W��</summary>
    [SerializeField] float _damageFromPlayer;
    /// <summary>�����U�����</summary>
    [SerializeField] int _spellLim;
    /// <summary>�����U���̃I�u�W�F�N�g</summary>
    [SerializeField] GameObject _spellObj;
    /// <summary>���S���I�u�W�F�N�g</summary>
    [SerializeField] GameObject _deathObj;
    /// <summary>���[�v�n�_</summary>
    [SerializeField] Transform[] _warpPos;
    /// <summary>BGM�̃I�[�f�B�I�\�[�X</summary>
    [SerializeField] AudioSource _bgmSource;
    [SerializeField] AudioClip _attackV;
    [SerializeField] AudioClip _deathV;
    [SerializeField] AudioClip _bossMid;
    [SerializeField] AudioClip _bossEnd;
    [SerializeField] Material _hpMidMat;
    [SerializeField] Material _hpEndMat;
    bool _ismiddleHP = false;
    bool _isendHP = false;
    //����J�t�B�[���hw
    /// <summary>�v���C���[�I�u�W�F�N�g</summary>
    GameObject _player;
    /// <summary>�ߑ��t���O</summary>
    public bool _captured = false;
    /// <summary>�U���͈͓��ߑ��t���O</summary>
    public bool _insideRange = false;
    /// <summary>�U���t���O</summary>
    public bool _attacked = false;
    /// <summary>�����U���t���O</summary>
    public bool _spelled = false;
    /// <summary>�����U���J�E���g</summary>
    int _spellCnt = 0;
    /// <summary>�ݐσ_���[�W��</summary>
    float _totalDamages = 0;
    /// <summary>�Q�[�����s���t���O</summary>
    bool _isGameRunning = false;
    //�v���C���[�ߑ��A�ҋ@�X�e�[�g
    //�ʏ�U���A���@�U��
    //��U���A���S
    private void Start()
    {
        //Unity�R���|�[�l���g�̎擾
        _anim = GetComponent<Animator>();
        _rb2d = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _as = GetComponent<AudioSource>();
        //�v���C���[�̌���
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
            _sr.material = _hpMidMat;
        }
        if (_health < _maxHealth * .3f && !_isendHP)
        {
            _isendHP = true;
            _bgmSource.Stop();
            _bgmSource.clip = _bossEnd;
            _bgmSource.Play();
            _sr.material = _hpEndMat;
        }
        float dThreshord = 9;
        if(Mathf.Abs(_player.transform.position.y - this.gameObject.transform.position.y) > dThreshord)
        {
            //���[�v����
            this.gameObject.transform.position =
                _player.transform.position +
                new Vector3(UnityEngine.Random.Range(-1, 1), _player.transform.position.y, 0);
        }
        _isGameRunning = !GameObject.FindAnyObjectByType<GameManager>().GetPausedFlag();
    }
    /// <summary>���S������</summary>
    void DeathBehaviour()
    {
        //�Q�[���}�l�[�W���[�̃C�x���g���Ăяo��
        GameObject.FindAnyObjectByType<GameManager>().AddScore(999);
        GameObject.FindAnyObjectByType<GameManager>().BossDeathEvent(this.gameObject);
        //���S����
        var obj = Instantiate(_deathObj);
        obj.transform.position = this.transform.position;
        this.gameObject.SetActive(false);
        Destroy(this);
        Destroy(obj, .5f);
        _as.PlayOneShot(_deathV);
    }
    /// <summary>�v���C���[�̋߂��փ��[�v����</summary>
    void WarpBehaviour()
    {
        var obj = Instantiate(_deathObj);
        obj.transform.position = this.transform.position;
        this.gameObject.SetActive(false);
        //���[�v����
        this.gameObject.transform.position = 
            _player.transform.position + 
            new Vector3(UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(0, 3), 0);
        this.gameObject.SetActive(true);
        if (_captured && _insideRange && _attacked && _spelled)
        {
            _captured = _insideRange = _attacked = _spelled = false;
        }
        Destroy(obj, 2);
    }
    /// <summary>�̗͂̎擾���\�b�h</summary>
    /// <returns></returns>
    public float GetHP()
    {
        return _health;
    }
    private void FixedUpdate()
    {
        if (_isGameRunning)
        {
            Debug.Log($"{this.gameObject.name} : �v���C���[�ߑ�");
            //�ߑ���ԍX�V
            _captured = Vector2.Distance(_player.transform.position,
            this.transform.position) < _playerCapturedDistance;
            //�U���͈͓��ɋ��邩�̔���
            _insideRange = Vector2.Distance(_player.transform.position,
            this.transform.position) < _attackRange && _captured;
            //�A�j���[�^�[
            _anim.SetBool("Moving", _captured);
            if (_captured)
            {
                //�ڕW�ւ̃x�N�g���̎Z�o
                var tv = (_player.transform.position - this.transform.position).normalized;
                //�摜�t���b�v����
                _sr.flipX = tv.x > 0;
                //���̕����֌�����
                _rb2d.AddForce(tv * _moveSpeed, ForceMode2D.Force);
            }
            if (_insideRange && !_attacked)
            {
                //�U������
                StartCoroutine(AttackRoutine(3));
            }//�U����������O��A���@�̎˒����ɂ���Ƃ�
            else if (_captured && Vector2.Distance(_player.transform.position,
                this.transform.position) < _spellRange && !_spelled && _spellCnt < _spellLim)
            {
                //��������
                StartCoroutine(SpellRoutine(3));
            }
        }
    }
    /// <summary>�ʏ�U�����[�`��</summary>
    /// <param name="t"></param>
    /// <returns></returns>
    IEnumerator AttackRoutine(float t)
    {
        //�A�j���[�^�[
        _anim.SetTrigger("actAtk");
        _as.PlayOneShot(_attackV);
        Debug.Log($"{this.gameObject.name}�U��");
        _attacked = true;
        yield return new WaitForSeconds(t);
        _attacked = false;
    }
    /// <summary>�����U�����[�`��</summary>
    /// <param name="t"></param>
    /// <returns></returns>
    IEnumerator SpellRoutine(float t)
    {
        //�A�j���[�^�[
        _anim.SetTrigger("actSpl");
        Debug.Log($"{this.gameObject.name}����");
        _spelled = true;
        //�I�u�W�F�N�g����
        var so = Instantiate(_spellObj);
        _spellCnt++;
        //�j��
        Destroy(so, 1);
        //���W�X�V
        so.transform.position = new Vector2(_player.transform.position.x, _player.transform.position.y + 1);
        yield return new WaitForSeconds(t);
        _spelled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�_���[�W����
        if (collision.CompareTag("PlayerWeapon"))
        {
            //�A�j���[�^�[
            _anim.SetTrigger("actHrt");
            //�_���[�W����
            _health -= (_damageFromPlayer + UnityEngine.Random.Range(10, 30));
            _totalDamages += _damageFromPlayer;
            //�ݐσ_���[�W����萅����������
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
