using System;
using UnityEngine;

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

        private void Start()
        {
                //_playerState = PlayerState.Preparing;
                playerState = PlayerState.Playing;
        }
}