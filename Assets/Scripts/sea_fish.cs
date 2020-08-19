using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sea_fish : MonoBehaviour
{
    public bool move_right;
    public float scale;
    public float speed;
    public Image block_touch;

    void Update()
    { 
        if (move_right) //오른쪽으로 가기
        {
            transform.localScale = new Vector3(scale, scale, 1);
            transform.Translate(speed, 0, 0);
        }
        else //왼쪽으로 가기
        {
            transform.localScale = new Vector3(-scale, scale, 1);
            transform.Translate(-speed, 0, 0);
        }
    }

    //turn_fish에 부딪히면 방향 바꾸기
    public void OnTriggerEnter2D(Collider2D trig)
    {
        if (trig.gameObject.CompareTag("turn_fish"))
        {
            if (move_right)
            {
                move_right = false;
            }
            else
            {
                move_right = true;
            }
        }
    }
}

