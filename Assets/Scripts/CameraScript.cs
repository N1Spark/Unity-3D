using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class CameraScript : MonoBehaviour
{
    private InputAction lookAction;
    private GameObject cameraPosition3;
    private GameObject character;
    private Vector3 c;
    private Vector3 cameraAngles, cameraAngles0;
    private bool isFpv;
    //private float sensitivityH = 0.15f;
    //private float sensitivityV = -0.08f;

    private float minDistance = 0.0f;
    //private float maxDistance = 10.0f;

    [SerializeField] private float minVerticalAngle = -60f;
    [SerializeField] private float maxVerticalAngle = 60f;

    [SerializeField] private Slider minAngleSlider;
    [SerializeField] private Slider maxAngleSlider;

    private AudioSource dayMusic;
    private AudioSource nightMusic;

    void Start()
    {
        lookAction = InputSystem.actions.FindAction("Look");
        character = GameObject.Find("Character");
        c = this.transform.position - character.transform.position;
        cameraPosition3 = GameObject.Find("CameraPosition");
        cameraAngles0 = cameraAngles = this.transform.eulerAngles;
        isFpv = true;

        if (minAngleSlider != null)
        {
            minAngleSlider.value = minVerticalAngle;
            minAngleSlider.onValueChanged.AddListener(UpdateMinVerticalAngle);
        }

        if (maxAngleSlider != null)
        {
            maxAngleSlider.value = maxVerticalAngle;
            maxAngleSlider.onValueChanged.AddListener(UpdateMaxVerticalAngle);
        }

        AudioSource[] audioSources = GetComponents<AudioSource>();
        if (audioSources == null || audioSources.Length != 2)
        {
            Debug.LogError("LightScript::Start audioSources error");
        }
        else
        {
            dayMusic = audioSources[0];
            nightMusic = audioSources[1];
        }

        SetLightMusic(true);

        GameState.AddChangeListener(
            OnSoundsVolumeChanged,
            nameof(GameState.musicVolume));

        GameState.AddChangeListener(
            OnSoundsVolumeChanged,
            nameof(GameState.isSoundsMuted));
    }

    void Update()
    {
        if (Time.timeScale == 0.0f) return;

        if (isFpv)
        {
            HandleFirstPersonView();
        }
        HandleCameraSwitch();
    }

    private void HandleFirstPersonView()
    {
        float wheel = Input.mouseScrollDelta.y;

        if (c.magnitude > GameState.fpvRange)
        {
            c *= 1 - wheel / 10.0f;
            if (c.magnitude <= GameState.fpvRange)
            {
                c *= 0.001f;
            }
        }
        else if (wheel < 0)
        {
            c *= GameState.fpvRange / c.magnitude;
            c *= 1 - wheel / 10.0f;
        }

        GameState.isFpv = c.magnitude < GameState.fpvRange;

        Vector2 lookValue = lookAction.ReadValue<Vector2>();
        cameraAngles.x += lookValue.y * GameState.lookSensitivityY;
        cameraAngles.y += lookValue.x * GameState.lookSensitivityX;

        cameraAngles.x = Mathf.Clamp(cameraAngles.x, minVerticalAngle, maxVerticalAngle);

        this.transform.eulerAngles = cameraAngles;

        this.transform.position = character.transform.position +
            Quaternion.Euler(0, cameraAngles.y - cameraAngles0.y, 0) * c;
    }
    private void HandleCameraSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isFpv = !isFpv;

            if (!isFpv)
            {
                this.transform.position = cameraPosition3.transform.position;
                this.transform.rotation = cameraPosition3.transform.rotation;
            }
            else
            {
                c = this.transform.position - character.transform.position;
            }
        }
    }

    private void OnSoundsVolumeChanged(string name)
    {
        dayMusic.volume =
            nightMusic.volume = GameState.isSoundsMuted
                ? 0.0f
                : GameState.musicVolume;
    }

    private void OnDestroy()
    {
        GameState.RemoveChangeListener(
            OnSoundsVolumeChanged,
            nameof(GameState.musicVolume));
        GameState.RemoveChangeListener(
            OnSoundsVolumeChanged,
            nameof(GameState.musicVolume));
    }

    public void UpdateMinVerticalAngle(float value)
    {
        minVerticalAngle = value;
        Debug.Log("Минимальный угол обновлен: " + minVerticalAngle);
    }
    public void UpdateMaxVerticalAngle(float value)
    {
        maxVerticalAngle = value;
        Debug.Log("Максимальный угол обновлен: " + maxVerticalAngle);
    }

    private void SetLightMusic(bool day)
    {
        if (day)
        {
            nightMusic.Stop();
            dayMusic.Play();
        }
        else
        {
            dayMusic.Stop();
            nightMusic.Play();
        }
    }
}
