using UnityEngine;
using UnityEngine.UI;

public class SoundSettingsUI : MonoBehaviour
{
    [Header("Settings Modal Window")]
    [SerializeField] private GameObject settingsPanelBody;
    [SerializeField] private Button openSettingsButton;
    [SerializeField] private Button closeSettingsButton;

    [Header("Music Controls")]
    [SerializeField] private Button musicToggleButton;
    [SerializeField] private Image musicToggleImage;
    [SerializeField] private Sprite musicOnSprite;
    [SerializeField] private Sprite musicOffSprite;

    [Header("SFX Controls")]
    [SerializeField] private Button sfxToggleButton;
    [SerializeField] private Image sfxToggleImage;
    [SerializeField] private Sprite sfxOnSprite;
    [SerializeField] private Sprite sfxOffSprite;

    void Awake()
    {
        // Wire listeners dynamically (avoiding duplicates)
        if (openSettingsButton != null)
        {
            openSettingsButton.onClick.RemoveListener(OpenSettings);
            openSettingsButton.onClick.AddListener(OpenSettings);
        }

        if (closeSettingsButton != null)
        {
            closeSettingsButton.onClick.RemoveListener(CloseSettings);
            closeSettingsButton.onClick.AddListener(CloseSettings);
        }

        if (musicToggleButton != null)
        {
            musicToggleButton.onClick.RemoveListener(OnMusicToggleClicked);
            musicToggleButton.onClick.AddListener(OnMusicToggleClicked);
        }

        if (sfxToggleButton != null)
        {
            sfxToggleButton.onClick.RemoveListener(OnSFXToggleClicked);
            sfxToggleButton.onClick.AddListener(OnSFXToggleClicked);
        }
    }

    void OnEnable()
    {
        RefreshUI();
    }

    void Start()
    {
        RefreshUI();
    }

    public void OpenSettings()
    {
        if (settingsPanelBody != null)
        {
            settingsPanelBody.SetActive(true);
        }
        RefreshUI();
    }

    public void CloseSettings()
    {
        if (settingsPanelBody != null)
        {
            settingsPanelBody.SetActive(false);
        }
    }

    public void OnMusicToggleClicked()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.ToggleMusicMute();
            RefreshUI();
        }
    }

    public void OnSFXToggleClicked()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.ToggleSFXMute();
            RefreshUI();
        }
    }

    public void RefreshUI()
    {
        if (SoundManager.Instance == null) return;

        // Music Toggle (Muted = Off sprite, Unmuted = On sprite)
        bool isMusicMuted = SoundManager.Instance.IsMusicMuted();
        if (musicToggleImage != null)
        {
            Sprite targetSprite = isMusicMuted ? musicOffSprite : musicOnSprite;
            if (targetSprite != null)
            {
                musicToggleImage.sprite = targetSprite;
                musicToggleImage.overrideSprite = targetSprite;
            }
        }

        // SFX Toggle (Muted = Off sprite, Unmuted = On sprite)
        bool isSFXMuted = SoundManager.Instance.IsSFXMuted();
        if (sfxToggleImage != null)
        {
            Sprite targetSprite = isSFXMuted ? sfxOffSprite : sfxOnSprite;
            if (targetSprite != null)
            {
                sfxToggleImage.sprite = targetSprite;
                sfxToggleImage.overrideSprite = targetSprite;
            }
        }
    }
}
