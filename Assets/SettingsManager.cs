using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    private Slider musicSlider, sfxSlider;
    private Toggle vsAiToggle;

    private PaddleScript leftPaddleScript;

    [SerializeField]
    private AudioMixer mixer;

    #region Constants
    private const string MIXER_MUSIC = "Music Volume";
    private const string MIXER_SFX = "SFX Volume";
    #endregion

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("Music"))
        {
            defaultSettings();
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "TitleScene")
        {
            GameObject[] settings = GameObject.FindGameObjectsWithTag("Setting");
            musicSlider = settings[1].GetComponent<Slider>();
            sfxSlider = settings[0].GetComponent<Slider>();
            vsAiToggle = settings[2].GetComponent<Toggle>();

            musicSlider.value = getMusic();
            sfxSlider.value = getSfx();
            vsAiToggle.isOn = getVsAi();
        }
        else if (SceneManager.GetActiveScene().name == "GameScene")
        {
            leftPaddleScript = GameObject.FindGameObjectWithTag("Left").GetComponent<PaddleScript>();

            leftPaddleScript.setAiDiff(getVsAi() ? PaddleScript.MEDIUM : PaddleScript.HUMAN);
        }
    }

    private void Start()
    {
        mixer.SetFloat(MIXER_MUSIC, getMusic());
        mixer.SetFloat(MIXER_SFX, getSfx());
    }


    #region Non-Unity Methods
    public void adjustMusic(float volume)
    {
        PlayerPrefs.SetFloat("Music", volume);
        mixer.SetFloat(MIXER_MUSIC, volume);
    }

    public void adjustSfx(float volume)
    {
        PlayerPrefs.SetFloat("SFX", volume);
        mixer.SetFloat(MIXER_SFX, volume);
    }

    public void toggleVsAi(bool state)
    {
        PlayerPrefs.SetInt("vsAI", state ? 1 : 0);
    }

    public float getMusic()
    {
        return PlayerPrefs.GetFloat("Music");
    }

    public float getSfx()
    {
        return PlayerPrefs.GetFloat("SFX");
    }

    public bool getVsAi()
    {
        return PlayerPrefs.GetInt("vsAI") == 1;
    }

    [ContextMenu("Reset Settings")]
    public void defaultSettings()
    {
        PlayerPrefs.SetFloat("Music", 0f);
        PlayerPrefs.SetFloat("SFX", 0f);
        PlayerPrefs.SetInt("vsAI", 0);
    }
    #endregion
}
