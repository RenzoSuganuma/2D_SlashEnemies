using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// �v���C���[�̃X�e�[�^�X�ƃQ�[���̃X�e�[�^�X���Ǘ�����N���X
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>�v���C���[�̗�</summary>
    [SerializeField] float _playerHealth;
    /// <summary>���ɐi�߂�X�R�A��</summary>
    [SerializeField] int _scoreGotoNext;
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
    /// <summary>�{�X���̃e�L�X�g</summary>
    [SerializeField] Text _scoreText;
    /// <summary>�{�X���̃e�L�X�g</summary>
    [SerializeField] Text _clearTimeText;
    /// <summary>���S�J�E���g�e�L�X�g</summary>
    [SerializeField] Text _deathcountText;
    /// <summary>���S�J�E���g�e�L�X�g</summary>
    [SerializeField] Text _rankingText;
    /// <summary>���֐i�߂̃e�L�X�g</summary>
    [SerializeField] GameObject _gotoNextText;
    /// <summary>�{�X�̗̓X���C�_�[</summary>
    [SerializeField] Slider _bossHpSlider;
    /// <summary>�{�X���j�n�a�i</summary>
    [SerializeField] GameObject _bossDefeatedPanel;
    /// <summary>�Q�[���o�ߎ��ԕێ��ϐ�</summary>
    public float _elapsedTime = 0;
    /// <summary>�v���C���[�X�R�A</summary>
    public int _playerScore = 0;
    /// <summary>���S�J�E���g</summary>
    public int _pDeathCount = 0;
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
        if (_playerScore >= _scoreGotoNext) { _gotoNextText.SetActive(true); };
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
    public void AddScore(int score)
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
    /// <summary>�{�X���S���̃��\�b�h</summary>
    /// <param name="bossObj"></param>
    public void BossDeathEvent(GameObject bossObj)
    {
        //�^�C���X�P�[���̑���
        Time.timeScale = .2f;
        var boss = GameObject.FindAnyObjectByType<CinemachineVirtualCamera>();
        boss.m_LookAt = bossObj.transform;
        boss.m_Lens.OrthographicSize = 2;
        _bossDefeatedPanel.SetActive(true);
        _clearTimeText.text = "�N���A���� : " + _elapsedTime.ToString("F2");
        _scoreText.text = "�X�R�A : " + _playerScore.ToString();
        _deathcountText.text = "���S�� : " + _pDeathCount.ToString();
        //string rank;
        //if(_clearTimeText)
        _rankingText.text = "<color=red>�]��</color> : ";
    }
    /// <summary>�Q�[���V�[���̓ǂݍ���</summary>
    public void GotoGameSceneNormal()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("GameScene1", LoadSceneMode.Single);
    }
    /// <summary>�Q�[���V�[���̓ǂݍ���</summary>
    public void GotoGameSceneBoss()
    {
        Time.timeScale = 1;
        GameObject.FindAnyObjectByType<SettingsManager>().SetDatas();
        SceneManager.LoadScene("GameScene2", LoadSceneMode.Single);
    }
    /// <summary>�Q�[���V�[���̓ǂݍ���</summary>
    public void GotoHome()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("HomeScene", LoadSceneMode.Single);
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