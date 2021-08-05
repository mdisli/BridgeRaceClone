using System;
using UnityEngine;
using TMPro;

namespace DefaultNamespace
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject prepareScreen, finishScreen;
        public TextMeshProUGUI finishText;

        private void Update()
        {
            CheckGameStatus();
        }

        void CheckGameStatus()
        {
            if (GameManager.instance.playerState == GameManager.PlayerState.Preparing)
            {
                prepareScreen.SetActive(true);
            }
            else
            {
                prepareScreen.SetActive(false);
            }

            if (GameManager.instance.playerState == GameManager.PlayerState.Finish)
            {
                finishScreen.SetActive(true);
            }
            else
            {
                finishScreen.SetActive(false);
            }
        }
    }
}