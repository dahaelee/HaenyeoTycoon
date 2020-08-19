using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sea_spots : MonoBehaviour
{
    public sea_item[] sea_item;
    public sea_item shell, seaweed, starfish, shrimp, jellyfish, crab, octopus, abalone, turtle;
    public float item_speed = 0f;
    public bool move_right, targeted;
    public Image block_touch;

    //기즈모 만들기 (게임에 실제로 보이지는 않지만 작업할 때 위치를 볼 수 있도록 함)
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 10f);
    }

    void Start()
    {
        sea_item = new sea_item[9];
        sea_item[0] = shell;
        sea_item[1] = seaweed;
        sea_item[2] = starfish;
        sea_item[3] = shrimp;
        sea_item[4] = jellyfish;
        sea_item[5] = crab;
        sea_item[6] = octopus;
        sea_item[7] = abalone;
        sea_item[8] = turtle;

        this.move_right = (Random.value > 0.5f); //true, false 중 랜덤 (왼쪽으로 먼저 갈지, 오른쪽으로 먼저 갈지)
        targeted = false;
    }
    
    void Update()
    {
        //움직이는 자원만 자원 종류에 따라 이동속도 다르게 초기화 
        int speed = Haenyeo.moving_speed; 

        if (this.gameObject.GetComponent<sea_item>().item_name == sea_item[3].name) //새우
        {
            item_speed = (float)speed / 7;
        }
        if (this.gameObject.GetComponent<sea_item>().item_name == sea_item[4].name) //해파리
        {
            item_speed = (float)speed / 6;
        }
        if (this.gameObject.GetComponent<sea_item>().item_name == sea_item[6].name) //문어
        {
            item_speed = (float)speed / 5;
        }
        if (this.gameObject.GetComponent<sea_item>().item_name == sea_item[8].name) //거북이
        {
            item_speed = (float)speed / 4;
        }
        if (targeted) //채집시도되면 멈춤
        {
            item_speed = 0f;
        }

        //안움직이는 자원은 좌우 반전만 랜덤으로, 움직이는 자원은 왔다갔다 이동까지  
        if (move_right) //오른쪽으로 가기
        {
            transform.localScale = new Vector2(-100, 100);
            if ((this.gameObject.GetComponent<sea_item>().moving) && (this.gameObject.activeSelf)) //움직이는 자원이고 생성되어 있을 때 
            {
                if (!block_touch.gameObject.activeSelf) //터치 방지가 비활성화 상태일 때만
                {
                    transform.Translate(item_speed, 0, 0);
                }
            }
        }
        else //왼쪽으로 가기
        {
            transform.localScale = new Vector2(100, 100);
            if ((this.gameObject.GetComponent<sea_item>().moving) && (this.gameObject.activeSelf)) //움직이는 자원이고 생성되어 있을 때 
            {
                if (!block_touch.gameObject.activeSelf) //터치 방지가 비활성화 상태일 때만
                {
                    transform.Translate(-item_speed, 0, 0);
                }
            }
        }
    }

    //turn에 충돌하면 방향 바꾸기
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("turn"))
        {
            if (move_right) 
            {
                item_speed = 0f;
                move_right = false;
            }
            else
            {
                move_right = true;
            }
        }
    }

    //해녀와 충돌하면 테두리 있는 애니메이션으로 변경하고 target 설정
    public void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("haenyeo"))
        {
            this.GetComponent<Animator>().SetBool("collided", true);
            sea_catch.target = this.gameObject;
        }
    }

    //해녀와의 충돌상태에서 나가면 테두리 없는 애니메이션으로 변경하고 target 초기화, targeted를 false로 
    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("haenyeo"))
        {
            this.GetComponent<Animator>().SetBool("collided", false);
            sea_catch.target = null;
            this.targeted = false;
        }
    }

}