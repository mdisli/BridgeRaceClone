using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class CreateLadder : MonoBehaviour
    {
        [SerializeField] private GameObject ladderPrefab;
        [SerializeField] private int count;
        [SerializeField] private Vector3 addpos;
        [SerializeField] private Transform startPos;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Create();
            }
        }

        void Create()
        {
            Vector3 startingPos = startPos.position;
            for (int i = 0; i < count; i++)
            {
                Instantiate(ladderPrefab, startingPos, Quaternion.identity, GameObject.Find("Ladder").transform);
                startingPos += addpos;
            }
        }
    }
}