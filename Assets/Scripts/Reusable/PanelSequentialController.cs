using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

/// <summary>
/// Sample script for navigating into sequential subpanels by pressing a game button.
/// Just put this script into a panel and it automatic detect and organize the subpanels. 
/// Version 1.1
/// </summary>
public class PanelSequentialController : MonoBehaviour {
	[SerializeField] string sceneToLoadWhenEndName; 
	[SerializeField] string forwardButtonName = "Fire1"; 
	[SerializeField] string backwardButtonName;
    [SerializeField] Fader fader;
	[SerializeField] AudioSource forwardClickSFX;
	[SerializeField] AudioSource backwardClickSFX;
	[SerializeField] float clickCooldownMin = 0.25f; // Cooldown min between clicks.

	RectTransform[] panelArray;
	int index;
	Timer clickCooldownTimer;

	void Start () {
		clickCooldownTimer = new Timer(clickCooldownMin);
		panelArray = CreatePanelArray();
		StartCoroutine(RefreshPanels());
	}

    // Try to get all childs as panels
    RectTransform[] CreatePanelArray() {
        List<RectTransform> panelList = new List<RectTransform>();
        foreach (Transform child in transform) {
            RectTransform subpanel = child as RectTransform;
            if (subpanel != null)
                panelList.Add(subpanel);
        }
        return panelList.ToArray();
    }

	void Update(){
        if (fader==null ||  !fader.ImageActive) {
            if (!string.IsNullOrEmpty(forwardButtonName) && Input.GetButtonDown(forwardButtonName) && clickCooldownTimer.CheckAndUpdate())
                GoFoward();
            if (!string.IsNullOrEmpty(backwardButtonName) && Input.GetButtonDown(backwardButtonName) && index > 0 && clickCooldownTimer.CheckAndUpdate())
                GoBackward();
        }
	}

    void GoFoward() {
        if (forwardClickSFX != null)
            forwardClickSFX.Play();
        index++;
        if (index == panelArray.Length)
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoadWhenEndName);
        else
            StartCoroutine(RefreshPanels());
    }

    void GoBackward() {
        if (backwardClickSFX != null)
            backwardClickSFX.Play();
        index--;
        StartCoroutine(RefreshPanels());
    }

    IEnumerator RefreshPanels() {
        if (fader != null && Time.timeSinceLevelLoad > 0.1f)
            yield return fader.FadeOut().WaitForCompletion();
        for (int i=0;i<panelArray.Length;i++)
			panelArray[i].gameObject.SetActive(i==index);
        if (fader != null && Time.timeSinceLevelLoad > 0.1f)
            yield return fader.FadeIn().WaitForCompletion();
    }
}