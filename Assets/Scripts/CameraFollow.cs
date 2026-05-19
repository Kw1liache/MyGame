using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Цель")]
    public Transform player;

    [Header("Смещение")]
    public Vector3 offset = new Vector3(0, 0, -10);

    [Header("Плавность")]
    public float smoothSpeed = 5f;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void LateUpdate()
    {
        Vector3 targetPosition = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}