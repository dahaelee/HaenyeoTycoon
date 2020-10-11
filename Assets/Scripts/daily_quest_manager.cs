using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class daily_quest_manager : MonoBehaviour
{
    void Update()
    {
        int questReady = PlayerPrefs.GetInt("questReady", 1);
        int doneFlag = PlayerPrefs.GetInt("doneFlag", 0);

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
        quest_Data.daily_quest_list[5].todo = $"연속 승리 {PlayerPrefs.GetInt("quest5", 0)}/3";
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
                if (PlayerPrefs.GetInt("quest5", 0) == 3) quest_Data.daily_quest_list[idx].state = 2;
                else if (quest_Data.daily_quest_list[idx].state != 0) quest_Data.daily_quest_list[idx].state = 1;
                break;
        }
    }

    public void quest_reward(int idx)
    {
        switch (idx)
        {
            case 0:
                Haenyeo.sea_item_number[0] -= 5;
                Haenyeo.money += 20000;
                StartCoroutine(GameObject.Find("quest_manager").GetComponent<quest_manager>().reward_effect(20000));
                break;
            case 1:
                Haenyeo.farm_item_number[1] -= 3;
                Haenyeo.money += 30000;
                StartCoroutine(GameObject.Find("quest_manager").GetComponent<quest_manager>().reward_effect(30000));
                break;
            case 2:
                Haenyeo.farm_item_number[3] -= 2;
                Haenyeo.farm_item_number[5] -= 1;
                Haenyeo.money += 50000;
                StartCoroutine(GameObject.Find("quest_manager").GetComponent<quest_manager>().reward_effect(50000));
                break;
            case 3:
                PlayerPrefs.DeleteKey("quest3");
                Haenyeo.money += 50000;
                StartCoroutine(GameObject.Find("quest_manager").GetComponent<quest_manager>().reward_effect(50000));
                break;
            case 4:
                Haenyeo.money += 30000;
                StartCoroutine(GameObject.Find("quest_manager").GetComponent<quest_manager>().reward_effect(30000));
                break;
            case 5:
                PlayerPrefs.DeleteKey("quest5");
                Debug.Log("quest5 완료!");
                break;
        }
    }
}
