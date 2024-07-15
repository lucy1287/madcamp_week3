using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{   
    public float speed = 10f;
    private float screenRightEdge;

    // Start is called before the first frame update
    void Start()
    {
        screenRightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
    }

    // Update is called once per frame
    void Update()
    {
        // 총알을 오른쪽으로 이동시킵니다.
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // 총알이 화면의 오른쪽 끝에 도달하면 삭제합니다.
        if (transform.position.x > screenRightEdge)
        {
            Destroy(gameObject);
        }
    }
}
