using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// �v���C���[�̃X�e�[�^�X�ƃQ�[���̃X�e�[�^�X���Ǘ�����N���X
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>�v���C���[�̗�</summary>
    [SerializeField] float _playerHealth;
    /// <summary>�f�t�H���g�̃v���C���[�̃}�e���A��</summary>
    [SerializeField] Material _pDefaultMaterial;
    /// <summary>�v���C���[�X�|�[�����̃}�e���A��</summary>
    [SerializeField] Material _pSpawnMaterial;
    /// <summary>�v���C���[�ăX�|�[�����̍��W</summary>
    [SerializeField] Transform _pSpawnTransform;
    /// <summary>�Q�[���o�ߎ���</summary>
    [SerializeField] Text _elapsedTimeText;
    /// <summary>�v���C���[�̃X�R�A</summary>
    [SerializeField] Text _playerScoreText;
    /// <summary>�f�X�J�E���g�̃e�L�X�g</summary>
    [SerializeField] Text _playerDeathCountText;
    /// <summary>�ꎞ��~�̃e�L�X�g</summary>
    [SerializeField] Text _pausedText;
    /// <summary>�v���C���[�̗̓X���C�_�[</summary>
    [SerializeField] Slider _hpSlider;
    /// <summary>�ꎞ��~�I�u�W�F�N�g</summary>
    [SerializeField] GameObject _pausedUI;
    /// <summary>�v���C���[�I�u�W�F�N�g</summary>
    [SerializeField] GameObject _playerObj;
    /// <summary>�Q�[���o�ߎ��ԕێ��ϐ�</summary>
    float _elapsedTime = 0;
    /// <summary>�v���C���[�X�R�A</summary>
    float _playerScore;
    /// <summary>���S�J�E���g</summary>
    int _pDeathCount = 0;
    /// <summary>�Q�[�����ꎞ��~���Ă��邩�̃t���O</summary>
    bool _isPaused = false;
    private void Start()
    {
        //�X�|�[������̃R���[�`��
        //StartCoroutine(GodMode(3));
        //�o�ߎ��Ԃ̕ϐ��l�̏�����
        _elapsedTime = 0;
        //�X�R�A�l�̏�����
        _playerScore = 0;
        //�f�X�J�E���g�l�̏�����
        _pDeathCount = 0;
        //�v���C���[�擾
        _playerObj = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        //�f���^�b���Z
        _elapsedTime += Time.deltaTime;
        //UI�e�L�X�g�ɕ\��
        _elapsedTimeText.text = _elapsedTime.ToString("F2");
        //�X���C�_�[�̒l�̏��������v���C���[�̗͕\��
        _hpSlider.value = _playerHealth / 100f;
        //�X�R�A�e�L�X�g�X�V
        _playerScoreText.text = _playerScore.ToString();
        //���S�J�E���g�̍X�V
        _playerDeathCountText.text = _pDeathCount.ToString();
        //�̗͂��O�̎�
        if (_playerHealth <= 0)
            Respawn();
    }
    /// <summary>�v���C���[�̃��X�|�[������</summary>
    private void Respawn()
    {
        //������
        _playerObj.SetActive(false);
        //���X�|�[�����W�̑��
        _playerObj.transform.position = _pSpawnTransform.position;
        //�v���C���[�̗̑͂�������
        _playerHealth = 100;
        //�o�ߎ��ԂɃy�i���e�B�������Z
        _elapsedTime += 2.0f;
        //�L����
        _playerObj.SetActive(true);
        //�f�X�J�E���g�����Z
        _pDeathCount++;
    }
    /// <summary>�̗͂��X�V���郁�\�b�h�B�����ɉ��Z����l����</summary>
    /// <param name="health"></param>
    public void ModifyHealth(float health)
    {
        _playerHealth += health;
    }
    /// <summary>�v���C���[�̃X�R�A�����Z</summary>
    /// <param name="score"></param>
    public void AddScore(float score)
    {
        _playerScore += score;
    }
    /// <summary>�Q�[���̈ꎞ��~</summary>
    public void Pause()
    {
        _isPaused = true;
        Time.timeScale = 0;
        _pausedUI.SetActive(true);
    }
    /// <summary>�Q�[���̍ĊJ</summary>
    public void Resume()
    {
        _isPaused = false;
        Time.timeScale = 1;
        _pausedUI.SetActive(false);
    }
    /// <summary>�X�|�[�������΂���̏�Ԃ̃V�F�[�_�[�ɐ؂�ւ�</summary>
    /// <param name="sec"></param>
    /// <returns></returns>
    IEnumerator GodMode(float sec)
    {
        //�X�|�[������̃}�e���A���ɕς���
        _playerObj.GetComponent<SpriteRenderer>().material = _pSpawnMaterial;
        //���Ԍo�ߌ�
        yield return new WaitForSeconds(sec);
        //�ʏ�̃}�e���A���ɕύX
        _playerObj.GetComponent<SpriteRenderer>().material = _pDefaultMaterial;
        Debug.Log("�R���[�`���I���");
    }
}