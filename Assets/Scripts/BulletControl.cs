using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class BulletControl : MonoBehaviour
{
    public int bullet_num = 1;
    public TMP_Text bulletNumText;

    // Start is called before the first frame update
    void Start()
    {
        // TMP_Text를 동적으로 찾기
        GameObject bulletNumTextObject = GameObject.FindGameObjectWithTag("BulletText");
        if (bulletNumTextObject != null)
        {
            bulletNumText = bulletNumTextObject.GetComponent<TMP_Text>();
        }
        else
        {
            Debug.LogError("bulletNumText object not found.");
            return;
        }

        if (bulletNumText == null)
        {
            Debug.LogError("TMP_Text component not found on bulletNumText object.");
            return;
        }
        // 보유한 총알 개수 표시
        bulletNumText.text = "Bullet: " + 0.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Bullet Trigger 호출됨");
        if(collision.CompareTag("Player"))
        {
            GameObject player = collision.gameObject;
            Destroy(gameObject);
            ItemGain(player);
        }   
    }

    protected virtual void ItemGain(GameObject player) 
    { 
        if (player == null)
        {
            Debug.LogError("Player object not set.");
            return;
        }
        
        PhotonView playerPhotonView = player.GetComponent<PhotonView>();
        Astronaut astronaut = player.GetComponent<Astronaut>();
        if (playerPhotonView != null && playerPhotonView.IsMine)
        {
            if (astronaut != null)
            {
                astronaut.bullet += bullet_num;
                bulletNumText.text = "Bullet: " + astronaut.bullet.ToString();
                Debug.Log("Bullet acquired! Total bullets: " + astronaut.bullet);
            }
            else
            {
                Debug.LogError("Astronaut component not found on player object.");
            }
        }
        else
        {
            Debug.Log("This player object is not owned by the local player.");
        }
    }
}
