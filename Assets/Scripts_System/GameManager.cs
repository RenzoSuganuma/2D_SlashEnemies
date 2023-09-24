using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG;
using DG.Tweening;
using System;
/// <summary>
/// プレイヤーのステータスとゲームのステータスを管理するクラス
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>プレイヤー体力</summary>
    [SerializeField] float _playerHealth;
    /// <summary>次に進めるスコア数</summary>
    [SerializeField] int _scoreGotoNext;
    /// <summary>ボス戦のシーンの入口</summary>
    [SerializeField] GameObject _nextGate;
    /// <summary>プレイヤー再スポーン時の座標</summary>
    [SerializeField] Transform _pSpawnTransform;
    /// <summary>ゲーム経過時間</summary>
    [SerializeField] Text _elapsedTimeText;
    /// <summary>プレイヤーのスコア</summary>
    [SerializeField] Text _playerScoreText;
    /// <summary>デスカウントのテキスト</summary>
    [SerializeField] Text _playerDeathCountText;
    /// <summary>プレイヤー体力スライダー</summary>
    [SerializeField] Slider _hpSlider;
    /// <summary>一時停止オブジェクト</summary>
    [SerializeField] GameObject _pausedUI;
    /// <summary>一時停止のラベル</summary>
    [SerializeField] Text _pausedLabel;
    /// <summary>プレイヤーオブジェクト</summary>
    [SerializeField] GameObject _playerObj;
    /// <summary>ボスオブジェクト</summary>
    [SerializeField] GameObject _bossObj;
    /// <summary>ボス名のテキスト</summary>
    [SerializeField] Text _bossNameText;
    /// <summary>ボス名のテキスト</summary>
    [SerializeField] Text _scoreText;
    /// <summary>ボス名のテキスト</summary>
    [SerializeField] Text _clearTimeText;
    /// <summary>死亡カウントテキスト</summary>
    [SerializeField] Text _deathcountText;
    /// <summary>ランキングテキスト</summary>
    [SerializeField] Text _rankingText;
    /// <summary>残機テキスト</summary>
    [SerializeField] Text _lifeText;
    /// <summary>ホーム画面のタイトルテキスト</summary>
    [SerializeField] GameObject _homeTitleText;
    /// <summary>ホームシーンのフラグ</summary>
    [SerializeField] bool _isHomeScene;
    /// <summary>次へ進めのテキスト</summary>
    [SerializeField] GameObject _gotoNextText;
    /// <summary>ボス体力スライダー</summary>
    [SerializeField] Slider _bossHpSlider;
    /// <summary>設定マネージャー</summary>
    [SerializeField] SettingsManager _sm;
    /// <summary>ボス撃破ＯＢＪ</summary>
    [SerializeField] GameObject _bossDefeatedPanel;
    /// <summary>ガメオベラOBJ</summary>
    [SerializeField] GameObject _gameOverOBJ;
    /// <summary>死亡回数限度値</summary>
    [SerializeField] int _deathLim;
    /// <summary>ゲーム経過時間保持変数</summary>
    public float _elapsedTime = 0;
    /// <summary>プレイヤースコア</summary>
    public int _playerScore = 0;
    /// <summary>死亡カウント</summary>
    public int _pDeathCount = 0;
    /// <summary>ゲームが一時停止しているかのフラグ</summary>
    bool _isPaused = false;
    private void Start()
    {
        //マウスカーソルロック
        if (SceneManager.GetActiveScene().name != "HomeScene")
            Cursor.lockState = CursorLockMode.Locked;
        if (SceneManager.GetActiveScene().name != "GameScene2")
        {
            //経過時間の変数値の初期化
            _elapsedTime = 0;
            //スコア値の初期化
            _playerScore = 0;
            //デスカウント値の初期化
            _pDeathCount = 0;
        }
        //ボス名表示初期化と体力取得、更新
        if (_bossObj != null)
        {
            _bossNameText.text = _bossObj.name;
            _bossHpSlider.maxValue = _bossObj.GetComponent<BossAI_alpha>().GetHP();
            _bossHpSlider.value = _bossObj.GetComponent<BossAI_alpha>().GetHP();
        }
        //DOTweenでタイトルをTween
        if (_isHomeScene)
        {
            _homeTitleText.transform.DOShakeScale(1, 3, 50, 100, true);
        }
    }
    private void Update()
    {
        //デルタ秒加算
        if (!_isPaused)
            _elapsedTime += Time.deltaTime;
        //UIテキストに表示
        _elapsedTimeText.text = _elapsedTime.ToString("F2");
        //スライダーの値の初期化→プレイヤー体力表示
        _hpSlider.value = _playerHealth / 100f;
        //スコアテキスト更新
        _playerScoreText.text = _playerScore.ToString();
        //ボス体力の更新
        if (_bossObj != null)
            _bossHpSlider.value = _bossObj.GetComponent<BossAI_alpha>().GetHP();
        //死亡カウントの更新
        _playerDeathCountText.text = _pDeathCount.ToString();
        //体力が０の時
        if (_playerHealth <= 0) { Respawn(); }
        //ボス部屋への道を有効化
        if (_playerScore >= _scoreGotoNext)
        {
            _nextGate.SetActive(true);
            _gotoNextText.SetActive(true);
        };
        //ゲームオーバー処理
        if (_pDeathCount >= _deathLim)
        {
            //カーソル表示
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            _gameOverOBJ.SetActive(true);
        }
        //残機表示更新
        _lifeText.text = $"残機：{(_deathLim - _pDeathCount).ToString()}";
    }
    /// <summary>プレイヤーのリスポーン処理</summary>
    private void Respawn()
    {
        //無効化
        _playerObj.SetActive(false);
        //リスポーン座標の代入
        _playerObj.transform.position = _pSpawnTransform.position;
        //プレイヤーの体力を初期化
        _playerHealth = 100;
        //経過時間にペナルティ分を加算
        _elapsedTime += 2.0f;
        //有効化
        _playerObj.SetActive(true);
        //デスカウントを加算
        _pDeathCount++;
    }
    /// <summary>体力を更新するメソッド。引数に加算する値を代入</summary>
    /// <param name="health"></param>
    public void ModifyHealth(float health)
    {
        _playerHealth += health;
    }
    /// <summary>プレイヤーのスコアを加算</summary>
    /// <param name="score"></param>
    public void AddScore(int score)
    {
        _playerScore += score;
    }
    /// <summary>ゲームの一時停止</summary>
    public void Pause()
    {
        Debug.Log("一時停止中");
        //カーソル表示
        Cursor.lockState = CursorLockMode.None;
        _isPaused = true;
        //Time.timeScale = 0;
        _pausedUI.SetActive(true);
        if (_pausedUI.activeSelf)
        {
            Debug.Log("一時停止中ラベル");
        }
    }
    /// <summary>ゲームの再開</summary>
    public void Resume()
    {
        Debug.Log("再開");
        //カーソル非表示
        Cursor.lockState = CursorLockMode.Locked;
        _isPaused = false;
        //Time.timeScale = 1;
        _pausedUI.SetActive(false);
    }
    /// <summary>ボス死亡時のメソッド</summary>
    /// <param name="bossObj"></param>
    public void BossDeathEvent(GameObject bossObj)
    {
        //カーソル表示
        Cursor.lockState = CursorLockMode.None;
        //タイムスケールの操作
        Time.timeScale = .2f;
        var boss = GameObject.FindAnyObjectByType<CinemachineVirtualCamera>();
        boss.m_Follow = bossObj.transform;
        boss.m_LookAt = bossObj.transform;
        boss.m_Lens.OrthographicSize = 2;
        _bossDefeatedPanel.SetActive(true);
        _clearTimeText.text = "クリア時間 : " + _elapsedTime.ToString("F2");
        _scoreText.text = "スコア : " + _playerScore.ToString();
        _deathcountText.text = "死亡数 : " + _pDeathCount.ToString();
        string rank = "";
        if (_elapsedTime < 100 && _pDeathCount < 3 && _playerScore > 1500)
        {
            rank = "S";
        }
        else if (_elapsedTime < 200 && _pDeathCount < 5)
        {
            rank = "A";
        }
        else if (_elapsedTime < 300 && _pDeathCount < 7)
        {
            rank = "B";
        }
        else if (_elapsedTime < 400 && _pDeathCount < 10)
        {
            rank = "C";
        }
        _rankingText.text = "<color=red>評価</color> : " + $"<color=yellow>{rank}</color>";
    }
    /// <summary>一時停止フラグ</summary>
    /// <returns></returns>
    public bool GetPausedFlag()
    {
        return _isPaused;
    }
    /// <summary>ゲームシーンの読み込み</summary>
    public void GotoGameSceneNormal()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("GameScene1", LoadSceneMode.Single);
    }
    /// <summary>ゲームシーンの読み込み</summary>
    public void GotoGameSceneBoss()
    {
        Time.timeScale = 1;
        Debug.Log("ボス戦へ移行");
        _sm.SetPlayerDatas();
        SceneManager.LoadScene("GameScene2", LoadSceneMode.Single);
    }
    /// <summary>ゲームシーンの読み込み</summary>
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