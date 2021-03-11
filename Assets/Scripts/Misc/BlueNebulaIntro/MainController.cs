using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BlueNebula.Intro {
    public class MainController : MonoBehaviour {
        SpaceController spaceController;
        [SerializeField] Transform shootingStarTransform;
        Vector3 shootingStarStartLocalPos;
        [SerializeField] AudioSource shootingStarSFX;
        [SerializeField] SpriteRenderer fadeMaskRenderer;
        [SerializeField] string nextSceneName;

        const float INITIAL_ZOOM = 8;

        void Awake() {
            spaceController = GetComponentInChildren<SpaceController>(true);
            shootingStarStartLocalPos = shootingStarTransform.transform.localPosition;
        }

        IEnumerator Start() {
            FadeScreen(1f);
            spaceController.RefreshItems(INITIAL_ZOOM);
            yield return null;
            yield return FadeScreenRoutine(1f, true);
            yield return ZoomOutRoutine(3f);
            yield return ShootingStarRoutine(0.5f);
            yield return new WaitForSeconds(0.1f);
            yield return FadeScreenRoutine(0.5f, false);
            if (!string.IsNullOrEmpty(nextSceneName))
                SceneManager.LoadScene(nextSceneName);
        }

        IEnumerator FadeScreenRoutine(float duration, bool isFadeIn) {
            return Tween(isFadeIn ? 1 : 0, isFadeIn ? 0 : 1, duration, FadeScreen, GetLinearEase);
        }

        IEnumerator ZoomOutRoutine(float duration) {
            return Tween(INITIAL_ZOOM, 1f, duration, spaceController.RefreshItems, GetEaseOutExpo);
        }

        IEnumerator ShootingStarRoutine(float duration) {
            shootingStarSFX.Play();
            return Tween(0f, 1f, duration, MoveShootingStar, GetLinearEase);
        }

        /// <summary>
        /// Simple tween
        /// </summary>
        /// <param name="start">Initial float</param>
        /// <param name="end">Final float</param>
        /// <param name="duration">Duration</param>
        /// <param name="action">Action to be called with float parameter</param>
        /// <param name="easeAction">Ease algorithm</param>
        /// <returns></returns>
        IEnumerator Tween(float start, float end, float duration, Action<float> action, Func<float, float> easeAction) {
            float ratioGain = 1f/duration;
            float ratioCounter = 0f;
            do {
                yield return null;
                ratioCounter = Mathf.Clamp01(ratioGain*Time.deltaTime + ratioCounter);
                action(start + easeAction(ratioCounter)* (end-start));
            } while (ratioCounter < 1f);
        }

        void MoveShootingStar(float ratio) {
            shootingStarTransform.localPosition = (1 - 2 * ratio) * shootingStarStartLocalPos;
        }

        void FadeScreen(float alpha) {
            fadeMaskRenderer.color = new Color(fadeMaskRenderer.color.r, fadeMaskRenderer.color.g, fadeMaskRenderer.color.b, alpha);
        }

        float GetEaseOutExpo(float x) {
            return x >= 1f ? 1f : 1f - Mathf.Pow(2f, -10f * x);
        }

        float GetLinearEase(float x) {
            return x;
        }
    }
}