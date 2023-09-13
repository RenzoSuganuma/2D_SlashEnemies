using UnityEngine;
//sdasdadada
/// <summary>��Ԗڋʂ̓G�̃N���X</summary>
class EnemyAI_arcived : MonoBehaviour
{
    Animator _anim;
    Rigidbody2D _rb2d;
    SpriteRenderer _sr;
    //���J�t�B�[���h
    [SerializeField] float _health;
    [SerializeField] float _moveSpeed;
    [SerializeField] EMoveMode _moveMode;
    //AI�t�B�[���h
    [SerializeField] float _patrollRadius;
    [SerializeField] float _playerCaptureDistance;
    [SerializeField] float _attackDistance;
    bool _isFound = false;
    /// <summary>�X�e�[�^�X</summary>
    public enum EnemyStat
    {
        Patrol,
        Chasing,
        Death
    }
    /// <summary>�ړ����[�h</summary>
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
        //���S����
        if (_health < 0) Destroy(this.gameObject);
        //�eAI����
        MoveSequence();
        IMGFlipSequence();
    }
    void ActionTakeDamage()
    {
        //�A�j���[�V�����Đ�
        _anim.SetTrigger("actTakeHit");
        _health -= 10;//�̗͂̌�������
        if (_health < 0) { _anim.SetBool("isDeath", true); }
    }
    void ActionAttack1() { _anim.SetTrigger("actAttack1"); }
    void ActionAttack2() { _anim.SetTrigger("actAttack2"); }
    /// <summary>�v���C���[��ǐՂ���</summary>
    void MoveSequence()
    {
        Debug.Log($"{this.gameObject.name}:�v���C���⑫{_isFound}");
        //���큨�������ǐՁ��U��
        //����X�e�[�g����
        if (!_isFound)//�p�g���[��
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
    /// <summary>�摜�̃t���b�v����</summary>
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
