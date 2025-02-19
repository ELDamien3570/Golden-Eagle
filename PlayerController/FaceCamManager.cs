using UnityEngine;

namespace GE
{
    public class FaceCamManager : MonoBehaviour
    {
        public Transform targetTransform; //Object camera follows
        public float cameraFollowSpeed = 0.2f;
        private Vector3 cameraFollowVelocity = Vector3.zero;

        private void Awake()
        {
            HandleAllCameraMovement();  
        }
        public void HandleAllCameraMovement()
        {
            FollowTarget();
           
        }
        private void FollowTarget()
        {
            Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position, ref cameraFollowVelocity, cameraFollowSpeed);
            transform.position = targetPosition;

        }
    }
}
