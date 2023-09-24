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
    /// <summary>名前入力フィールド</summary>
    [SerializeField] InputField _inputFeild;
    /// <summary>プレイヤー名</summary>
    [SerializeField] Text _playerName;
    [SerializeField] GameManager _gm;
    string jsonData = string.Empty;
    private void Start()
    {
        StreamReader reader = new StreamReader(Application.dataPath + "/test.json");
        string data = reader.ReadToEnd();
        reader.Close();
        Debug.Log($"JSONファイルからの読み込みデーター：{data}");
        PlayerSaveDataContainer psdc =
            JsonUtility.FromJson<PlayerSaveDataContainer>(data);
        //オーディオミキサー音量の設定
        _aMixer.SetFloat("MasterVol", psdc._masterVol);
        _aMixer.SetFloat("BGMVol", psdc._bgmVol);
        _aMixer.SetFloat("VoiceVol", psdc._voiceVol);
        _masterSlider.value = psdc._masterVol;
        _bgmSlider.value = psdc._bgmVol;
        _voiceSlider.value = psdc._voiceVol;
        //プレイヤー名設定
        _playerName.text = psdc._playerName;
        //ゲームマネージャーのデーター初期化
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
        //JSON化
        jsonData = JsonUtility.ToJson(psdc);
        //データ書き込み
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
        //ゲームマネージャーからデーター取得
        Debug.Log($"GMからのデータ{_gm._playerScore},{_gm._pDeathCount},{_gm._elapsedTime}");
        psdc._score = int.Parse(_pScr.text);
        psdc._deathcount = int.Parse(_dCnt.text);
        psdc._elapsedtime = float.Parse(_eTime.text);
        //JSON化
        jsonData = JsonUtility.ToJson(psdc);
        //データ書き込み
        StreamWriter writer = new StreamWriter(Application.dataPath + "/test.json");
        writer.WriteLine(jsonData);
        writer.Flush();
        writer.Close();
    }
    public void SetPlayerDatas()
    {
        //データ読み込み
        StreamReader reader = new StreamReader(Application.dataPath + "/test.json");
        string data = reader.ReadToEnd();
        reader.Close();
        PlayerSaveDataContainer psdcFromJson =
            JsonUtility.FromJson<PlayerSaveDataContainer>(data);
        Debug.Log($"psdcFromJSON：{data}");
        //データ初期化とインスタンス化
        PlayerSaveDataContainer psdc = new PlayerSaveDataContainer();
        psdc._playerName = _playerName.text;
        //音量の値の比較と初期化
        psdc._masterVol = 
            (psdcFromJson._masterVol != psdc._masterVol) 
            ? _masterSlider.value : psdcFromJson._masterVol;
        psdc._bgmVol = 
            (psdcFromJson._bgmVol != psdc._bgmVol) 
            ? _bgmSlider.value : psdcFromJson._bgmVol;
        psdc._voiceVol = 
            (psdcFromJson._voiceVol != psdc._voiceVol) 
            ? _voiceSlider.value : psdcFromJson._voiceVol;
        //ゲームマネージャーからデーター取得と初期化
        Debug.Log($"GMからのデータ{_gm._playerScore},{_gm._pDeathCount},{_gm._elapsedTime}");
        psdc._score = int.Parse(_pScr.text);
        psdc._deathcount = int.Parse(_dCnt.text);
        psdc._elapsedtime = float.Parse(_eTime.text);
        //JSON化
        jsonData = JsonUtility.ToJson(psdc);
        //データ書き込み
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
