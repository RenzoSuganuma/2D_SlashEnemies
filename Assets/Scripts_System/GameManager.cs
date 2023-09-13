using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// プレイヤーのステータスとゲームのステータスを管理するクラス
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] float _playerHealth;
    [SerializeField] float _playerStamina;
    /// <summary>プレイヤースポーン時のマテリアル(ShaderGraph)</summary>
    [SerializeField] Material _playerSpawnMat;
    /// <summary>プレイヤーのデフォルトのマテリアル(ShaderGraph)</summary>
    [SerializeField] Material _playerDefaultMat;
    private void Start()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        Material mat = go.GetComponent<Renderer>().sharedMaterial;

    }
    /// <summary>体力を更新するメソッド。引数に加算する値を代入</summary>
    /// <param name="health"></param>
    public void ModifyHealth(float health)
    {
        _playerHealth += health;
    }
    /// <summary>スタミナを更新するメソッド。引数に加算する値を代入</summary>
    /// <param name="stamina"></param>
    public void ModifyStamina(float stamina)
    {
        _playerStamina += stamina;
    }
}