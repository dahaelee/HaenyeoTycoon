using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Tutorial_Serialization
{

    [SerializeField] List<Tutorial_quest_form> tutorial_list;    //진행중인 퀘스트 목록
    public List<Tutorial_quest_form> toList() { return tutorial_list; }

    public Tutorial_Serialization(List<Tutorial_quest_form> tutorial_list)
    {
        this.tutorial_list = tutorial_list;
    }
}

[Serializable]
public class Daily_Serialization
{

    [SerializeField] List<Daily_quest_form> daily_list;    //진행중인 퀘스트 목록
    public List<Daily_quest_form> toList() { return daily_list; }

    public Daily_Serialization(List<Daily_quest_form> daily_list)
    {
        this.daily_list = daily_list;
    }
}

[System.Serializable]
public class Tutorial_quest_form
{
    [SerializeField] public int state;   //0. new 1. ing 2. done -1. 
    [SerializeField] public string summary;
    [SerializeField] public string type;

    public Tutorial_quest_form(int state, string type, string summary)
    {
        this.state = state;
        this.type = type;
        this.summary = summary;
    }

}

[System.Serializable]
public class Daily_quest_form
{
    [SerializeField] public int state;//0. new 1. ing 2. done, -1 미확인 퀘스트
    [SerializeField] public string summary;
    [SerializeField] public string type;
    [SerializeField] public int person; //0. 사채업자 1. 아빠 2. 상인아저씨
    [SerializeField] public string text; // 말풍선 텍스트
    [SerializeField] public string todo; // 해야할 일
    [SerializeField] public string reward;   // 보상

    public Daily_quest_form(int state, string summary, string type, int person, string text, string todo, string reward)
    {
        this.state = state;
        this.summary = summary;
        this.type = type;
        this.person = person;
        this.text = text;
        this.todo = todo;
        this.reward = reward;
    }
}

public class quest_Data : MonoBehaviour
{
    [SerializeField] public static List<Tutorial_quest_form> tutorial_quest_list = new List<Tutorial_quest_form>();  //튜토리얼 퀘스트 리스트
    [SerializeField] public static List<Daily_quest_form> daily_quest_list = new List<Daily_quest_form>();  //일일 퀘스트 리스트

    //빚 관련
    public enum quest_debt
    {
        debt1=500000,
        debt2=2000000,
        debt3=5000000
    }

    public enum quest_endDay
    {
        endDay1=5,
        endDay2=10,
        endDay3=15
    }

    public void newStart()
    {
        PlayerPrefs.DeleteKey("tutorial_quest");
        PlayerPrefs.DeleteKey("daily_quest");

        tutorial_quest_list.Clear();
        daily_quest_list.Clear();

        //튜토리얼 퀘스트 추가
        tutorial_quest_list.Add(new Tutorial_quest_form(-1, "아빠의 가르침", "바다에서 자원 1개 이상 채집하기"));
        tutorial_quest_list.Add(new Tutorial_quest_form(-1, "아빠의 가르침", "자원 양식하기"));
        tutorial_quest_list.Add(new Tutorial_quest_form(-1, "아빠의 가르침", "상점 방문하기"));

        //일일 퀘스트 추가
        daily_quest_list.Add(new Daily_quest_form(-1,"조개 3개 채집하기","제철 해산물",2, "과일도 제철 과일이 맛있고,\n해산물도 제철 해산물이 맛있지.\n그런고로, 오늘은 조개를 가져오면 짭짤하게 쳐주마!","","2만원"));
        daily_quest_list.Add(new Daily_quest_form(-1, "양식 미역 3번 수확하기", "양식의 달인", 1, "해녀 생활을 훌륭하게 잘 해내고 있구나\n양식 하는 건 아직 미숙하던데\n미역 3번만 수확해 보겠니?", "", "3만원"));
        daily_quest_list.Add(new Daily_quest_form(-1, "새우 4개 채집하기", "오늘의 점심", 1, "해녀야,, 오늘따라 새우구이가 끌리지 않니..?\n새우 4개만 가져와 주렴 ..\n그럼.. 부탁할게..", "", "5만원"));
        daily_quest_list.Add(new Daily_quest_form(-1, "바다에서 금화 3개 모으기", "바다에서 돈모으기", 1, "해녀야 바다에서 가끔식 동전이 떨어지는 걸 봤니?\n동전을 먹어보렴! 꽤나 쏠쏠한 수입이 될 거야..!!", "", "5만원"));
        daily_quest_list.Add(new Daily_quest_form(-1, "해파리 3개 채집하기", "제철 해산물", 2, "옆 가게에서 해파리 냉채 메뉴를 게시했다더구나!!\n해파리를 얼른 준비해 놔야겠어\n해녀야 너가 도와주겠니?", "", "3만원"));
        daily_quest_list.Add(new Daily_quest_form(-1, "3번 연속 지지 않기", "내기의 달인", 2, "너의 가위바위보 솜씨가 예사롭지 않더구나\n나의 승부욕을 자극하고 있어\n내기에 내기를 더해볼까?", "", "싹쓸이 그물 3개 "));
        daily_quest_list.Add(new Daily_quest_form(-1, "꽃게 2개 채집하기", "오늘의 점심", 1, "요즘 꽃게가 철이라고 하더구나\n꽃게 두마리만 부탁하마\n아참! 꽃게는 해저 10미터 미만에서만 잡히는 걸 알아두렴!", "", "체력 추가 2개"));
        daily_quest_list.Add(new Daily_quest_form(-1, "새우,꽃게,문어 한개씩 채집하기", "오늘의 점심", 1, "오늘 점심을 해물 라면 어떠니?\n어제 상점 아저씨와 과음을 했더니 해장이 필요할 거 같아..\n아빠가 라면은 잘 끓이잖니!!", "", "4만원"));
        daily_quest_list.Add(new Daily_quest_form(-1, "기능성 오리발 구입하기", "장비 업그레이드", 2, "오늘부터 장비 제품 행사가 시작된단다! 오리발 업그레이드 시 싹쓸이 그물 5개를 얻어갈 수 있으니\n좋은 기회 놓치지 마렴", "", "싹쓸이 그물 5개"));
        daily_quest_list.Add(new Daily_quest_form(-1, "양식 문어 3번 수확하기", "양식의 달인", 2, "긴급긴급!! 오늘 등산 동호회가 온다더구나 혹시 몰라 문어를 많이 공수해놓아야겠어\n해녀야 부탁하마!", "", "4만원"));
        daily_quest_list.Add(new Daily_quest_form(-1, "양식장 6개 확장하기", "양식의 달인", 1, "해녀야.. 이제는 정말 베테랑 해녀라고 불러도 되겠어 양식장을 늘리면 한번에 더 많은 자원을 양식할 수 있단다\n양식장을 늘려보겠니?", "", "중간 구슬 3개"));

        //첫 튜토 시작
        GameObject.Find("tutorial_quest").GetComponent<tutorial_quest>().Sache1();

    }
}
