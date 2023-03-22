using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player1;
    public Vector2 minCameraRimit;
    public Vector2 maxCameraRimit;
    public float cameraSpeed = 0.2f;

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        //move rimit
        Vector2 targetPos = new(player1.position.x, player1.position.y);

        targetPos.x = Mathf.Clamp(targetPos.x, minCameraRimit.x, maxCameraRimit.x);
        targetPos.y = Mathf.Clamp(targetPos.y, minCameraRimit.y, maxCameraRimit.y);

        transform.position = Vector2.Lerp(transform.position, targetPos, cameraSpeed);
    }
}

