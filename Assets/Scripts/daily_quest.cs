using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class daily_quest_form
{
    public string summary;
    public string type;
    public int person;
    public string text;
    public string todo;
    public string reward;

    public daily_quest_form(string summary, string type,int person,string text,string todo,string reward) {
        this.summary = summary;
        this.type = type;
        this.person = person;
        this.text = text;
        this.todo = todo;
        this.reward = reward;
    }
}
*/
public class daily_quest : MonoBehaviour
{
    public static Daily_quest_form daily_quest1 
        = new Daily_quest_form(
            0,
            "조개 5개, 꽃게 1개, 미역 2개 채집하기",
            "점심 식사 준비",
            1,
            "해녀야.. 오늘 점심은 해물라면 어떠니? \n아빠가 아주 맛있게 끓여줄게..!\n아래 재료를 부탁한단다..",
            "조개   /5  꽃게    /1  미역    /2",
            "체력 아이템"
        );
}
