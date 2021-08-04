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

        private Animator animator;
        private static readonly int Number = Animator.StringToHash("Number");
        
        private void Awake()
        {
            animator = GetComponent<Animator>();
            
        }
        private void Update()
        {
            Move();
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
            /////////////////////////////////////////////
        }
        void Collect(GameObject stick)
        {
            stick.transform.tag = "Collected Stick";
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
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Blue Stick"))
            {
                Collect(other.gameObject);
                //Destroy(other.gameObject);
                StartCoroutine(StickCreater.instance.CreateStickAfterCollect(other.transform.position, 1));
            }
        }
    }
}