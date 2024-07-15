using UnityEngine;

public class ManualPlatform : MonoBehaviour
{
    public float speed = 2.0f; // 속도
    public float height = 0.5f; // 이동할 최대 높이

    private Vector3 originalPosition; // 초기 위치
    private bool isMoving = false; // 플랫폼이 움직이고 있는지 확인
    private bool goingUp = false; // 플랫폼이 올라가는지 내려가는지 확인

    void Start()
    {
        originalPosition = transform.position;
    }

    void Update()
    {
        if (isMoving)
        {
            MovePlatform();
        }
    }

    public void MovePlatform()
    {
        float newY = transform.position.y + (goingUp ? speed : -speed) * Time.deltaTime;

        if (!goingUp && newY <= originalPosition.y - height)
        {
            newY = originalPosition.y - height;
            isMoving = false;
        }
        else if (goingUp && newY >= originalPosition.y)
        {
            newY = originalPosition.y;
            isMoving = false;
        }
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    // 플랫폼을 움직이기 시작
    public void ToggleMovement()
    {
        if(!isMoving)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        goingUp = !goingUp; // 방향 전환
    }
}
