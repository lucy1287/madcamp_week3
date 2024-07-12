using System. Collections;
using System.Collections.Generic;
using UnityEngine;
public class ButtonEvent : MonoBehaviour {
    GameObject Player;
    Move move;
    // Use this for initialization
    void Start() {
        Player = GameObject.Find("GameObject");
        move = Player.GetComponent<Move>();
    }
    // Update is called once per frame
    void Update() {

    }
    public void LeftBtnDown() { move.LeftMove = true; }
    public void LeftBtnUp() { move.LeftMove = false; }
    public void RightBtnDown() { move.RightMove = true; }
    public void RightBtnUp() { move.RightMove = false; }

}
