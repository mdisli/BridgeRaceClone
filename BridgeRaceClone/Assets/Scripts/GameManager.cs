using System;
using UnityEngine;
using  UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
        public static GameManager instance;
        public PlayerState playerState;
        public  enum PlayerState
        {
                Preparing,
                Playing,
                Finish,
        }

        private void Awake()
        {
                if (instance == null)
                {
                        instance = this;
                }
        }

        public void StartGame()
        {
                playerState = PlayerState.Playing;
        }

        public void RestartGame()
        {
                SceneManager.LoadScene(0);
        }

        private void Start()
        {
                playerState = PlayerState.Preparing;
        }
}