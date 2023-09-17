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
    /// <summary>デフォルトのプレイヤーのマテリアル</summary>
    [SerializeField] Material _pDefaultMaterial;
    /// <summary>プレイヤースポーン時のマテリアル</summary>
    [SerializeField] Material _pSpawnMaterial;
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
    /// <summary>ゲーム経過時間保持変数</summary>
    float _elapsedTime = 0;
    /// <summary>プレイヤースコア</summary>
    float _playerScore;
    /// <summary>死亡カウント</summary>
    int _pDeathCount = 0;
    private void Start()
    {
        //スポーン直後のコルーチン
        StartCoroutine(GodMode(3));
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
        if (_playerHealth <= 0)
            Respawn();
    }
    /// <summary>プレイヤーのリスポーン処理</summary>
    private void Respawn()
    {
        //プレイヤーの検索
        var p = GameObject.FindGameObjectWithTag("Player");
        //無効化
        p.SetActive(false);
        //リスポーン座標の代入
        p.transform.position = _pSpawnTransform.position;
        //プレイヤーの体力を初期化
        _playerHealth = 100;
        //経過時間にペナルティ分を加算
        _elapsedTime += 2.0f;
        //有効化
        p.SetActive(true);
        //デスカウントを加算
        _pDeathCount++;
    }
    /// <summary>体力を更新するメソッド。引数に加算する値を代入</summary>
    /// <param name="health"></param>
    public void ModifyHealth(float health)
    {
        _playerHealth += health;
    }
    public void AddScore(float score)
    {
        _playerScore += score;
    }
    /// <summary>スポーンしたばかりの状態のシェーダーに切り替え</summary>
    /// <param name="sec"></param>
    /// <returns></returns>
    IEnumerator GodMode(float sec)
    {
        //スポーン直後のマテリアルに変える
        GameObject.FindGameObjectWithTag("Player").GetComponent<Renderer>().material = _pSpawnMaterial;
        //時間経過後
        yield return new WaitForSeconds(sec);
        //通常のマテリアルに変更
        GameObject.FindGameObjectWithTag("Player").GetComponent<Renderer>().material = _pDefaultMaterial;
    }
}