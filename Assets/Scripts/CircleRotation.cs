using UnityEngine;

public class CircleRotation : MonoBehaviour
{
    public float rotationSpeed = 50f;   // 회전 속도 (각도 단위)

    void Update()
    {
        // 회전 속도에 따라 오브젝트를 회전시킵니다.
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}