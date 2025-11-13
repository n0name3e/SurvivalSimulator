using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeonEscape
{
    public class DexMovement : MonoBehaviour
    {
        private Transform cameraObject;
        public Vector3 moveDirection;

        public Rigidbody rb;
        private DexAnimatorHandler animatorHandler;
        private TimeLapse timeLapse;

        public float movementSpeed = 2f;
        public float rotationSpeed = 10f;

        private Vector3 normalVector;
        private Vector3 targetPosition;


        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            cameraObject = Camera.main.transform;
            animatorHandler = GetComponent<DexAnimatorHandler>();
            timeLapse = GetComponent<TimeLapse>();            
        }
        private void Update()
        {
            HandleMovement(Time.unscaledDeltaTime);            
        }
        // Update is called once per frame
        void FixedUpdate()
        {
            if (timeLapse.isRewinding)
            {
                return;
            }
        }

        public void HandleMovement(float delta)
        {
            float horizontal = 0;
            float vertical = 0;
            if (Time.timeScale <= 0)
            {
                horizontal = Input.GetAxisRaw("Horizontal");
                vertical = Input.GetAxisRaw("Vertical");
            }
            else
            {
                horizontal = Input.GetAxis("Horizontal");
                vertical = Input.GetAxis("Vertical");
            }
            moveDirection = cameraObject.forward * vertical;
            moveDirection += cameraObject.right * horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;
            // At timeScale 1.0 (normal), modifier is b
            // At timeScale 0.0 (stopped), modifier is a 
            float speedModifier = Mathf.Lerp(0.25f, 1f, Time.timeScale);
            float speed = movementSpeed * speedModifier;

            moveDirection *= speed;
            //Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            //rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);

            Vector3 movementDirection = moveDirection * delta;
            //movementDirection.y = rb.velocity.y * delta;
            if (Time.timeScale <= 0)
            {
                transform.position += movementDirection;
            }
            else
                rb.MovePosition(rb.position + movementDirection);


            animatorHandler.UpdateAnimatorValues(horizontal, vertical, speedModifier);

            HandleRotation(delta, horizontal, vertical);

        }
        private void HandleRotation(float delta, float horizontal, float vertical)
        {
            Vector3 targetDir;
            //float moveOverride = inputHandler.moveAmount;
            targetDir = cameraObject.forward * vertical;
            targetDir += cameraObject.right * horizontal;

            targetDir.Normalize();
            if (targetDir == Vector3.zero)
                targetDir = transform.forward;

            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * delta);
            if (Time.timeScale <= 0)
            {
                transform.rotation = tr;
            }
            else
                rb.MoveRotation(targetRotation);


            // makes player always look forward and not somewhere down
            Vector3 rot = transform.rotation.eulerAngles;
            rot.x = 0;
            transform.rotation = Quaternion.Euler(rot);
        }
    }
}
