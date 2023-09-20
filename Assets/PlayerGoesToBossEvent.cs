using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerGoesToBossEvent : MonoBehaviour
{
    [SerializeField] UnityEvent _playerGoesToBossEvent;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _playerGoesToBossEvent.Invoke();
        }
    }
}
