using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// プレイヤーのステータスとゲームのステータスを管理するクラス
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] float _playerHealth;
    [SerializeField] Material _pDefaultMaterial;
    [SerializeField] Material _pSpawnMaterial;
    [SerializeField] Transform _pSpawnTransform;
    [SerializeField] Text _elapsedTimeText;
    [SerializeField] Slider _hpSlider;
    float _elapsedTime = 0;
    private void Start()
    {
        StartCoroutine(GodMode(3));
        _elapsedTime = 0;
    }
    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        _elapsedTimeText.text = _elapsedTime.ToString("F2");
        _hpSlider.value = _playerHealth / 100f;
        if (_playerHealth <= 0)
            Respawn();
    }
    /// <summary>プレイヤーのリスポーン処理</summary>
    private void Respawn()
    {
        var p = GameObject.FindGameObjectWithTag("Player");
        p.SetActive(false);
        p.transform.position = _pSpawnTransform.position;
        _playerHealth = 100;
        _elapsedTime += 2.0f;
        p.SetActive(true);
    }
    /// <summary>体力を更新するメソッド。引数に加算する値を代入</summary>
    /// <param name="health"></param>
    public void ModifyHealth(float health)
    {
        _playerHealth += health;
    }
    /// <summary>スポーンしたばかりの状態のシェーダーに切り替え</summary>
    /// <param name="sec"></param>
    /// <returns></returns>
    IEnumerator GodMode(float sec)
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Renderer>().material = _pSpawnMaterial;
        yield return new WaitForSeconds(sec);
        GameObject.FindGameObjectWithTag("Player").GetComponent<Renderer>().material = _pDefaultMaterial;
    }
}