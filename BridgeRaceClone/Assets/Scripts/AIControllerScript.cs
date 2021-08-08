using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using  DG.Tweening;

namespace DefaultNamespace
{
    public class AIControllerScript : MonoBehaviour
    {
        [SerializeField] private List<GameObject> stickList;
        [SerializeField] private List<GameObject> reachableStickList;
        [SerializeField] private List<GameObject> sameColorList;

        [Header("--COLLECTING SETTINGS--")] [SerializeField]
        private string aiName;

        [SerializeField] private Transform stickBucket;
        [SerializeField] private float collectPossibility;
        [SerializeField] private float maxDistance;
        [SerializeField] private GameObject selectedBridge;
        [SerializeField] private Vector3 firstLadder;

        private GameObject stickHolder;
        private NavMeshAgent agent;
        private bool collecting = true;

        private void Awake()
        {
            stickHolder = GameObject.Find("Stick Holder");
            agent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            SelectBridge();
           
        }

        private void Update()
        {
            if (GameManager.instance.playerState == GameManager.PlayerState.Playing)
            {
                 
                InvokeRepeating("CheckReachableStick",2f,2.5f);
                InvokeRepeating("SelectStickToGo",1,2.5f);
                InvokeRepeating("CheckToGoBuild",1,5);


            }
            if (stickBucket.childCount == 0 && !collecting)
            {
                collecting = true;
                InvokeRepeating("CheckReachableStick",2f,2f);
                InvokeRepeating("SelectStickToGo",1,5f);
                InvokeRepeating("CheckToGoBuild",1,5);
            }
        }

        void AddStickToList()
        {
            stickList.Clear();
            for (int i = 0; i < stickHolder.transform.childCount; i++)
            {
                stickList.Add(stickHolder.transform.GetChild(i).gameObject);
            }
        }
        void CheckReachableStick()
        {
            AddStickToList();
            
            reachableStickList.Clear();
            foreach (GameObject stick in stickList)
            {
                if (Vector3.Distance(transform.position,stick.transform.position) <= maxDistance)
                {
                    reachableStickList.Add(stick);
                }
            }
            sameColorList.Clear();
            foreach (GameObject stick in reachableStickList)
            {
                if (stick.name == aiName + " Stick(Clone)")
                {
                    sameColorList.Add(stick);
                    reachableStickList.Remove(stick);
                }
            }
        }

        void SelectStickToGo()
        {
            int randomNum = Random.Range(0, 10);
            if (randomNum > collectPossibility)
            {
                // GO TO SAME COLOR
                GameObject selectedStick = sameColorList[Random.Range(0, sameColorList.Count - 1)];
                Vector3 pos = selectedStick.transform.position;
                agent.SetDestination(pos);
                agent.transform.LookAt(selectedStick.transform);
            }
            else
            {
                // GO TO WRONG COLOR
                GameObject selectedStick = reachableStickList[Random.Range(0, reachableStickList.Count)];
                Vector3 pos = selectedStick.transform.position;
                agent.SetDestination(pos);
                agent.transform.LookAt(selectedStick.transform);
            }
        }

        void SelectBridge()
        {
            int bridgeCount = GameObject.FindGameObjectsWithTag("Bridge").Length;
            selectedBridge = GameObject.FindGameObjectsWithTag("Bridge")[Random.Range(0, bridgeCount)];
        }

        void CheckToGoBuild()
        {
            if (stickBucket.childCount > 2 && Random.Range(0,2) == 0)
            {
                GoToBuild();
            }
        }

        void GoToBuild()
        {
            CancelInvoke();
            collecting = false;
            int stickCount = stickBucket.childCount;
            //Debug.Log(selectedBridge.transform.GetChild(selectedBridge.transform.childCount - stickCount).position);

            if (stickCount < selectedBridge.transform.childCount)
            {
                agent.SetDestination(selectedBridge.transform.GetChild(stickCount).position);
            }
            else
            {
                StartCoroutine(GoToFinish());
            }
        }

        IEnumerator GoToFinish()
        {
            agent.SetDestination(selectedBridge.transform.GetChild(selectedBridge.transform.childCount-1).position);
            yield return new WaitForSeconds(2);
            agent.SetDestination(GameObject.FindGameObjectWithTag("Finish").transform.position);
        }


        // COLLECTING ------------------------------
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Red Stick"))
            {
                FindObjectOfType<PlayerController>().Collect(other.gameObject,stickBucket);
                StartCoroutine(StickCreater.instance.CreateStickAfterCollect(other.transform.position, 1));
            }
        }
        
        // Building

        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.CompareTag("Ladder Stick") && stickBucket.childCount > 0)
            {
                FindObjectOfType<PlayerController>().BuildStairs(other.gameObject,"Builded Stick Red",stickBucket);
                
            }
        }
    }
}