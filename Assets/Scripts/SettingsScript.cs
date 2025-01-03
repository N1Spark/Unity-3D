using UnityEngine;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    private GameObject content;

    #region soundEffectsSlider

    private Slider soundEffectsSlider;
    private float soundEffectsSliderDefault;
    private void initSoundEffectsSlider()
    {
        soundEffectsSlider = transform
            .Find("Content/Sound/EffectsSlider")
            .GetComponent<Slider>();
        
        soundEffectsSliderDefault = soundEffectsSlider.value;

        if (PlayerPrefs.HasKey(nameof(soundEffectsSlider)))
        {
            soundEffectsSlider.value = PlayerPrefs.GetFloat(
                nameof(soundEffectsSlider));
        }
        OnSoundEffectsChanged(soundEffectsSlider.value);
        //GameState.effectsVolume = soundEffectsSlider.value;
    }
    public void OnSoundEffectsChanged(System.Single value)
    {
        GameState.effectsVolume = value;
    }

    #endregion

    #region soundAmbientSlider

    private Slider soundAmbientSlider;
    private float soundAmbientSliderDefault;
    private void initSoundAmbientSlider()
    {
        soundAmbientSlider = transform
            .Find("Content/Sound/AmbientSlider")
            .GetComponent<Slider>();
        soundAmbientSliderDefault = soundAmbientSlider.value;

        if (PlayerPrefs.HasKey(nameof(soundAmbientSlider)))
        {
            soundAmbientSlider.value = PlayerPrefs.GetFloat(
                nameof(soundAmbientSlider));
        }
        OnSoundEffectsChanged(soundAmbientSlider.value);
    }
    public void OnSoundAmbientChanged(System.Single value)
    {
        GameState.ambientVolume = value;
    }

    #endregion

    #region soundMusicSlider

    private Slider soundMusicSlider;
    private float soundMusicSliderDefault;
    private void initSoundMusicSlider()
    {
        soundMusicSlider = transform
            .Find("Content/Sound/MusicSlider")
            .GetComponent<Slider>();

        soundMusicSliderDefault = soundMusicSlider.value;

        if (PlayerPrefs.HasKey(nameof(soundMusicSlider)))
        {
            soundMusicSlider.value = PlayerPrefs.GetFloat(
                nameof(soundMusicSlider));
        }
        OnSoundEffectsChanged(soundMusicSlider.value);
    }
    public void OnSoundMusicChanged(System.Single value)
    {
        GameState.musicVolume = value;
    }

    #endregion

    #region soundsMuteToggle

    private Toggle soundsMuteToggle;
    private bool soundsMuteToggleDefault;
    private void initSoundsMuteToggle()
    {
        soundsMuteToggle = transform
            .Find("Content/Sound/MuteToggle")
            .GetComponent<Toggle>();
        soundsMuteToggleDefault = soundsMuteToggle.isOn;

        if (PlayerPrefs.HasKey(nameof(soundsMuteToggle)))
        {
            soundsMuteToggle.isOn = PlayerPrefs.GetInt(
                nameof(soundsMuteToggle)) > 0;
        }

        OnSoundsMuteToggle(soundsMuteToggle.isOn);
    }
    public void OnSoundsMuteToggle(System.Boolean value)
    {
        GameState.isSoundsMuted = value;
    }

    #endregion

    #region Camera Tilt Limits
    private Slider minTiltSlider;
    private Slider maxTiltSlider;

    private void initCameraTiltLimits()
    {
        minTiltSlider = transform
            .Find("Content/Controls/MinTiltSlider")
            .GetComponent<Slider>();
        maxTiltSlider = transform
            .Find("Content/Controls/MaxTiltSlider")
            .GetComponent<Slider>();

        minTiltSlider.value = Mathf.InverseLerp(-90f, 0f, GameState.cameraMinTilt);
        maxTiltSlider.value = Mathf.InverseLerp(0f, 90f, GameState.cameraMaxTilt);

        OnMinTiltSlider(minTiltSlider.value);
        OnMaxTiltSlider(maxTiltSlider.value);
    }

    public void OnMinTiltSlider(float value)
    {
        GameState.cameraMinTilt = Mathf.Lerp(-90f, 0f, value);
    }

    public void OnMaxTiltSlider(float value)
    {
        GameState.cameraMaxTilt = Mathf.Lerp(0f, 90f, value);
    }
    #endregion

    void Start()
    {
        initSoundEffectsSlider();
        initSoundAmbientSlider();
        initSoundsMuteToggle();
        initSoundMusicSlider();
        initControlsSensitivity();
        initControlsFpv();

        content = transform
            .Find("Content")
            .gameObject;

        if (content.activeInHierarchy)
        {
            Time.timeScale = 0.0f;
        }
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Time.timeScale = content.activeInHierarchy ? 1.0f : 0.0f;
            content.SetActive(!content.activeInHierarchy);
        }
    }


    private Slider sensXSlider;
    private Slider sensYSlider;
    private Toggle linkToggle;
    private bool isLinked = true;
    private void initControlsSensitivity()
    {
        linkToggle = transform
            .Find("Content/Controls/LinkToggle")
            .GetComponent<Toggle>();

        sensXSlider = transform
            .Find("Content/Controls/SensXSlider")
            .GetComponent<Slider>();

        sensYSlider = transform
            .Find("Content/Controls/SensYSlider")
            .GetComponent<Slider>();
        OnLinkToggle(linkToggle.isOn);
        OnSensXChanged(sensXSlider.value);
        if (!isLinked) OnSensYChanged(sensYSlider.value);
    }

    public void OnSensXChanged(System.Single value)
    {
        float sens = Mathf.Lerp(0.01f, 0.1f, value);
        GameState.lookSensitivityX = sens;
        if (isLinked)
        {
            sensYSlider.value = value;
            GameState.lookSensitivityY = -sens;
        }
    }
    public void OnSensYChanged(System.Single value)
    {
        float sens = Mathf.Lerp(-0.01f, -0.1f, value);
        GameState.lookSensitivityY = sens;
        if (isLinked)
        {
            sensXSlider.value = value;
            GameState.lookSensitivityX = -sens;
        }
    }
    public void OnLinkToggle(System.Boolean value)
    {
        isLinked = value;
    }



    #region FPV Limit

    private Slider fpvSlider;

    private void initControlsFpv()
    {
        fpvSlider = transform
        .Find("Content/Controls/FPVSlider")
            .GetComponent<Slider>();
        OnFpvSlider(GameState.fpvRange);
    }
    public void OnFpvSlider(System.Single value)
    {
        GameState.fpvRange = Mathf.Lerp(0.3f, 1.1f, value);
    }
    #endregion


    #region Buttons
    public void OnSaveButtonClick()
    {
        PlayerPrefs.SetFloat(nameof(soundEffectsSlider), soundEffectsSlider.value);
        PlayerPrefs.SetFloat(nameof(soundAmbientSlider), soundAmbientSlider.value);
        PlayerPrefs.SetFloat(nameof(soundMusicSlider), soundMusicSlider.value);
        PlayerPrefs.SetInt(nameof(soundsMuteToggle), soundsMuteToggle.isOn ? 1 : 0);
        PlayerPrefs.SetFloat(nameof(sensXSlider), sensXSlider.value);
        PlayerPrefs.SetFloat(nameof(sensYSlider), sensYSlider.value);
        PlayerPrefs.SetFloat(nameof(fpvSlider), fpvSlider.value);
        PlayerPrefs.Save();
    }

    public void OnExitButtonClick()
    {
        Time.timeScale = 1.0f;
        content.SetActive(false);
    }

    public void OnDefaultsButtonClick()
    {
        soundEffectsSlider.value = soundEffectsSliderDefault;
        soundAmbientSlider.value = soundAmbientSliderDefault;
        soundMusicSlider.value = soundMusicSliderDefault;
        soundsMuteToggle.isOn = soundsMuteToggleDefault;
    }
    #endregion
}
