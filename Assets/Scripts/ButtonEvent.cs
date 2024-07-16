using System. Collections;
using System.Collections.Generic;
using UnityEngine;
public class ButtonEvent : MonoBehaviour {
    GameObject Player;
    Move move;
    // Use this for initialization
    void Start() {
        
    }
    // Update is called once per frame
    void Update() {

    }
    // Player 객체를 설정하는 메서드
    public void SetPlayer(GameObject player)
    {
        if (player != null)
        {
            move = player.GetComponent<Move>();

            if (move == null)
            {
                Debug.LogError("Move component not found on the player object.");
            }
        }
        else
        {
            Debug.LogError("Player object not found.");
        }
    }

    public void LeftBtnDown() { move.LeftMove = true; }
    public void LeftBtnUp() { move.LeftMove = false; }
    public void RightBtnDown() { move.RightMove = true; }
    public void RightBtnUp() { move.RightMove = false; }

}
