using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// プレイヤーのステータスとゲームのステータスを管理するクラス
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>プレイヤー体力</summary>
    [SerializeField] float _playerHealth;
    /// <summary>プレイヤー再スポーン時の座標</summary>
    [SerializeField] Transform _pSpawnTransform;
    /// <summary>ゲーム経過時間</summary>
    [SerializeField] Text _elapsedTimeText;
    /// <summary>プレイヤーのスコア</summary>
    [SerializeField] Text _playerScoreText;
    /// <summary>デスカウントのテキスト</summary>
    [SerializeField] Text _playerDeathCountText;
    /// <summary>一時停止のテキスト</summary>
    [SerializeField] Text _pausedText;
    /// <summary>プレイヤー体力スライダー</summary>
    [SerializeField] Slider _hpSlider;
    /// <summary>一時停止オブジェクト</summary>
    [SerializeField] GameObject _pausedUI;
    /// <summary>プレイヤーオブジェクト</summary>
    [SerializeField] GameObject _playerObj;
    /// <summary>ゲーム経過時間保持変数</summary>
    float _elapsedTime = 0;
    /// <summary>プレイヤースコア</summary>
    float _playerScore;
    /// <summary>死亡カウント</summary>
    int _pDeathCount = 0;
    /// <summary>ゲームが一時停止しているかのフラグ</summary>
    bool _isPaused = false;
    private void Start()
    {
        //経過時間の変数値の初期化
        _elapsedTime = 0;
        //スコア値の初期化
        _playerScore = 0;
        //デスカウント値の初期化
        _pDeathCount = 0;
    }
    private void Update()
    {
        //デルタ秒加算
        _elapsedTime += Time.deltaTime;
        //UIテキストに表示
        _elapsedTimeText.text = _elapsedTime.ToString("F2");
        //スライダーの値の初期化→プレイヤー体力表示
        _hpSlider.value = _playerHealth / 100f;
        //スコアテキスト更新
        _playerScoreText.text = _playerScore.ToString();
        //死亡カウントの更新
        _playerDeathCountText.text = _pDeathCount.ToString();
        //体力が０の時
        if (_playerHealth <= 0) { Respawn(); }
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
    public void AddScore(float score)
    {
        _playerScore += score;
    }
    /// <summary>ゲームの一時停止</summary>
    public void Pause()
    {
        _isPaused = true;
        Time.timeScale = 0;
        _pausedUI.SetActive(true);
    }
    /// <summary>ゲームの再開</summary>
    public void Resume()
    {
        _isPaused = false;
        Time.timeScale = 1;
        _pausedUI.SetActive(false);
    }
}