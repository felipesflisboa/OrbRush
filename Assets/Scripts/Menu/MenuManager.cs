using UnityEngine;
using System.Collections;
using System.Linq;
using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine.UI;

/// <summary>
/// Sample option class.
/// Version 1.2
/// </summary>
public class MenuManager : SingletonMonoBehaviour<MenuManager> {
	[SerializeField] AudioSource clickSFX;
    [SerializeField] Button playButton;
    [SerializeField] Button infoButton;
    [SerializeField] Button localHighScoresButton;
    MenuPanelType currentPanelOption;
	MenuPanel[] panelArray;
    Fader fader;
	Timer clickCooldownTimer;

    bool FadeActive =>  fader != null && fader.ImageActive;
    bool ShouldReturnToTitleAtClick => !new[] { MenuPanelType.Title, MenuPanelType.Loading }.Contains(currentPanelOption);

    void Awake() {
        fader = GetComponentInChildren<Fader>(true);
        panelArray = GetComponentsInChildren<MenuPanel>(true);
        AssignButtonListeners();
    }

    void AssignButtonListeners() {
        playButton.onClick.AddListener(OnPlayClick);
        infoButton.onClick.AddListener(OnInfoClick);
        localHighScoresButton.onClick.AddListener(OnLocalHighScoresClick);
    }

    void Start () {
        clickCooldownTimer = new Timer(0.75f);
        EnablePanel(SimpleScoreListTimedDrawer.lastScore == null ? MenuPanelType.Title : MenuPanelType.LocalHighScores);
	}

    async Task EnablePanel(MenuPanelType type) {
        if (fader != null && Time.timeSinceLevelLoad > 0.1f) 
            await fader.FadeOut().WaitForCompletion();
        currentPanelOption = type;
        foreach (MenuPanel panel in panelArray)
            panel.gameObject.SetActive(panel.type == type);
        if (fader != null && Time.timeSinceLevelLoad > 0.1f)
            await fader.FadeIn().WaitForCompletion();
    }

    public void PlayClickSFX(){
        if (clickSFX == null)
            return;
        clickSFX.Play();
    }

#region Buttons
	public void OnPlayClick(){
        OnPlay();
	}
	public void OnInfoClick(){
		PlayClickSFX();
		EnablePanel(MenuPanelType.Info);
	}
    public void OnLocalHighScoresClick() {
        PlayClickSFX();
        EnablePanel(MenuPanelType.LocalHighScores);
    }
    public void OnExitClick(){
		PlayClickSFX();
        System.Action action = () => {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        };
        if (fader == null) {
            fader.FadeOut(action);
        } else {
            action();
        }
	}
#endregion

    async void OnPlay() {
        PlayClickSFX();
        SimpleScoreListTimedDrawer.lastScore = null;
        await EnablePanel(MenuPanelType.Loading);
        await new WaitForUpdate();
        GameManager.level = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

	void Update(){
		if(Input.GetButtonDown("Fire1") && !FadeActive && ShouldReturnToTitleAtClick && clickCooldownTimer.CheckAndUpdate()) {
            PlayClickSFX();
            BackToTitle();
        }
	}

    public void BackToTitle() {
        EnablePanel(MenuPanelType.Title);
    }
}
