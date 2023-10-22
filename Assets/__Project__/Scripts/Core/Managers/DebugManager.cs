using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Managers
{
    public class DebugManager : MonoBehaviour
    {
        #region PRIVATE_VARIABLES

        private Coroutine _fpsMeasurementCoroutine;

        #endregion

        #region EVENTS

        public Action<int> FPSUpdated;

        #endregion

        #region MONO

        private void Start()
        {
            Init();
        }

        private void OnDisable()
        {
            StopFPSMeasurement();
        }

        #endregion

        #region PUBLIC_FUNCTIONS

        public void Reload()
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name, LoadSceneMode.Single);
        }

        #endregion

        #region PRIVATE_FUNCTIONS

        private void Init()
        {
            StartFPSMeasurement();
        }

        private void StartFPSMeasurement()
        {
            if (_fpsMeasurementCoroutine != null) StopCoroutine(_fpsMeasurementCoroutine);
            _fpsMeasurementCoroutine = StartCoroutine(FPSMeasurementCoroutine());
        }

        private void StopFPSMeasurement()
        {
            if (_fpsMeasurementCoroutine != null) StopCoroutine(_fpsMeasurementCoroutine);
            _fpsMeasurementCoroutine = null;
        }

        private IEnumerator FPSMeasurementCoroutine()
        {
            int frameCounter = 0;
            float timeCounter = 0.0f;

            const float refreshTime = 0.5f;

            while (true)
            {
                if (timeCounter < refreshTime)
                {
                    timeCounter += Time.deltaTime;
                    frameCounter++;
                }
                else
                {
                    // This code will break if you set your refreshTime to 0, which makes no sense.
                    float lastFramerate = (frameCounter / timeCounter);

                    frameCounter = 0;
                    timeCounter = 0.0f;

                    FPSUpdated?.Invoke((int)lastFramerate);
                }

                yield return null;
            }
        }

        #endregion
    }
}