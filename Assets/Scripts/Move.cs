using System. Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {
    Animator animator;
    public bool LeftMove = false;
    public bool RightMove = false;
    Vector3 moveVelocity = Vector3.zero;
    float moveSpeed = 30; //버튼을 누르는 동안에 오브젝트의 움직이는 속도
    // Use this for initialization
    void Start () {
        animator = gameObject.GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update () {
        if (LeftMove)
        {
            animator.SetBool("Direction", false);
            moveVelocity = new Vector3(-0.10f, 0, 0) ;
            transform.position += moveVelocity * moveSpeed * Time.deltaTime;
        }
        if (RightMove)
        {
            animator.SetBool("Direction", true);
            moveVelocity = new Vector3(+0.10f, 0, 0);
            transform.position += moveVelocity * moveSpeed * Time.deltaTime;
        }
    }
}
