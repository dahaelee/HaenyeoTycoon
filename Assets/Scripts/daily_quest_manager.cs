using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class daily_quest_manager : MonoBehaviour
{
    void Update()
    {
        int questReady = PlayerPrefs.GetInt("questReady", 1);
        if (questReady == 1) give_daily_quest(); //일일 퀘스트 제공

        todo_update();  //todo문 업데이트
        for (int idx = 0; idx < quest_Data.daily_quest_list.Count; idx++)
        {
            if (quest_Data.daily_quest_list[idx].state != -1) daily_quest_check(idx);
        }

        GameObject.Find("quest_manager").GetComponent<quest_manager>().done_check();
    }

    //일일 퀘스트 제공
    public void give_daily_quest()
    {
        PlayerPrefs.SetInt("questReady", 0);
        if (Haenyeo.day>=2 && Haenyeo.day <= quest_Data.daily_quest_list.Count+1)
        {
            quest_Data.daily_quest_list[Haenyeo.day - 2].state = 0;
            GameObject.Find("quest_manager").GetComponent<quest_manager>().show_quest_box(quest_Data.daily_quest_list[Haenyeo.day - 2]);
        }
        GameObject.Find("quest_manager").GetComponent<quest_manager>().quest_contents_update();
    }


    public void todo_update()
    {
        quest_Data.daily_quest_list[0].todo = $"조개  {Haenyeo.sea_item_number[0]}/3";
        quest_Data.daily_quest_list[1].todo = $"양식 미역  {Haenyeo.farm_item_number[1]}/3";
        quest_Data.daily_quest_list[2].todo = $"새우 {Haenyeo.sea_item_number[3]}/4 ";
        quest_Data.daily_quest_list[3].todo = $"금화 {PlayerPrefs.GetInt("quest3", 0)}/3";
        quest_Data.daily_quest_list[4].todo = $"해파리 {Haenyeo.sea_item_number[4]}/3";
        quest_Data.daily_quest_list[5].todo = $"지지 않기 {PlayerPrefs.GetInt("quest5", 0)}/3";
        quest_Data.daily_quest_list[6].todo = $"꽃게 {Haenyeo.sea_item_number[5]}/2";
        quest_Data.daily_quest_list[7].todo = $"새우 {Haenyeo.sea_item_number[3]}/1  꽃게  {Haenyeo.sea_item_number[5]}/1  문어  {Haenyeo.sea_item_number[6]}/1";
        quest_Data.daily_quest_list[8].todo = $"기능성 오리발 {equipment_upgrade.my_flipper}/1";
        quest_Data.daily_quest_list[9].todo = $"양식 문어 {Haenyeo.farm_item_number[6]}/3";
        quest_Data.daily_quest_list[10].todo = $"양식장 개수 {GameObject.Find("farm_manager").GetComponent<farm_manager>().Activated_farm_number()}/6";
        quest_Data.daily_quest_list[11].todo = $"연속 승리 {PlayerPrefs.GetInt("quest11", 0)}/3";
        quest_Data.daily_quest_list[12].todo = $"하루 50만원 송금하기";
        quest_Data.daily_quest_list[13].todo = $"전복 {Haenyeo.sea_item_number[7]}/3";
        quest_Data.daily_quest_list[14].todo = $"양식 꽃게 {Haenyeo.farm_item_number[5]}/5";
        quest_Data.daily_quest_list[15].todo = $"조금 비싼 물안경 {equipment_upgrade.my_goggle}/1";
        quest_Data.daily_quest_list[16].todo = $"조개 {Haenyeo.sea_item_number[0]}/5";

    }

    public void daily_quest_check(int idx)
    {
        switch (idx)
        {
            case 0:
                if (Haenyeo.sea_item_number[0] >= 3) quest_Data.daily_quest_list[idx].state = 2;
                else if (quest_Data.daily_quest_list[idx].state != 0) quest_Data.daily_quest_list[idx].state = 1;
                break;
            case 1:
                if (Haenyeo.farm_item_number[1] >= 3) quest_Data.daily_quest_list[idx].state = 2;
                else if (quest_Data.daily_quest_list[idx].state != 0) quest_Data.daily_quest_list[idx].state = 1;
                break;
            case 2:
                if (Haenyeo.sea_item_number[3] >= 4) quest_Data.daily_quest_list[idx].state = 2;
                else if (quest_Data.daily_quest_list[idx].state != 0) quest_Data.daily_quest_list[idx].state = 1;
                break;
            case 3:
                if (PlayerPrefs.GetInt("quest3", 0) >= 3) quest_Data.daily_quest_list[idx].state = 2;
                else if (quest_Data.daily_quest_list[idx].state != 0) quest_Data.daily_quest_list[idx].state = 1;
                break;
            case 4:
                if (Haenyeo.sea_item_number[4] >= 3) quest_Data.daily_quest_list[idx].state = 2;
                else if (quest_Data.daily_quest_list[idx].state != 0) quest_Data.daily_quest_list[idx].state = 1;
                break;
            case 5:
                if (PlayerPrefs.GetInt("quest5", 0) >= 3) quest_Data.daily_quest_list[idx].state = 2;
                else if (quest_Data.daily_quest_list[idx].state != 0) quest_Data.daily_quest_list[idx].state = 1;
                break;
            case 6:
                if (Haenyeo.sea_item_number[5] >= 2) quest_Data.daily_quest_list[idx].state = 2;
                else if (quest_Data.daily_quest_list[idx].state != 0) quest_Data.daily_quest_list[idx].state = 1;
                break;
            case 7:
                if (Haenyeo.sea_item_number[3] >= 1 && Haenyeo.sea_item_number[5] >= 1 && Haenyeo.sea_item_number[6] >= 1) quest_Data.daily_quest_list[idx].state = 2;
                else if (quest_Data.daily_quest_list[idx].state != 0) quest_Data.daily_quest_list[idx].state = 1;
                break;
            case 8:
                if (equipment_upgrade.my_flipper >= 1) quest_Data.daily_quest_list[idx].state = 2;
                break;
            case 9:
                if (Haenyeo.farm_item_number[6] >= 3) quest_Data.daily_quest_list[idx].state = 2;
                else if (quest_Data.daily_quest_list[idx].state != 0) quest_Data.daily_quest_list[idx].state = 1;
                break;
            case 10: //양식장 오픈 개수
                if (GameObject.Find("farm_manager").GetComponent<farm_manager>().Activated_farm_number() >= 6) quest_Data.daily_quest_list[idx].state = 2;
                break;
            case 11:
                if (PlayerPrefs.GetInt("quest11", 0) >= 3) quest_Data.daily_quest_list[idx].state = 2;
                else if (quest_Data.daily_quest_list[idx].state != 0) quest_Data.daily_quest_list[idx].state = 1;
                break;
            case 12:
                if (PlayerPrefs.GetInt("quest12", 0) == 1) quest_Data.daily_quest_list[idx].state = 2;
                else if (quest_Data.daily_quest_list[idx].state != 0) quest_Data.daily_quest_list[idx].state = 1;
                break;
            case 13:
                if (Haenyeo.sea_item_number[7] >= 4) quest_Data.daily_quest_list[idx].state = 2;
                else if (quest_Data.daily_quest_list[idx].state != 0) quest_Data.daily_quest_list[idx].state = 1;
                break;
            case 14:
                if (Haenyeo.farm_item_number[5] >= 5) quest_Data.daily_quest_list[idx].state = 2;
                else if (quest_Data.daily_quest_list[idx].state != 0) quest_Data.daily_quest_list[idx].state = 1;
                break;
            case 15:
                if (equipment_upgrade.my_goggle >= 1) quest_Data.daily_quest_list[idx].state = 2;
                break;
            case 16:
                if (Haenyeo.sea_item_number[0] >= 5) quest_Data.daily_quest_list[idx].state = 2;
                else if (quest_Data.daily_quest_list[idx].state != 0) quest_Data.daily_quest_list[idx].state = 1;
                break;
        }
    }

    public void quest_reward(int idx)
    {
        switch (idx)
        {
            case 0:
                Haenyeo.sea_item_number[0] -= 3;
                Haenyeo.money += 20000;
                StartCoroutine(GameObject.Find("quest_manager").GetComponent<quest_manager>().reward_effect("money",20000));

                break;
            case 1:
                Haenyeo.farm_item_number[1] -= 3;
                Haenyeo.money += 30000;
                StartCoroutine(GameObject.Find("quest_manager").GetComponent<quest_manager>().reward_effect("money", 30000));
                break;
            case 2:
                Haenyeo.sea_item_number[3] -= 4;
                Haenyeo.money += 50000;
                StartCoroutine(GameObject.Find("quest_manager").GetComponent<quest_manager>().reward_effect("money", 50000));

                break;
            case 3:
                PlayerPrefs.DeleteKey("quest3");
                Haenyeo.money += 50000;
                StartCoroutine(GameObject.Find("quest_manager").GetComponent<quest_manager>().reward_effect("money", 50000));
                break;
            case 4:
                Haenyeo.sea_item_number[4] -= 3;
                Haenyeo.money += 30000;
                StartCoroutine(GameObject.Find("quest_manager").GetComponent<quest_manager>().reward_effect("money", 30000));
                break;
            case 5:
                PlayerPrefs.DeleteKey("quest5");
                StartCoroutine(GameObject.Find("quest_manager").GetComponent<quest_manager>().reward_effect("net", 0));
                //싹쓸이 그물 3개 추가
                break;
            case 6:
                Haenyeo.sea_item_number[5] -= 2;
                StartCoroutine(GameObject.Find("quest_manager").GetComponent<quest_manager>().reward_effect("bonus", 0));
                //체력 추가 2개 추가
                break;
            case 7:
                Haenyeo.sea_item_number[3] -= 1;
                Haenyeo.sea_item_number[5] -= 1;
                Haenyeo.sea_item_number[6] -= 1;
                Haenyeo.money += 40000;
                StartCoroutine(GameObject.Find("quest_manager").GetComponent<quest_manager>().reward_effect("money", 40000));
                break;
            case 8:
                StartCoroutine(GameObject.Find("quest_manager").GetComponent<quest_manager>().reward_effect("net", 0));
                //싹쓸이그물 5개 추가
                break;
            case 9:
                Haenyeo.farm_item_number[6] -= 3;
                Haenyeo.money += 40000;
                StartCoroutine(GameObject.Find("quest_manager").GetComponent<quest_manager>().reward_effect("money", 40000));
                break;
            case 10:
                StartCoroutine(GameObject.Find("quest_manager").GetComponent<quest_manager>().reward_effect("ball2", 0));
                //중간 구슬 3개 추가
                break;
            case 11:
                Haenyeo.money += 100000;
                StartCoroutine(GameObject.Find("quest_manager").GetComponent<quest_manager>().reward_effect("money", 100000));
                break;
            case 12:
                StartCoroutine(GameObject.Find("quest_manager").GetComponent<quest_manager>().reward_effect("bonus", 0));
                //보너스 3개 추가
                break;
            case 13:
                Haenyeo.sea_item_number[7] -= 4;
                StartCoroutine(GameObject.Find("quest_manager").GetComponent<quest_manager>().reward_effect("ball3", 0));
                //깊은 구슬 3개 추가
                break;
            case 14:
                Haenyeo.farm_item_number[5] -= 5;
                Haenyeo.money += 120000;
                StartCoroutine(GameObject.Find("quest_manager").GetComponent<quest_manager>().reward_effect("money", 120000));
                break;
            case 15:
                StartCoroutine(GameObject.Find("quest_manager").GetComponent<quest_manager>().reward_effect("net", 0));
                //싹쓸이그물 3개 추가
                break;
            case 16:
                Haenyeo.sea_item_number[0] -= 5;
                StartCoroutine(GameObject.Find("quest_manager").GetComponent<quest_manager>().reward_effect("ball1", 0));
                //그물 2개씩 추가
                break;
        }
    }
}
