using UnityEngine;

public class Gate1Script : MonoBehaviour
{
    [SerializeField] private string key;
    [SerializeField] private string author;
    private AudioSource closedSound;
    void Start()
    {
        closedSound = GetComponent<AudioSource>();
        GameState.AddChangeListener(
            OnSoundsVolumeChanged,
            nameof(GameState.effectsVolume));

        GameState.AddChangeListener(
            OnSoundsVolumeChanged,
            nameof(GameState.isSoundsMuted));
    }

    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Character")
        {
            MessageScript.ShowMessage($"Необхідний ключ №{key}. Продовжуйте пошуки!", author);

            closedSound.volume = GameState.isSoundsMuted
            ? 0.0f
            : GameState.effectsVolume;
            closedSound.Play();
        }
    }
    private void OnSoundsVolumeChanged(string name)
    {
        closedSound.volume = GameState.isSoundsMuted
            ? 0.0f
            : GameState.effectsVolume;
    }

    private void OnDestroy()
    {
        GameState.RemoveChangeListener(
            OnSoundsVolumeChanged,
            nameof(GameState.effectsVolume));
        GameState.RemoveChangeListener(
            OnSoundsVolumeChanged,
            nameof(GameState.isSoundsMuted));
    }
}
