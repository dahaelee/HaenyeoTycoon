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
        quest_Data.daily_quest_list[8].todo = $"나의 오리발 단계 : {equipment_upgrade.my_flipper+1}";
        quest_Data.daily_quest_list[9].todo = $"양식 문어 {Haenyeo.farm_item_number[6]}/3";
        quest_Data.daily_quest_list[10].todo = $"양식장 개수 {GameObject.Find("farm_manager").GetComponent<farm_manager>().Activated_farm_number()}/6";
        quest_Data.daily_quest_list[11].todo = $"연속 승리 {PlayerPrefs.GetInt("quest11", 0)}/3";
        quest_Data.daily_quest_list[12].todo = $"하루 50만원 송금하기";
        quest_Data.daily_quest_list[13].todo = $"전복 {Haenyeo.sea_item_number[7]}/4";
        quest_Data.daily_quest_list[14].todo = $"양식 꽃게 {Haenyeo.farm_item_number[5]}/5";
        quest_Data.daily_quest_list[15].todo = $"나의 물안경 단계 : {equipment_upgrade.my_goggle + 1}";
        quest_Data.daily_quest_list[16].todo = $"조개 {Haenyeo.sea_item_number[0]}/5";
        quest_Data.daily_quest_list[17].todo = $"양식 조개 {Haenyeo.farm_item_number[0]}/10";
        quest_Data.daily_quest_list[18].todo = $"나의 오리발 단계 : {equipment_upgrade.my_flipper + 1}";
        quest_Data.daily_quest_list[19].todo = $"거북이 {Haenyeo.sea_item_number[8]}/3";
        quest_Data.daily_quest_list[20].todo = $"양식 전복 {Haenyeo.farm_item_number[7]}/5";
        quest_Data.daily_quest_list[21].todo = $"하루 80만원 송금하기";
        quest_Data.daily_quest_list[22].todo = $"조개 {Haenyeo.sea_item_number[0]}/2  새우 {Haenyeo.sea_item_number[3]}/2  문어 {Haenyeo.sea_item_number[6]}/2";
        quest_Data.daily_quest_list[23].todo = $"양식 미역 {Haenyeo.farm_item_number[1]}/10";
        quest_Data.daily_quest_list[24].todo = $"금화 {PlayerPrefs.GetInt("quest24", 0)}/3";
        quest_Data.daily_quest_list[25].todo = $"꽃게 {Haenyeo.sea_item_number[5]}/5";
        quest_Data.daily_quest_list[26].todo = $"양식 꽃게 {Haenyeo.farm_item_number[5]}/3";
        quest_Data.daily_quest_list[27].todo = $"양식 전복 {Haenyeo.farm_item_number[7]}/3";
        quest_Data.daily_quest_list[28].todo = $"잡은 자원 종류 {cnt_item_type()}/8";
    }

    public int cnt_item_type()
    {
        int cc = 0;
        for (int i = 0; i < 9; i++) {
            if (Haenyeo.sea_item_number[i] > 0) cc++;
        }
        return cc;
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
            case 10: 
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
            case 17:
                if (Haenyeo.farm_item_number[0] >= 10) quest_Data.daily_quest_list[idx].state = 2;
                else if (quest_Data.daily_quest_list[idx].state != 0) quest_Data.daily_quest_list[idx].state = 1;
                break;
            case 18:
                if (equipment_upgrade.my_flipper >= 2) quest_Data.daily_quest_list[idx].state = 2;
                break;
            case 19:
                if (Haenyeo.sea_item_number[8] >= 3) quest_Data.daily_quest_list[idx].state = 2;
                else if (quest_Data.daily_quest_list[idx].state != 0) quest_Data.daily_quest_list[idx].state = 1;
                break;
            case 20:
                if (Haenyeo.farm_item_number[7] >= 5) quest_Data.daily_quest_list[idx].state = 2;
                else if (quest_Data.daily_quest_list[idx].state != 0) quest_Data.daily_quest_list[idx].state = 1;
                break;
            case 21:
                if (PlayerPrefs.GetInt("quest21", 0) >= 3) quest_Data.daily_quest_list[idx].state = 2;
                else if (quest_Data.daily_quest_list[idx].state != 0) quest_Data.daily_quest_list[idx].state = 1;
                break;
            case 22:
                if (Haenyeo.sea_item_number[0] >= 2 && Haenyeo.sea_item_number[3] >= 2 && Haenyeo.sea_item_number[6] >= 2) quest_Data.daily_quest_list[idx].state = 2;
                else if (quest_Data.daily_quest_list[idx].state != 0) quest_Data.daily_quest_list[idx].state = 1;
                break;
            case 23:
                if (Haenyeo.farm_item_number[1]>=10) quest_Data.daily_quest_list[idx].state = 2;
                else if (quest_Data.daily_quest_list[idx].state != 0) quest_Data.daily_quest_list[idx].state = 1;
                break;
            case 24:
                if (PlayerPrefs.GetInt("quest24", 0) >= 3) quest_Data.daily_quest_list[idx].state = 2;
                else if (quest_Data.daily_quest_list[idx].state != 0) quest_Data.daily_quest_list[idx].state = 1;
                break;
            case 25:
                if (Haenyeo.sea_item_number[5] >= 5) quest_Data.daily_quest_list[idx].state = 2;
                else if (quest_Data.daily_quest_list[idx].state != 0) quest_Data.daily_quest_list[idx].state = 1;
                break;
            case 26:
                if (Haenyeo.farm_item_number[5] >= 3) quest_Data.daily_quest_list[idx].state = 2;
                else if (quest_Data.daily_quest_list[idx].state != 0) quest_Data.daily_quest_list[idx].state = 1;
                break;
            case 27:
                if (Haenyeo.farm_item_number[7] >= 3) quest_Data.daily_quest_list[idx].state = 2;
                else if (quest_Data.daily_quest_list[idx].state != 0) quest_Data.daily_quest_list[idx].state = 1;
                break;
            case 28:
                if (Haenyeo.sea_item_number[0] > 0 && Haenyeo.sea_item_number[1] > 0 && Haenyeo.sea_item_number[2] > 0 && Haenyeo.sea_item_number[3] > 0 && Haenyeo.sea_item_number[4] > 0 && Haenyeo.sea_item_number[5] > 0 && Haenyeo.sea_item_number[6] > 0 && Haenyeo.sea_item_number[7] > 0 && Haenyeo.sea_item_number[8] > 0) quest_Data.daily_quest_list[idx].state = 2;
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
                Haenyeo.item_inven[0] += 3;
                break;
            case 6:
                Haenyeo.sea_item_number[5] -= 2;
                StartCoroutine(GameObject.Find("quest_manager").GetComponent<quest_manager>().reward_effect("bonus", 0));
                Haenyeo.item_inven[1] += 2;
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
                Haenyeo.item_inven[0] += 5;
                break;
            case 9:
                Haenyeo.farm_item_number[6] -= 3;
                Haenyeo.money += 40000;
                StartCoroutine(GameObject.Find("quest_manager").GetComponent<quest_manager>().reward_effect("money", 40000));
                break;
            case 10:
                StartCoroutine(GameObject.Find("quest_manager").GetComponent<quest_manager>().reward_effect("kick", 0));
                Haenyeo.item_inven[1] += 3;
                break;
            case 11:
                Haenyeo.money += 100000;
                StartCoroutine(GameObject.Find("quest_manager").GetComponent<quest_manager>().reward_effect("money", 100000));
                break;
            case 12:
                StartCoroutine(GameObject.Find("quest_manager").GetComponent<quest_manager>().reward_effect("bonus", 0));
                Haenyeo.item_inven[3] += 3;
                break;
            case 13:
                Haenyeo.sea_item_number[7] -= 4;
                StartCoroutine(GameObject.Find("quest_manager").GetComponent<quest_manager>().reward_effect("double", 0));
                Haenyeo.item_inven[2] += 3;
                break;
            case 14:
                Haenyeo.farm_item_number[5] -= 5;
                Haenyeo.money += 120000;
                StartCoroutine(GameObject.Find("quest_manager").GetComponent<quest_manager>().reward_effect("money", 120000));
                break;
            case 15:
                StartCoroutine(GameObject.Find("quest_manager").GetComponent<quest_manager>().reward_effect("net", 0));
                Haenyeo.item_inven[0] += 3;
                break;
            case 16:
                Haenyeo.sea_item_number[0] -= 5;
                StartCoroutine(GameObject.Find("quest_manager").GetComponent<quest_manager>().reward_effect("double", 0));
                Haenyeo.item_inven[2] += 4;
                break;
            case 17:
                Haenyeo.farm_item_number[0] -= 10;
                Haenyeo.money += 70000;
                StartCoroutine(GameObject.Find("quest_manager").GetComponent<quest_manager>().reward_effect("money", 70000));
                break;
            case 18:
                Haenyeo.item_inven[0] += 3;
                StartCoroutine(GameObject.Find("quest_manager").GetComponent<quest_manager>().reward_effect("net", 0));
                break;
            case 19:
                Haenyeo.sea_item_number[7] -= 3;
                Haenyeo.item_inven[3] += 3;
                StartCoroutine(GameObject.Find("quest_manager").GetComponent<quest_manager>().reward_effect("bonus", 0));
                break;
            case 20:
                Haenyeo.farm_item_number[7] -= 5;
                Haenyeo.money += 50000;
                StartCoroutine(GameObject.Find("quest_manager").GetComponent<quest_manager>().reward_effect("money", 50000));
                break;
            case 21:
                StartCoroutine(GameObject.Find("quest_manager").GetComponent<quest_manager>().reward_effect("kick", 0));
                Haenyeo.item_inven[1] += 5;
                break;
            case 22:
                Haenyeo.sea_item_number[0] -= 2;
                Haenyeo.sea_item_number[3] -= 2;
                Haenyeo.sea_item_number[6] -= 2;
                Haenyeo.money += 100000;
                StartCoroutine(GameObject.Find("quest_manager").GetComponent<quest_manager>().reward_effect("money", 100000));
                break;
            case 23:
                Haenyeo.farm_item_number[0] -= 10;
                Haenyeo.money += 150000;
                StartCoroutine(GameObject.Find("quest_manager").GetComponent<quest_manager>().reward_effect("money", 100000));
                break;
            case 24:
                PlayerPrefs.DeleteKey("quest24");
                Haenyeo.item_inven[3] += 3;
                StartCoroutine(GameObject.Find("quest_manager").GetComponent<quest_manager>().reward_effect("bonus", 0));
                break;
            case 25:
                Haenyeo.sea_item_number[5] -= 5;
                Haenyeo.money += 70000;
                StartCoroutine(GameObject.Find("quest_manager").GetComponent<quest_manager>().reward_effect("money", 70000));
                break;
            case 26:
                Haenyeo.farm_item_number[5] -= 3;
                Haenyeo.money += 50000;
                StartCoroutine(GameObject.Find("quest_manager").GetComponent<quest_manager>().reward_effect("money", 50000));
                break;
            case 27:
                Haenyeo.farm_item_number[7] -= 3;
                Haenyeo.money += 70000;
                StartCoroutine(GameObject.Find("quest_manager").GetComponent<quest_manager>().reward_effect("money", 70000));
                break;
            case 28:
                Haenyeo.sea_item_number[0] -= 1;
                Haenyeo.sea_item_number[1] -= 1;
                Haenyeo.sea_item_number[2] -= 1;
                Haenyeo.sea_item_number[3] -= 1;
                Haenyeo.sea_item_number[4] -= 1;
                Haenyeo.sea_item_number[5] -= 1;
                Haenyeo.sea_item_number[6] -= 1;
                Haenyeo.sea_item_number[7] -= 1;
                Haenyeo.sea_item_number[8] -= 1;
                Haenyeo.money += 200000;
                StartCoroutine(GameObject.Find("quest_manager").GetComponent<quest_manager>().reward_effect("money", 200000));
                break;
        }
    }
}
