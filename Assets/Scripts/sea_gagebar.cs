using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sea_gagebar : MonoBehaviour
{
    public GameObject gagebar;
    public float ball_speed; //구슬 왔다갔다 속도 
    public bool move_right;

    void Start()
    {
        ball_speed = 7.5f;
        move_right = false;
    }

    void Update()
    {
        if (gagebar.gameObject.activeSelf) //게이지바가 활성화 상태일 때만
        {
            if (move_right) //오른쪽으로 가기
            {
                transform.Translate(ball_speed, 0, 0);
            }
            else //왼쪽으로 가기
            {
                transform.Translate(-ball_speed, 0, 0);
            }
        }
    }

    //turn_ball에 부딪히면 방향 바꾸기
    public void OnTriggerEnter2D(Collider2D trig)
    {
        if (trig.gameObject.CompareTag("turn_ball"))
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
