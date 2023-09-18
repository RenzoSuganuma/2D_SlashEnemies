using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    /// <summary>インスタンス化する敵プレハブ</summary>
    [SerializeField] GameObject[] _enemies;
    /// <summary>生成インターバル</summary>
    [SerializeField] float _interval;
    /// <summary>個体数限界</summary>
    [SerializeField] int _limit;
    /// <summary>経過時間</summary>
    float _elapsedT = 0;
    private void FixedUpdate()
    {
        Debug.Log("敵スポナー時間カウント");
        _elapsedT += Time.deltaTime;
        if (_elapsedT > _interval && GameObject.FindGameObjectsWithTag("Monster").Length < _limit)
        {
            var go = Instantiate(_enemies[UnityEngine.Random.Range(0, _enemies.Length)]);
            go.transform.position = this.transform.position;
            _elapsedT = 0;
        }
    }
}
