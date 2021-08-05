using System;
using System.Security.Cryptography;
using UnityEngine;
using DG.Tweening;

namespace DefaultNamespace
{
    public class PlayerController : MonoBehaviour
    {
        [Header("--CHARACTER MOVE--")]
        [SerializeField] private DynamicJoystick joystick;
        [SerializeField] private float speed;
        [SerializeField] private float turnSpeed;

        [Header("--STICK COLLECTING--")]
        [SerializeField] private Transform stickBucket;
        [SerializeField] private int stickCount;

        [Header("--LADDER SETTINGS--")] [SerializeField]
        private Transform raycastPos;
        [SerializeField]private float ladderJump = .21f;
        [SerializeField] private float maxDistance;
        
        

        private Animator animator;
        private static readonly int Number = Animator.StringToHash("Number");
        
        private void Awake()
        {
            animator = GetComponent<Animator>();
            
        }
        private void Update()
        {
            Move();
            UpStairs();
            stickCount = stickBucket.childCount;
        }
        void Move()
        {
            float horizontalInput = joystick.Horizontal;
            float verticalInput = joystick.Vertical;

            if (GameManager.instance.playerState == GameManager.PlayerState.Playing)
            {
                Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
                movementDirection.Normalize();

                transform.Translate(movementDirection * speed * Time.deltaTime, Space.World);

                if (movementDirection != Vector3.zero)
                {
                    Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, turnSpeed * Time.deltaTime);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0,transform.rotation.y,0);
                }
            }
            // ANIMATION CONTROL //
            if (horizontalInput == 0 && verticalInput == 0)
            {
                animator.SetInteger(Number, 0);
            }
            else
            {
                animator.SetInteger(Number, 1);
            }
        }
        void Collect(GameObject stick)
        {
            stick.transform.tag = "Collected Stick";
            Destroy(stick.gameObject.GetComponent<Rigidbody>());
            if (stickBucket.childCount == 0)
            {
                stick.transform.SetParent(stickBucket);

                stick.transform.DOMove(stickBucket.position, .25f);
            }
            else
            {
                stick.transform.SetParent(stickBucket);
                stick.transform.DOMove(stickBucket.position, .25f);
            }
        }

        void UpStairs()
        {
            RaycastHit hit;
            if (Physics.Raycast(raycastPos.position,raycastPos.forward,out hit,maxDistance))
            {
                if (hit.transform.CompareTag("Builded Stick"))
                {
                    transform.position += new Vector3(0, ladderJump, 0);
                }
                if (hit.transform.CompareTag("Ladder Stick") && stickCount > 0)
                {
                    transform.position += new Vector3(0, ladderJump, 0);
                }
            }
        }

        void BuildStairs(GameObject ladder)
        {
            GameObject stick = stickBucket.GetChild(stickCount-1).gameObject;
            stick.transform.SetParent(null);
            Sequence seq = DOTween.Sequence();
            seq.Append(stick.transform.DOMove(ladder.transform.position, .25f))
                .Insert(0, stick.transform.DOScale(ladder.transform.localScale, .25f))
                .Insert(0,
                    ladder.GetComponent<MeshRenderer>().material
                        .DOColor(stick.GetComponent<MeshRenderer>().material.color, .25f));
            
            Destroy(stick,1f);
            ladder.GetComponent<MeshRenderer>().enabled = true;

            ladder.transform.tag = "Builded Stick";

        }

        private void OnTriggerEnter(Collider other)
        {
            // COLLECT
            if (other.CompareTag("Blue Stick"))
            {
                Collect(other.gameObject);
                //Destroy(other.gameObject);
                StartCoroutine(StickCreater.instance.CreateStickAfterCollect(other.transform.position, 1));
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.CompareTag("Ladder Stick") && stickCount > 0)
            {
                BuildStairs(other.gameObject);
            }
        }
    }
}