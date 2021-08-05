using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class CollisionDetectorForFinish : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            GameManager.instance.playerState = GameManager.PlayerState.Finish;
            FindObjectOfType<UIManager>().finishText.text = other.transform.tag.ToUpper() + " Is Winner!!";
        }
    }
}