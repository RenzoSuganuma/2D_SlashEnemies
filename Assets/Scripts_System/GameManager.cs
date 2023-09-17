using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// �v���C���[�̃X�e�[�^�X�ƃQ�[���̃X�e�[�^�X���Ǘ�����N���X
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
    /// <summary>�v���C���[�̃��X�|�[������</summary>
    private void Respawn()
    {
        var p = GameObject.FindGameObjectWithTag("Player");
        p.SetActive(false);
        p.transform.position = _pSpawnTransform.position;
        _playerHealth = 100;
        _elapsedTime += 2.0f;
        p.SetActive(true);
    }
    /// <summary>�̗͂��X�V���郁�\�b�h�B�����ɉ��Z����l����</summary>
    /// <param name="health"></param>
    public void ModifyHealth(float health)
    {
        _playerHealth += health;
    }
    /// <summary>�X�|�[�������΂���̏�Ԃ̃V�F�[�_�[�ɐ؂�ւ�</summary>
    /// <param name="sec"></param>
    /// <returns></returns>
    IEnumerator GodMode(float sec)
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Renderer>().material = _pSpawnMaterial;
        yield return new WaitForSeconds(sec);
        GameObject.FindGameObjectWithTag("Player").GetComponent<Renderer>().material = _pDefaultMaterial;
    }
}