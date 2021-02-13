using UnityEngine;
using System.Collections;
using System.Linq;
using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine.UI;
using TMPro;

public class MainMenuManager : SingletonMonoBehaviour<MainMenuManager> {
    [SerializeField] AudioSource clickSFX;
    [SerializeField] MenuOption marathonOption;
    [SerializeField] MenuOption quickRaceOption;
    [SerializeField] MenuOption infoOption;
    [SerializeField] MenuOption localHighScoresOption;
    [SerializeField] TextMeshProUGUI versionText;
    MenuPanelType currentPanelType;
    MenuPanel[] panelArray;
    Fader fader;

    bool FadeActive => fader != null && fader.ImageActive;
    bool ShouldReturnToMenuAtClick => !new[] { MenuPanelType.Menu, MenuPanelType.Loading }.Contains(currentPanelType);
    string Version => string.Format("Version {0}", Application.version);

    bool CanPressBack {
        get {
            if (currentPanelType == MenuPanelType.Title) {
#if UNITY_WEBGL
                return false;
#else
                return true;
#endif
            }
            return ShouldReturnToMenuAtClick || currentPanelType == MenuPanelType.Menu;
        }
    }

    void Awake() {
        fader = GetComponentInChildren<Fader>(true);
        panelArray = GetComponentsInChildren<MenuPanel>(true);
        versionText.text = Version;
        AssignOptionListeners();
    }

    void AssignOptionListeners() {
        marathonOption.button.onClick.AddListener(OnMarathonClick);
        quickRaceOption.button.onClick.AddListener(OnQuickRaceClick);
        infoOption.button.onClick.AddListener(OnInfoClick);
        localHighScoresOption.button.onClick.AddListener(OnLocalHighScoresClick);
    }

    void Start() {
        Time.timeScale = 1;
        EnablePanel(ScoreListMarathonDrawer.lastScore == null ? MenuPanelType.Title : MenuPanelType.LocalHighScores);
        ClickLoop();
    }

    async Task EnablePanel(MenuPanelType type) {
        if (fader != null && Time.timeSinceLevelLoad > 0.1f)
            await fader.FadeOut().WaitForCompletion();
        currentPanelType = type;
        foreach (MenuPanel panel in panelArray)
            panel.gameObject.SetActive(panel.type == type);
        if (fader != null && Time.timeSinceLevelLoad > 0.1f)
            await fader.FadeIn().WaitForCompletion();
    }

    public void PlayClickSFX() {
        if (clickSFX == null)
            return;
        clickSFX.Play();
    }

    #region Buttons
    public void OnMarathonClick() {
        OnPlay(new MarathonData());
    }
    public void OnQuickRaceClick() {
        OnPlay(new QuickRaceData());
    }
    public void OnInfoClick() {
        PlayClickSFX();
        EnablePanel(MenuPanelType.Info);
    }
    public void OnLocalHighScoresClick() {
        PlayClickSFX();
        EnablePanel(MenuPanelType.LocalHighScores);
    }
    #endregion

    async void OnPlay(ModeData modeData) {
        PlayClickSFX();
        ScoreListMarathonDrawer.ClearLast();
        await EnablePanel(MenuPanelType.Loading);
        await new WaitForUpdate();
        GameManager.modeData = modeData;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    async void ClickLoop() {
        while (true) {
            await new WaitForUpdate();
            if (!FadeActive) {
                if (Input.GetButtonDown("Click") && ShouldReturnToMenuAtClick) {
                    EnablePanel(MenuPanelType.Menu);
                    await OnPanelClick();
                }
                if (Input.GetKeyDown(KeyCode.Escape) && CanPressBack) {
                    Back();
                    await OnPanelClick();
                }
            }
        }
    }

    void Back() {
        switch (currentPanelType) {
            case MenuPanelType.Title:
                Exit();
                break;
            case MenuPanelType.Menu:
                EnablePanel(MenuPanelType.Title);
                break;
            default:
                EnablePanel(MenuPanelType.Menu);
                break;
        }
    }

    async Task OnPanelClick() {
        PlayClickSFX();
        await new WaitForSeconds(0.75f);
    }

    void Exit() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif !UNITY_WEBGL && !UNITY_IOS
		    Application.Quit();
#endif
    }
}
