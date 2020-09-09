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
    public IEnumerator item_generating, item1coroutine, item2coroutine, item3coroutine;   //양식 하고있는 함수 코루틴


    void Start()
    {//양식장 초기화 코드

        data_load();

    }

        void OnDisable()
        {
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
                PlayerPrefs.SetInt("farm" + farm_number + "_sea_item", item.number);
        }
            else
            {
                PlayerPrefs.SetInt("farm" + farm_number + "_isFarming", 0);
            }
            PlayerPrefs.SetInt("farm" + farm_number + "_opportunity", farm_opportunity);
            PlayerPrefs.SetInt("farm" + farm_number + "_remaining_time", remaining_time);
        
    }
    void Update()
    {
        if (is_money_on)
        {
        }
    }

    //생성까지 시간 차감하며 기다리기
    public IEnumerator Wait_generating()
    {
        this.isFarming = true;
        this.farm_opportunity--;
        this.remaining_time = this.item.farm_time;
        if (item1coroutine == null)
        {
            item1coroutine = itemAnim(this, this.item1);
            StartCoroutine(item1coroutine);
        }
        this.item1.gameObject.SetActive(true);
        this.item2.gameObject.SetActive(false);
        this.item3.gameObject.SetActive(false);
        if (this.item.name == "turtle")
        {
            this.item1.rectTransform.localScale = new Vector3((float)1.2, (float)0.8, (float)0.8);
            this.item2.rectTransform.localScale = new Vector3((float)1.2, 1, 1);
            this.item3.rectTransform.localScale = new Vector3((float)1.2, 1, 1);
        }
        else
        {
            this.item1.rectTransform.localScale = new Vector3(1, 1, 1);
            this.item2.rectTransform.localScale = new Vector3(1, 1, 1);
            this.item3.rectTransform.localScale = new Vector3(1, 1, 1);
        }

        bool item2_effect_On = false;
        bool item3_effect_On = false;
        while (this.remaining_time > 0)
        {
            if ((this.remaining_time < (this.item.farm_time / 3) * 2) && (this.remaining_time >= this.item.farm_time / 3))
            {
                this.item2.gameObject.SetActive(true);
                if (item2coroutine == null)
                {
                    item2coroutine = itemAnim(this, this.item2);
                    StartCoroutine(item2coroutine);
                }
                if (!item2_effect_On)
                {
                    item2_effect_On = true;
                }
            }
            if (this.remaining_time < this.item.farm_time / 3)
            {
                this.item3.gameObject.SetActive(true);
                if (item3coroutine == null)
                {
                    item3coroutine = itemAnim(this, this.item3);
                    StartCoroutine(item3coroutine);
                }
                if (!item3_effect_On)
                {
                    item3_effect_On = true;
                }
            }
            yield return new WaitForSeconds(1);

            this.remaining_time--;
        }
        this.isFarming = false;
        this.money.gameObject.SetActive(true);
        this.is_money_on = true;
        //farm.bubble_item.gameObject.SetActive(true);
    }


    //게임 다시 시작할 때 셋팅용으로 쓰는 함수
    IEnumerator Wait_generating_start()
    {
        if (this.is_money_on)
        {
            this.money.gameObject.SetActive(true);
        }
        else if (this.isFarming)
        {
            this.item1.gameObject.SetActive(true);
            if (item1coroutine == null)
            {
                item1coroutine = itemAnim(this, this.item1);
                StartCoroutine(item1coroutine);
            }
            this.item2.gameObject.SetActive(false);
            this.item3.gameObject.SetActive(false);
            while (this.remaining_time > 0)
            {
                if (this.remaining_time < (this.item.farm_time / 3) * 2)
                {
                    if (item2coroutine == null)
                    {
                        item2coroutine = itemAnim(this, this.item2);
                        StartCoroutine(item2coroutine);
                    }
                    this.item2.gameObject.SetActive(true);

                }
                if (this.remaining_time < this.item.farm_time / 3)
                {
                    if (item3coroutine == null)
                    {
                        item3coroutine = itemAnim(this, this.item3);
                        StartCoroutine(item3coroutine);
                    }
                    this.item3.gameObject.SetActive(true);
                }
                yield return new WaitForSeconds(1);

                this.remaining_time--;
            }
            this.isFarming = false;
            this.money.gameObject.SetActive(true);
            this.is_money_on = true;
            //farm.bubble_item.gameObject.SetActive(true);
        }

    }

    public IEnumerator itemAnim(farm farm, Image img)
    {

        while (farm.item_generating != null)
        {
            UnityEngine.Debug.Log("coroutine");
            img.sprite = Resources.Load<Sprite>("items_anim/"+this.item.name+"1");
            yield return new WaitForSeconds(0.5f);
            img.sprite = Resources.Load<Sprite>("items_anim/" + this.item.name + "2");
            yield return new WaitForSeconds(0.5f);
            img.sprite = Resources.Load<Sprite>("items_anim/" + this.item.name + "1");
            yield return new WaitForSeconds(0.5f);
            img.sprite = Resources.Load<Sprite>("items_anim/" + this.item.name + "2");
            yield return new WaitForSeconds(4f);
        }
    }


    // 양식장 비우는 코드
    public void farmReset()
    {
        this.item = null;
        this.isFarming = false;         //양식중 아님으로 바꾸기
        this.farm_opportunity = 5;          //양식 횟수 초기화
        this.item1.gameObject.SetActive(false);     //자원들 다 안보이게 하기
        this.item2.gameObject.SetActive(false);
        this.item3.gameObject.SetActive(false);
        this.money.gameObject.SetActive(false);
        //this.bubble_item.gameObject.SetActive(false);
        this.plus.gameObject.SetActive(true);       //양식하기 플러스 아이콘 보이게하기
        if (item_generating != null)
        {
            StopCoroutine(item_generating);
            StopCoroutine(item1coroutine);
            StopCoroutine(item2coroutine);
            StopCoroutine(item3coroutine);
            item1coroutine = null;
            item2coroutine = null;
            item3coroutine = null;
            item_generating = null;
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
        //this.bubble_item.gameObject.SetActive(false);

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
        this.item = farm_manager.sea_item[PlayerPrefs.GetInt("farm" + farm_number + "_sea_item", 0)].GetComponent<sea_item>();  //무슨 자원 담고있었는지 가져옴
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
                this.money.sprite = Resources.Load<Sprite>(item.name); //양식 다된 버블안 이미지바뀜
                if (time_interv.TotalSeconds > this.remaining_time)   //양식시간이 이미 지났는가?
                {
                    this.remaining_time = 0;
                    this.item1.gameObject.SetActive(true);
                    this.item2.gameObject.SetActive(true);
                    this.item3.gameObject.SetActive(true);
                    this.money.gameObject.SetActive(true);
                    //this.bubble_item.gameObject.SetActive(true);
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
                    this.item_generating = this.Wait_generating_start();
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








