using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] AudioMixer _aMixer;
    [SerializeField] Slider _masterSlider;
    [SerializeField] Slider _bgmSlider;
    [SerializeField] Slider _voiceSlider;
    public int _deathCount = 0;
    public int _score = 0;
    public float _elapsedTime = 0;
    /// <summary>名前入力フィールド</summary>
    [SerializeField] InputField _inputFeild;
    /// <summary>プレイヤー名</summary>
    [SerializeField] Text _playerName;
    string jsonData = string.Empty;
    private void Start()
    {
        PlayerSaveDataContainer psdc =
            JsonUtility.FromJson<PlayerSaveDataContainer>(PlayerPrefs.GetString("userDatas"));
        //オーディオミキサー音量の設定
        _aMixer.SetFloat("MasterVol", psdc._masterVol);
        _aMixer.SetFloat("BGMVol", psdc._bgmVol);
        _aMixer.SetFloat("VoiceVol", psdc._voiceVol);
        //プレイヤー名設定
        _playerName.text = psdc._playerName;
        //ゲームマネージャーのデーター初期化
        var gm = GetComponent<GameManager>();
        gm._playerScore = psdc._score;
        gm._pDeathCount = psdc._deathcount;
        gm._elapsedTime = psdc._elapsedtime;
        Debug.Log($"STARTプレイヤーデータ{PlayerPrefs.GetString("userDatas")}");
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
        //ゲームマネージャーからデーター取得
        var gm = GetComponent<GameManager>();
        psdc._score = gm._playerScore;
        psdc._deathcount = gm._pDeathCount;
        psdc._elapsedtime = gm._elapsedTime;
        //JSON化
        jsonData = JsonUtility.ToJson(psdc);
        //データのセーブ
        PlayerPrefs.SetString("userDatas", jsonData);
        Debug.Log($"プレイヤーデータ{PlayerPrefs.GetString("userDatas")}");
    }
    public void SetDatas()
    {
        PlayerSaveDataContainer psdc = new PlayerSaveDataContainer();
        psdc._playerName = _playerName.text;
        psdc._masterVol = _masterSlider.value;
        psdc._bgmVol = _bgmSlider.value;
        psdc._voiceVol = _voiceSlider.value;
        //ゲームマネージャーからデーター取得
        var gm = GetComponent<GameManager>();
        psdc._score = gm._playerScore;
        psdc._deathcount = gm._pDeathCount;
        psdc._elapsedtime = gm._elapsedTime;
        //JSON化
        jsonData = JsonUtility.ToJson(psdc);
        //データのセーブ
        PlayerPrefs.SetString("userDatas", jsonData);
        Debug.Log($"プレイヤーデータ{PlayerPrefs.GetString("userDatas")}");
    }
    public void ResetDatas()
    {
        PlayerPrefs.DeleteAll();
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
