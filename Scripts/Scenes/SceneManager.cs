using System;
using System.Collections;
using System.Collections.Generic;
using ToolKit.Easing;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace ToolKit.Scenes
{
    public class SceneManager : MonoBehaviour
    {
        public static SceneManager Current;
        [SerializeField] private GameObject _loadingScreenPrefab;
        [SerializeField] private GameObject _loadingScreenBackground;
        [SerializeField] private GameObject _loadingScreenContent;
        [SerializeField] private AnimationCurve _transitionCurve;
        [SerializeField] private float _transitionTime;
        
        [SerializeField] private Text _hintField;
        [SerializeField] private List<string> _hints = new List<string>();
        private void Awake()
        {
            SceneManager[] objs = FindObjectsOfType<SceneManager>();

            if (objs.Length > 1)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
            Current = this;
        }

        private void Start()
        {
            Image[] images = _loadingScreenContent.transform.GetComponentsInChildren<Image>();
            Text[] texts = _loadingScreenContent.transform.GetComponentsInChildren<Text>();

            foreach (var image in images)
            {
                image.enabled = true;
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
            }

            foreach (var text in texts)
            {
                text.enabled = true;
                text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
            }
        }

        /// <summary>
        /// Quits the game
        /// </summary>
        public void Quit()
        {
            Application.Quit();
        }

        /// <summary>
        /// Opens a scene directly
        /// </summary>
        /// <param name="sceneIndex">Index of the scene</param>
        public void OpenScene(int sceneIndex)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
        }

        /// <summary>
        /// Starts a coroutine that loads the scene on the background while a loading screen with hints popsup
        /// </summary>
        /// <param name="sceneIndex">Index of the scene</param>
        public void StartAsyncLoad(int sceneIndex)
        {
            _hintField.text = _hints[Random.Range(0, _hints.Count)];
            _loadingScreenPrefab.gameObject.SetActive(true);
            StartCoroutine(Ease.Fade(_loadingScreenBackground.transform, false, EasingCurve.Smooth(), _transitionTime,
                () =>
                {
                    StartCoroutine(Ease.Fade(_loadingScreenContent.transform, false, EasingCurve.Smooth(),
                        _transitionTime,
                        () => { StartCoroutine(LoadSceneAsync(sceneIndex)); }));
                }));
        }

        /// <summary>
        /// Coroutine for the asyncload that keeps track of the loading bar
        /// </summary>
        /// <param name="sceneIndex">Index of the scene</param>
        /// <returns></returns>
        private IEnumerator LoadSceneAsync(int sceneIndex)
        {
            Slider loadingBar = null;
            if (_loadingScreenPrefab)
            {
                loadingBar = _loadingScreenPrefab.GetComponentInChildren<Slider>();
            }

            AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneIndex);
            while (!asyncLoad.isDone)
            {
                yield return null;
                Debug.Log(asyncLoad.progress);
                if (loadingBar != null)
                {
                    loadingBar.value = asyncLoad.progress;
                }
            }

            StartCoroutine(Ease.Fade(_loadingScreenContent.transform, true, _transitionCurve, _transitionTime,
                () =>
                {
                    if (loadingBar != null)
                    {
                        loadingBar.value = 0;
                    }

                    StartCoroutine(Ease.Fade(_loadingScreenBackground.transform, true, _transitionCurve,
                        _transitionTime,
                        () => { _loadingScreenPrefab.gameObject.SetActive(false); }));
                }));
        }
    }
}