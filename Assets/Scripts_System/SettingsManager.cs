using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
public class SettingsManager : MonoBehaviour
{
    [SerializeField] AudioMixer _aMixer;
    [SerializeField] Slider _masterSlider;
    [SerializeField] Slider _bgmSlider;
    [SerializeField] Slider _voiceSlider;
    [SerializeField] Text _eTime;
    [SerializeField] Text _dCnt;
    [SerializeField] Text _pScr;
    /// <summary>���O���̓t�B�[���h</summary>
    [SerializeField] InputField _inputFeild;
    /// <summary>�v���C���[��</summary>
    [SerializeField] Text _playerName;
    [SerializeField] GameManager _gm;
    string jsonData = string.Empty;
    private void Start()
    {
        StreamReader reader = new StreamReader(Application.dataPath + "/test.json");
        string data = reader.ReadToEnd();
        reader.Close();
        Debug.Log($"JSON�t�@�C������̓ǂݍ��݃f�[�^�[�F{data}");
        PlayerSaveDataContainer psdc =
            JsonUtility.FromJson<PlayerSaveDataContainer>(data);
        //�I�[�f�B�I�~�L�T�[���ʂ̐ݒ�
        _aMixer.SetFloat("MasterVol", psdc._masterVol);
        _aMixer.SetFloat("BGMVol", psdc._bgmVol);
        _aMixer.SetFloat("VoiceVol", psdc._voiceVol);
        //�v���C���[���ݒ�
        _playerName.text = psdc._playerName;
        //�Q�[���}�l�[�W���[�̃f�[�^�[������
        _gm._playerScore = psdc._score;
        _gm._pDeathCount = psdc._deathcount;
        _gm._elapsedTime = psdc._elapsedtime;
    }
    private void OnDisable()
    {
        if (SceneManager.GetActiveScene().name == "GameScene2")
        {
            ResetDatas();
        }
    }
    public void SetDatasOnHome()
    {
        PlayerSaveDataContainer psdc = new PlayerSaveDataContainer();
        psdc._playerName = _inputFeild.text;
        psdc._masterVol = _masterSlider.value;
        psdc._bgmVol = _bgmSlider.value;
        psdc._voiceVol = _voiceSlider.value;
        //JSON��
        jsonData = JsonUtility.ToJson(psdc);
        //�f�[�^��������
        StreamWriter writer = new StreamWriter(Application.dataPath + "/test.json");
        writer.WriteLine(jsonData);
        writer.Flush();
        writer.Close();
    }
    public void SetDatas()
    {
        PlayerSaveDataContainer psdc = new PlayerSaveDataContainer();
        psdc._playerName = _playerName.text;
        psdc._masterVol = _masterSlider.value;
        psdc._bgmVol = _bgmSlider.value;
        psdc._voiceVol = _voiceSlider.value;
        //�Q�[���}�l�[�W���[����f�[�^�[�擾
        Debug.Log($"GM����̃f�[�^{_gm._playerScore},{_gm._pDeathCount},{_gm._elapsedTime}");
        psdc._score = int.Parse(_pScr.text);
        psdc._deathcount = int.Parse(_dCnt.text);
        psdc._elapsedtime = float.Parse(_eTime.text);
        //JSON��
        jsonData = JsonUtility.ToJson(psdc);
        //�f�[�^��������
        StreamWriter writer = new StreamWriter(Application.dataPath + "/test.json");
        writer.WriteLine(jsonData);
        writer.Flush();
        writer.Close();
    }
    public void ResetDatas()
    {
        StreamWriter writer = new StreamWriter(Application.dataPath + "/test.json");
        writer.WriteLine(" ");
        writer.Flush();
        writer.Close();
    }
    public void SetMasterVolume()
    {
        _aMixer.SetFloat("MasterVol", _masterSlider.value);
    }
    public void SetBGMVolume()
    {
        _aMixer.SetFloat("BGMVol", _bgmSlider.value);
    }
    public void SetVoiceVolume()
    {
        _aMixer.SetFloat("VoiceVol", _voiceSlider.value);
    }
}
