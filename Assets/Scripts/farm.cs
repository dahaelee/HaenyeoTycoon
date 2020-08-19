using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class farm : MonoBehaviour
{
    public int farm_number; //양식장 번호
    public sea_item item;   //자원 종류
    public int farm_opportunity = 1;
    public int remaining_time;    //남은 양식 시간
    public bool is_farm_Activated; //양식장 활성화 여부
    public bool isFarming;  //자원 양식중인지 여부
    public bool is_money_on; //생성 완료가 되고나서 수확할 단계인지 확인
    public Image plus, farming_effect;
    public Image item1, item2, item3, locked_bg, lock_img, money;
    public Text farm_price, item_count;
    public IEnumerator item_generating;   //양식 하고있는 함수 코루틴


    void Start()
    {//양식장 초기화 코드

        data_load();

    }

        void OnDisable()
        {
            PlayerPrefs.SetInt("farm" + farm_number + "_sea_item", item.number);
            if (is_farm_Activated)
            {
                PlayerPrefs.SetInt("farm" + farm_number + "_is_farm_activated", 1);     //true면 1저장
            }
            else
            {
                PlayerPrefs.SetInt("farm" + farm_number + "_is_farm_activated", 0);
            }

            if (isFarming)
            {
                PlayerPrefs.SetInt("farm" + farm_number + "_isFarming", 1);     //true면 1저장
            }
            else
            {
                PlayerPrefs.SetInt("farm" + farm_number + "_isFarming", 0);
            }
            PlayerPrefs.SetInt("farm" + farm_number + "_opportunity", farm_opportunity);
            PlayerPrefs.SetInt("farm" + farm_number + "_remaining_time", remaining_time);
        
    }


    //게임 다시 시작할 때 셋팅용으로 쓰는 함수
    IEnumerator Wait_generating_start(farm farm)
    {
        if (farm.is_money_on)
        {
            farm.money.gameObject.SetActive(true);
        }
        else if (farm.isFarming)
        {
            farm.item1.gameObject.SetActive(true);
            farm.item2.gameObject.SetActive(false);
            farm.item3.gameObject.SetActive(false);
            while (farm.remaining_time > 0)
            {
                if (farm.remaining_time < (farm.item.farm_time / 3) * 2)
                {

                    farm.item2.gameObject.SetActive(true);

                }
                if (farm.remaining_time < farm.item.farm_time / 3)
                {

                    farm.item3.gameObject.SetActive(true);
                }
                yield return new WaitForSeconds(1);
                Debug.Log(farm.remaining_time);

                farm.remaining_time--;
            }
            farm.money.gameObject.SetActive(true);
        }

    }

    public void data_load()
    {
        this.farming_effect.gameObject.SetActive(false);
        this.farm_price.gameObject.SetActive(false);
        this.plus.gameObject.SetActive(false);
        this.item1.gameObject.SetActive(false);
        this.item2.gameObject.SetActive(false);
        this.item3.gameObject.SetActive(false);
        this.locked_bg.gameObject.SetActive(false);
        this.money.gameObject.SetActive(false);

        TimeSpan time_interv;       //시간 차이를 담는 변수
        if (PlayerPrefs.GetInt("farm" + farm_number + "_is_farm_activated", 0) != 0)    //양식장 활성화 상태였는지 확인
        {
            this.is_farm_Activated = true;  //0이면 false, 아니면 true
        }
        else
        {
            this.is_farm_Activated = false;
        }
        if((this.farm_number==2)|| (this.farm_number == 3) || (this.farm_number == 4) || (this.farm_number == 5))
        {
            this.is_farm_Activated = true;
        }


        if (PlayerPrefs.GetInt("farm" + farm_number + "_isFarming", 0) != 0)    //양식 중이었는지 확인
        {
            this.isFarming = true;
        }
        else
        {
            this.isFarming = false;
        }
        this.farm_opportunity = PlayerPrefs.GetInt("farm" + farm_number + "_opportunity", 1);   //양식 횟수 받아오기 없으면 초기화 1
        this.remaining_time = PlayerPrefs.GetInt("farm" + farm_number + "_remaining_time", 0);  //남은 시간 받아오기 없으면 초기화 0
        this.item = farm_manager.sea_item[PlayerPrefs.GetInt("farm" + farm_number + "_sea_item", 0)];  //무슨 자원 담고있었는지 가져옴
        string LastTime = PlayerPrefs.GetString("lasttime", System.DateTime.Now.ToString());    //마지막 접속시간 확인
        DateTime OldTime = System.DateTime.Parse(LastTime);
        DateTime NewTime = System.DateTime.Now;

        time_interv = (NewTime - OldTime);  //지금이랑 마지막 접속시간 시간차이 계산



        if (this.is_farm_Activated)      //활성화 된 양식장인가?
        {
            if (this.isFarming)          //양식중인 양식장인가?
            {
                if(item.name == "turtle")
                {
                    item1.rectTransform.localScale = new Vector3((float)1.2,(float)0.8,(float)0.8);
                    item2.rectTransform.localScale = new Vector3((float)1.2, 1, 1);
                    item3.rectTransform.localScale = new Vector3((float)1.2, 1, 1);
                }
                else
                {
                    item1.rectTransform.localScale = new Vector3(1, 1, 1);
                    item2.rectTransform.localScale = new Vector3(1, 1, 1);
                    item3.rectTransform.localScale = new Vector3(1, 1, 1);
                }
                this.item1.sprite = Resources.Load<Sprite>(item.name);   //자원 이미지 바꾸기
                this.item2.sprite = Resources.Load<Sprite>(item.name);   //자원 이미지 바꾸기
                this.item3.sprite = Resources.Load<Sprite>(item.name);   //자원 이미지 바꾸기
                if (time_interv.TotalSeconds > this.remaining_time)   //양식시간이 이미 지났는가?
                {
                    this.remaining_time = 0;
                    this.item1.gameObject.SetActive(true);
                    this.item2.gameObject.SetActive(true);
                    this.item3.gameObject.SetActive(true);
                    this.money.gameObject.SetActive(true);
                    this.is_money_on = true;
                    this.farm_opportunity--;
                }
                else
                {                                          //양식 시간이 남은 경우 남은 시간만큼 다시 함수 시작
                    this.remaining_time = this.remaining_time - (int)time_interv.TotalSeconds;
                    if (this.remaining_time < 0)
                    {
                        this.remaining_time = 0;
                    }
                    item1.gameObject.SetActive(true);
                    this.item_generating = Wait_generating_start(this);
                    StartCoroutine(this.item_generating);
                }
            }
            else
            {                                           //양식중이 아니면 자원 선택 가능하게 해두기
                this.plus.gameObject.SetActive(true);
            }

        }
        else
        {

            this.locked_bg.gameObject.SetActive(true);     //활성화되어있지 않으면 잠금상태
        }
    }


}








