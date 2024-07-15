using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BulletControl : MonoBehaviour
{
    private GameObject player;
    public int bullet_num = 1;
    public TMP_Text bulletNumText;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
            Destroy(gameObject);
            ItemGain();
        }   
    }

    protected virtual void ItemGain() 
    { 
        Astronaut astronaut = player.GetComponent<Astronaut>();
        if(astronaut != null)
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
}
