using UnityEngine;

public class FirstPersonCameraController : MonoBehaviour
{
    public Transform playerTransform; 
    public float smoothSpeed = 10f; 

    private Vector3 offset; 

    private void Start()
    {
        offset = transform.position - playerTransform.position;
    }

    private void Update()
    {
        Vector3 targetPosition = playerTransform.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}
