using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//TODO SpaceController and MainController. SpaceController is the pivot
namespace BlueNebula.Intro {
    public class Controller : MonoBehaviour {
        [SerializeField] Transform pivot;
        [SerializeField] Transform shootingStarTransform;
        Vector3 shootingStarStartPos;
        [SerializeField] AudioSource shootingStarSFX;
        [SerializeField] SpriteRenderer spriteRenderer;
        [SerializeField] string nextSceneName;
        Item[] itemArray;

        const float INITIAL_ZOOM = 8;

        void Awake() {
            shootingStarStartPos = shootingStarTransform.transform.position;
            itemArray = GetComponentsInChildren<Item>();
            foreach (var item in itemArray)
                item.Setup(pivot.transform.position);
        }

        IEnumerator Start() {
            FadeScreen(1f);
            RefreshItems(INITIAL_ZOOM);
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
            return Tween(INITIAL_ZOOM, 1f, duration, RefreshItems, GetEaseOutExpo);
        }

        IEnumerator ShootingStarRoutine(float duration) {
            shootingStarSFX.Play();
            return Tween(0f, 1f, duration, MoveShootingStar, GetLinearEase);
        }

        /* //remove
        IEnumerator ZoomOut(float start, float end, float duration, float delay, Func<float, float> easeAction) {
            RefreshItems(start);
            yield return new WaitForSeconds(delay);
            yield return Tween(start, end, duration, RefreshItems, easeAction);
        }
        */

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
            /*//remove
             * 
            float step = (end - start)/ duration;
            float counter = start;
            do {
                counter = Mathf.Min(end, counter + step);
                action((counter-start)/(end-start));
                yield return null;
            } while (counter < end);
             */
        }

        void RefreshItems(float zoomMultiplier) {
            Debug.Log("zoomMultiplier="+zoomMultiplier); //remove
            foreach (var item in itemArray)
                item.Refresh(zoomMultiplier);
        }

        void MoveShootingStar(float ratio) {
            shootingStarTransform.position = (1 - 2 * ratio) * (shootingStarStartPos - pivot.position) + pivot.position;
        }

        void FadeScreen(float alpha) {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
        }

        float GetEaseOutQuad(float x) {
            return 1f - (1f - x) * (1f - x);
        }

        float GetEaseOutExpo(float x) {
            return x >= 1f ? 1f : 1f - Mathf.Pow(2f, -10f * x);
        }

        float GetEaseInOutExpo(float x) {
            return x <= 0f ? 0f : (
                x >= 1f ? 1f : (
                    x < 0.5f ? Mathf.Pow(2f, 20f * x - 10f) / 2f : (2f - Mathf.Pow(2f, -20f * x + 10f)) / 2f
                )
            );
        }

        float GetLinearEase(float x) {
            return x;
        }
    }
}