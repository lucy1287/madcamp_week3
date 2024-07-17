using UnityEngine;

public class CircleMovement : MonoBehaviour
{
    public float orbitRadius = 5f;      // 원의 궤도 반지름
    public float rotationSpeed = 50f;   // 플레이어의 회전 속도
    public float orbitSpeed = 2f;       // 원을 그리는 속도 (회전 속도)

    private float orbitAngle;           // 원의 궤도를 따라 이동하는 각도 (라디안 단위)
    private float playerAngle;          // 플레이어의 회전 각도 (라디안 단위)
    private Vector3 orbitCenter;        // 원의 중심 위치

    void Start()
    {
        // 원의 중심 위치를 현재 위치로 설정
        orbitCenter = transform.position;

        // 초기 각도를 설정
        orbitAngle = 0f;
        playerAngle = 0f;
    }

    void Update()
    {
        // 원의 궤도를 따라 이동할 각도를 증가시킵니다.
        orbitAngle += orbitSpeed * Time.deltaTime;

        // 각도를 라디안 단위로 변환
        float orbitRadian = orbitAngle * Mathf.Deg2Rad;

        // 새로운 위치를 계산
        float x = orbitCenter.x + Mathf.Cos(orbitRadian) * orbitRadius;
        float z = orbitCenter.z + Mathf.Sin(orbitRadian) * orbitRadius;

        // 플레이어의 위치를 업데이트합니다.
        transform.position = new Vector3(x, transform.position.y, z);

        // 플레이어의 회전 속도에 따라 각도를 증가시킵니다.
        playerAngle += rotationSpeed * Time.deltaTime;

        // 플레이어를 주위의 중심을 기준으로 회전시킵니다.
        transform.rotation = Quaternion.Euler(0f, playerAngle, 0f);
    }
}