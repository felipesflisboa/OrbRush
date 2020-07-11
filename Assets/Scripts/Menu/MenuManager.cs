using UnityEngine;
using System.Collections;
using System.Linq;
using DG.Tweening;

/// <summary>
/// Sample option class.
/// Version 1.2
/// </summary>
public class MenuManager : SingletonMonoBehaviour<MenuManager> {
	[SerializeField] AudioSource clickSFX;
    MenuPanelType currentPanelOption;
	MenuPanel[] panelArray;
    Fader fader;
	Timer clickCooldownTimer;

    bool ShouldReturnToTitleAtClick {
        get {
            return !new[] { MenuPanelType.Title, MenuPanelType.Rename, MenuPanelType.Loading }.Contains(currentPanelOption);
        }
    }

    bool FadeActive {
        get {
            return fader != null && fader.ImageActive;
        }
    }

    void Awake() {
        fader = GetComponentInChildren<Fader>(true);
        panelArray = GetComponentsInChildren<MenuPanel>(true);
    }

    void Start () {
		clickCooldownTimer = new Timer(0.75f);
        StartCoroutine(EnablePanel(SimpleScoreListTimedDrawer.lastScore == null ? MenuPanelType.Title : MenuPanelType.HighScores));
	}

	IEnumerator EnablePanel(MenuPanelType type) {
        if (fader != null && Time.timeSinceLevelLoad > 0.1f) 
            yield return fader.FadeOut().WaitForCompletion();
        currentPanelOption = type;
        foreach (MenuPanel panel in panelArray)
            panel.gameObject.SetActive(panel.type == type);
        if (fader != null && Time.timeSinceLevelLoad > 0.1f)
            yield return fader.FadeIn().WaitForCompletion();
    }

	public void PlayClickSFX(){
        if (clickSFX == null)
            return;
        clickSFX.Play();
    }

#region Buttons
	public void OnPlayClick(){
        StartCoroutine(PlayCoroutine());
	}
	public void OnInfoClick(){
		PlayClickSFX();
		StartCoroutine(EnablePanel(MenuPanelType.Info));
	}
	public void OnHighScoresClick(){
		PlayClickSFX();
        StartCoroutine(EnablePanel(MenuPanelType.HighScores));
    }
    public void OnLocalHighScoresClick() {
        PlayClickSFX();
        StartCoroutine(EnablePanel(MenuPanelType.LocalHighScores));
    }
    public void OnGameSparksHighScoresClick() {
        PlayClickSFX();
        StartCoroutine(EnablePanel(MenuPanelType.GameSparksHighScores));
    }
    public void OnRenameClick() {
        PlayClickSFX();
        StartCoroutine(EnablePanel(MenuPanelType.Rename));
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

    IEnumerator PlayCoroutine() {
        PlayClickSFX();
        SimpleScoreListTimedDrawer.lastScore = null;
        yield return EnablePanel(MenuPanelType.Loading);
        yield return null;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

	void Update(){
		if(Input.GetButtonDown("Fire1") && !FadeActive && ShouldReturnToTitleAtClick && clickCooldownTimer.CheckAndUpdate()) {
            PlayClickSFX();
            BackToTitle();
        }
	}

    public void BackToTitle() {
        StartCoroutine(EnablePanel(MenuPanelType.Title));
    }
}
