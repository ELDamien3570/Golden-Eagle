using UnityEngine;
using UnityEngine.Rendering;

namespace GE
{
    public class CameraManager : MonoBehaviour
    {
        public Transform targetTransform; //Object camera follows
        public Transform cameraPivot; //Object the camera pivots around
        public Transform cameraTransform; //Actual positon of the camera
        public LayerMask collisionLayers; //Layers we want camera to collide with

        InputManager inputManager;

        private float defaultPosition;
        private Vector3 cameraFollowVelocity = Vector3.zero;
        private Vector3 cameraVectorPosition;

        public float cameraFollowSpeed = 0.2f;
        public float minimumCollisionOffset = 0.2f;
        public float cameraLookSpeed = 2;
        public float cameraPivotSpeed = 2;
        public float cameraCollisionRadius = 0.2f;
        public float cameraCollisionOffset = 0.2f;

        public float lookAngle; //Camera looking up and down
        public float pivotAngle; //Looking left and right
        public float minimumPivotAngle = -35;
        public float maximumPivotAngle = 35;

        //[Header("Camera Controls")]
        //public KeyCode freeLookKey = KeyCode.LeftAlt;


        private void Awake()
        {
            inputManager = FindAnyObjectByType<InputManager>();
            targetTransform = FindAnyObjectByType<PlayerManager>().transform; //Make the object holding the PlayerManager script the target, can be serialized to allow a custom target
            cameraTransform = Camera.main.transform;
            defaultPosition = cameraTransform.localPosition.z;
        }

        public void HandleAllCameraMovement()
        {
            FollowTarget();
            RotateCamera();
            HandleCameraCollisions();
        }

        private void FollowTarget()
        {
            Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position, ref cameraFollowVelocity, cameraFollowSpeed);
            transform.position = targetPosition;

        }

        private void RotateCamera()
        {
            Vector3 rotation;
            lookAngle = lookAngle + (inputManager.cameraInputX * cameraLookSpeed);
            pivotAngle = pivotAngle - (inputManager.cameraInputY * cameraPivotSpeed);
            //pivotAngle = pivotAngle + cameraPivotSpeed;
            pivotAngle = Mathf.Clamp(pivotAngle, minimumPivotAngle, maximumPivotAngle);


            rotation = Vector3.zero;
            rotation.y = lookAngle;
            Quaternion targetRotation = Quaternion.Euler(rotation);
            transform.rotation = targetRotation;


            rotation = Vector3.zero;

            rotation.x = pivotAngle;
            targetRotation = Quaternion.Euler(rotation);
            cameraPivot.localRotation = targetRotation;
        }

        private void HandleCameraCollisions()
        {
            float targetPosition = defaultPosition;
            RaycastHit hit;
            Vector3 direction = cameraTransform.position - cameraPivot.position;
            direction.Normalize();

            if (Physics.SphereCast(cameraPivot.transform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetPosition), collisionLayers))
            {
                float distance = Vector3.Distance(cameraPivot.position, hit.point);
                targetPosition = -targetPosition - (distance - cameraCollisionOffset);
            }

            if (Mathf.Abs(targetPosition) < minimumCollisionOffset)
            {
                targetPosition = targetPosition - minimumCollisionOffset;
            }

            cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, 0.2f);
            cameraTransform.localPosition = cameraVectorPosition;
        }
    }
}