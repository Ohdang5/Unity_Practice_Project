using UnityEngine;

public class StaticCamera : MonoBehaviour
{
    public Transform player;
    public Vector2 maxCameraRimit;
    public Vector2 minCameraRimit;
    public float cameraSpeed = 0.5f;
    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector2 targetPos = new(player.position.x, transform.position.y);

        targetPos.x = Mathf.Clamp(targetPos.x, minCameraRimit.x, maxCameraRimit.x);

        transform.position = Vector2.Lerp(transform.position, targetPos, cameraSpeed);

        if (player.position.y - transform.position.y > 6)
            transform.Translate(0, 12, 0);
        if (player.position.y - transform.position.y < -6)
            transform.Translate(0, -12, 0);
    }
}