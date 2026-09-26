using UnityEngine;

public class ParallaxBG : MonoBehaviour
{
    public Vector2 velocityMP;
    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
     
    private void Start(){
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
    }

    // Update is called once per frame
    private void LateUpdate(){
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * velocityMP.x, deltaMovement.y * velocityMP.y);
        lastCameraPosition = cameraTransform.position;
        
    }
}
