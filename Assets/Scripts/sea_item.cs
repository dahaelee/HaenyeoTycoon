using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sea_item : MonoBehaviour
{
    public int raw_price, farm_price, farm_time, number;
    public string item_name;
    public string item_name_kor;
    public Image chosen_mark;
    public float sea1_prob, sea2_prob, sea3_prob; //각 바다에서의 생성확률
    public bool moving; //바다에서 움직이는지 여부
    public int difficulty; //채집 시 난이도
    public Text info_name, info_price, info_time;
    public Animator anim;
    public sea_item(string name, int raw_price, int farm_price, int farm_time, float sea1_prob, float sea2_prob, float sea3_prob, bool moving, int difficulty, Animator anim)
    {
        this.name = name;
        this.raw_price = raw_price;
        this.farm_price = farm_price;
        this.farm_time = farm_time;
        this.sea1_prob = sea1_prob;
        this.sea2_prob = sea2_prob;
        this.sea3_prob = sea3_prob;
        this.difficulty = difficulty;
        this.anim = anim;
    }

    void Awake()
    {
        info_name.text = item_name_kor;
        info_price.text = string.Format("{0:#,###0}", farm_price);
        /*        if ((farm_time / 60) > 0)
                {
                    if (farm_time % 60 == 0)
                    {
                        info_time.text = (farm_time / 60).ToString() + "분 ";
                    }
                    else
                    {
                        info_time.text = (farm_time / 60).ToString() + "분 " + (farm_time % 60).ToString() + "초";
                    }

                }
                else
                {
                    info_time.text = (farm_time % 60).ToString() + "초";
                }
                */
        info_time.text = (farm_time).ToString() + "초";
    }
}