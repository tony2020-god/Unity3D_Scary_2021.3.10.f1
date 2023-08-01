using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Networking;
using System.Collections;

public class AUDManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    public static AUDManager instance;

    [SerializeField] AudioSource mainAudioSource;
    [SerializeField] AudioSource ScendAudioSource;
    [SerializeField] GameObject playerObj;

    [SerializeField, Header("玩家聲音效")] AudioSource PlayerSound;

    [SerializeField, Header("人物/物品聲音效")] AudioSource grandmaSound;
    #region 人物/物品聲    
    [SerializeField, Tooltip("奶奶開始向前")] public AudioClip grandma_Starts_Walking;
    [SerializeField, Tooltip("奶奶詭異聲")] public AudioClip grandma_StrangeVoice;
    [SerializeField, Tooltip("扭動身體的音效")] public AudioClip body_Twisting_Sound;
    [SerializeField, Tooltip("模糊不清的人聲音")] public AudioClip muffled_Vocals;
    [SerializeField, Tooltip("腳步聲")] public AudioClip walking;
    [SerializeField, Tooltip("緊張呼吸聲")] public AudioClip strained_Breathing;
    [SerializeField, Tooltip("手電筒開關聲")] public AudioClip flashlight_Switch_Sound;
    [SerializeField, Tooltip("鬼影出現聲")] public AudioClip ghosting_Sound;
    [SerializeField, Tooltip("鬼影音效")] public AudioClip ghost_Sound;
    [SerializeField, Tooltip("鬼魂逃跑")] public AudioClip ghost_Escape;
    [SerializeField, Tooltip("門縫鬼影")] public AudioClip ghostIn_The_Door;
    #endregion  

    [SerializeField, Header("房間")] AudioSource[] roomSound;
    #region 房間  
    [SerializeField, Tooltip("開關燈")] AudioClip light_Switch_Sound;
    [SerializeField, Tooltip("抽屜")] AudioClip drawer_Opening_Sound;
    [SerializeField, Tooltip("床")] AudioClip getting_Out_Of_Bed;
    [SerializeField, Tooltip("衣櫃")] AudioClip the_Sound_Of_Opening_Wardrobes_And_Doors;
    [SerializeField, Tooltip("鑰匙")] AudioClip tet_Sound_Of_Get_The_Key;
    #endregion

    [SerializeField, Header("客廳")] AudioSource[] livingRoomSound;
    #region 客廳
    [SerializeField, Tooltip("金紙")] AudioClip[] gold_Paper;
    [SerializeField, Tooltip("時鐘")] AudioClip clock;
    [SerializeField, Tooltip("鋼琴")] AudioClip piano;
    [SerializeField, Tooltip("孝簾")] AudioClip filial_Piety_Curtain;
    [SerializeField, Tooltip("佛歌")] AudioClip buddhist_Song;
    [SerializeField, Tooltip("佛歌中斷")] AudioClip buddhist_Song_Stop;
    #endregion

    [SerializeField, Header("客廳加廚房")] AudioSource livingRoomPlusKitchen;
    #region 客廳&廚房
    [SerializeField, Tooltip("蠟燭燃燒")] AudioClip candle_Burning;
    [SerializeField, Tooltip("蠟燭吹襲熄聲")] AudioClip candle_Blowing_Sound;
    #endregion

    [SerializeField, Header("門")] AudioSource doorSound;
    #region 客廳+廁所
    [SerializeField, Tooltip("門解鎖聲")] AudioClip door_Unlock_Sound;
    [SerializeField, Tooltip("關門聲")] AudioClip door_Slam;
    [SerializeField, Tooltip("開門聲")] AudioClip door_Opening;
    #endregion

    [SerializeField, Header("廚房")] AudioSource footRiceSound;
    #region 廚房
    [SerializeField, Tooltip("腳尾飯")] AudioClip sound_Of_Something_Falling;
    #endregion

    [SerializeField, Header("廁所")] AudioSource bathroomSound;
    #region 廁所
    [SerializeField, Tooltip("水滴聲")] AudioClip dripping_Sound;
    [SerializeField, Tooltip("鬼手抓玩家聲")] AudioClip ghost_Hand_Catch_Player_Sound;
    [SerializeField, Tooltip("墜落聲")] AudioClip falling_Sound;
    [SerializeField, Tooltip("墜落後黑畫面")] AudioClip black_Screen_After_Fall;
    [SerializeField, Tooltip("轉水龍頭聲")] AudioClip turn_The_Tap;
    #endregion

    [SerializeField, Header("環境/其他")] AudioSource environmentOtherSound;
    #region 環境&其他
    [SerializeField, Tooltip("白噪音")] AudioClip white_Noise;
    [SerializeField, Tooltip("選單背景音樂")] AudioClip menu_Background_Music;
    [SerializeField, Tooltip("恐怖白噪音")] AudioClip horror_White_Noise;
    [SerializeField, Tooltip("遊戲開始")] AudioClip games_Start;
    [SerializeField, Tooltip("恐怖開始")] AudioClip horror_Start;
    [SerializeField, Tooltip("高音小提琴聲")] AudioClip soprano_Violin;
    [SerializeField, Tooltip("進入場景聲")] AudioClip enter_Scene_Sound;
    [SerializeField, Tooltip("UI內文")] AudioClip ui_Context;
    [SerializeField, Tooltip("墜落轉黑畫面聲")] AudioClip falling_To_Black_Screen_Sound;
    #endregion

    #region - 抓取音效區(陶宇測試中) -
    [SerializeField] string[] strSoundClipName;
    string soundFolderPath = @"D:\ScarySound\";
    AudioClip tempClip;

    void Start()
    {
        PlaySound(strSoundClipName);
    }

    void PlaySound(string[] soundFileName)
    {
        StartCoroutine(LoadSound(soundFileName));
    }

    IEnumerator LoadSound(string[] soundFileName)
    {
        int iCount = soundFileName.Length;
        string[] soundFilePaths = new string[iCount];
        UnityWebRequest[] unityWebRequests = new UnityWebRequest[iCount];

        for (int index = 0; index < iCount; index++)
        {
            // 組合路徑 + 檔名
            soundFilePaths[index] = soundFolderPath + soundFileName[index] + ".wav";

            // UnityWebRequest 抓取音效檔
            unityWebRequests[index] = UnityWebRequestMultimedia.GetAudioClip(soundFilePaths[index], AudioType.WAV);

            // 送出請求並等待回應
            yield return unityWebRequests[index].SendWebRequest();

            // 檢查是否有錯誤發生
            if (unityWebRequests[index].result == UnityWebRequest.Result.Success)
            {
                tempClip = DownloadHandlerAudioClip.GetContent(unityWebRequests[index]);
                Debug.Log(string.Format("抓取音效檔案 : {0} {1} 成功", index, soundFileName[index]));

                // 將讀取到的音效資料設定到 AudioClip 元件
                SetAudioClip(index, tempClip);
            }
            else
            {
                Debug.LogError(string.Format("無法讀取音效檔 ： {0} {1} 因為 {2}", index, soundFileName[index], unityWebRequests[index].error));
            }
        }
    }

    void SetAudioClip(int r_index, AudioClip audioClip)
    {
        switch (r_index)
        {
            case 0:
                grandma_Starts_Walking = audioClip;
                break;
            case 1:
                grandma_StrangeVoice = audioClip;
                break;
            case 2:
                body_Twisting_Sound = audioClip;
                break;
            case 3:
                muffled_Vocals = audioClip;
                break;
            case 4:
                walking = audioClip;
                break;
            case 5:
                strained_Breathing = audioClip;
                break;
            case 6:
                flashlight_Switch_Sound = audioClip;
                break;
            case 7:
                ghosting_Sound = audioClip;
                break;
            case 8:
                ghost_Sound = audioClip;
                break;
            case 9:
                ghost_Escape = audioClip;
                break;
            case 10:
                ghostIn_The_Door = audioClip;
                break;
            case 11:
                light_Switch_Sound = audioClip;
                break;
            case 12:
                drawer_Opening_Sound = audioClip;
                break;
            case 13:
                getting_Out_Of_Bed = audioClip;
                break;
            case 14:
                tet_Sound_Of_Get_The_Key = audioClip;
                break;
            case 15:
                clock = audioClip;
                break;
            case 16:
                piano = audioClip;
                break;
            case 17:
                filial_Piety_Curtain = audioClip;
                break;
            case 18:
                buddhist_Song_Stop = audioClip;
                break;
            case 19:
                candle_Blowing_Sound = audioClip;
                break;
            case 20:
                door_Unlock_Sound = audioClip;
                break;
            case 21:
                door_Slam = audioClip;
                break;
            case 22:
                door_Opening = audioClip;
                break;
            case 23:
                sound_Of_Something_Falling = audioClip;
                break;
            case 24:
                dripping_Sound = audioClip;
                break;
            case 25:
                ghost_Hand_Catch_Player_Sound = audioClip;
                break;
            case 26:
                falling_Sound = audioClip;
                break;
            case 27:
                black_Screen_After_Fall = audioClip;
                break;
            case 28:
                turn_The_Tap = audioClip;
                break;
            case 29:
                white_Noise = audioClip;
                break;
            case 30:
                menu_Background_Music = audioClip;
                break;
            case 31:
                horror_White_Noise = audioClip;
                break;
            case 32:
                games_Start = audioClip;
                break;
            case 33:
                horror_Start = audioClip;
                break;
            case 34:
                soprano_Violin = audioClip;
                break;
            case 35:
                enter_Scene_Sound = audioClip;
                break;
            case 36:
                ui_Context = audioClip;
                break;
            case 37:
                falling_To_Black_Screen_Sound = audioClip;
                break;
            default:
                break;
        }
        Debug.Log(string.Format("設定音效檔案 : {0} 成功", r_index));
    }
    #endregion

    public const string MUSIC_KEY = "musicVolume";

    public const string SFX_KEY = "sfxVolume";

    void Awake()
    {
        mainAudioSource = GetComponent<AudioSource>();
        Transform childTransform = transform.Find("SecondAudioSource");

        if (childTransform != null)
        {
            // 使用 GetComponent 方法獲取子物件上的 AudioSource 元件
            ScendAudioSource = childTransform.GetComponent<AudioSource>();
        }
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    IEnumerator RestoreVolume(float originalVolume)
    {
        yield return new WaitForSeconds(drawer_Opening_Sound.length); // 等待音效播放完畢

        mainAudioSource.volume = originalVolume; // 恢復原始音量
    }

    public void OpenTheDrawerSFX()
    {
        float originalVolume = mainAudioSource.volume;
        mainAudioSource.volume = originalVolume * 0.5f;
        mainAudioSource.PlayOneShot(drawer_Opening_Sound);

        StartCoroutine(RestoreVolume(originalVolume));
    }

    public void GetTheKeySFX()
    {
        ScendAudioSource.PlayOneShot(tet_Sound_Of_Get_The_Key);
    }

    public void PlayerDoorOpenSFX()
    {
        mainAudioSource.PlayOneShot(door_Opening);
    }

    public void PlayerDoorLockSFX()
    {
        mainAudioSource.PlayOneShot(door_Unlock_Sound);
    }

    public void PlayerLotusPaperSFX()
    {
        mainAudioSource.PlayOneShot(gold_Paper[Random.Range(0, 2)]);
    }

    public void PlayerLightSwitchSFX()
    {
        mainAudioSource.PlayOneShot(light_Switch_Sound);
    }

    public void PlayerFlashlighSFX()
    {
        mainAudioSource.PlayOneShot(flashlight_Switch_Sound);
    }

    public void PlayerGrandmaRushSFX()
    {
        mainAudioSource.PlayOneShot(grandma_Starts_Walking);
    }

    public void PlayerGameEventSFX()
    {
        mainAudioSource.PlayOneShot(ui_Context);
    }

    public void PlayerWhiteTentSFX()
    {
        mainAudioSource.PlayOneShot(filial_Piety_Curtain);
    }

    /// <summary>
    /// 存取紀錄
    /// </summary>
    public void LoadVolume()
    {
        float musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        float sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 1f);

        audioMixer.SetFloat(AudSetting.MIXER_MUSIC, Mathf.Log10(musicVolume) * 20);
        audioMixer.SetFloat(AudSetting.MIXER_SFX, Mathf.Log10(sfxVolume) * 20);
    }
}