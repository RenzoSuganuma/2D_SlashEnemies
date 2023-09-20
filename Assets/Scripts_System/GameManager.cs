using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// �v���C���[�̃X�e�[�^�X�ƃQ�[���̃X�e�[�^�X���Ǘ�����N���X
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>�v���C���[�̗�</summary>
    [SerializeField] float _playerHealth;
    /// <summary>�v���C���[�ăX�|�[�����̍��W</summary>
    [SerializeField] Transform _pSpawnTransform;
    /// <summary>�Q�[���o�ߎ���</summary>
    [SerializeField] Text _elapsedTimeText;
    /// <summary>�v���C���[�̃X�R�A</summary>
    [SerializeField] Text _playerScoreText;
    /// <summary>�f�X�J�E���g�̃e�L�X�g</summary>
    [SerializeField] Text _playerDeathCountText;
    /// <summary>�v���C���[�̗̓X���C�_�[</summary>
    [SerializeField] Slider _hpSlider;
    /// <summary>�ꎞ��~�I�u�W�F�N�g</summary>
    [SerializeField] GameObject _pausedUI;
    /// <summary>�v���C���[�I�u�W�F�N�g</summary>
    [SerializeField] GameObject _playerObj;
    /// <summary>�{�X�I�u�W�F�N�g</summary>
    [SerializeField] GameObject _bossObj;
    /// <summary>�{�X���̃e�L�X�g</summary>
    [SerializeField] Text _bossNameText;
    /// <summary>�{�X�̗̓X���C�_�[</summary>
    [SerializeField] Slider _bossHpSlider;
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
        //�o�ߎ��Ԃ̕ϐ��l�̏�����
        _elapsedTime = 0;
        //�X�R�A�l�̏�����
        _playerScore = 0;
        //�f�X�J�E���g�l�̏�����
        _pDeathCount = 0;
        //�{�X���\���������Ƒ̗͎擾�A�X�V
        if (_bossObj != null)
        {
            _bossNameText.text = _bossObj.name;
            _bossHpSlider.maxValue = _bossObj.GetComponent<BossAI_alpha>().GetHP();
            _bossHpSlider.value = _bossObj.GetComponent<BossAI_alpha>().GetHP();
        }
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
        //�{�X�̗͂̍X�V
        if (_bossObj != null)
            _bossHpSlider.value = _bossObj.GetComponent<BossAI_alpha>().GetHP();
        //���S�J�E���g�̍X�V
        _playerDeathCountText.text = _pDeathCount.ToString();
        //�̗͂��O�̎�
        if (_playerHealth <= 0) { Respawn(); }
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
    /// <summary>�Q�[���V�[���̓ǂݍ���</summary>
    public void GotoGameSceneNormal()
    {
        SceneManager.LoadScene("GameScene1", LoadSceneMode.Single);
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}