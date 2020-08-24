using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_manager : MonoBehaviour
{
    public Image[] UIs;
    public static UIstate currentState = UIstate.None;
    

    public enum UIstate
    {
        UIbackground,   //
        farm_info,  //양식장 정보
        farmable_item,  // 양식 가능한 자원 UI
        trash,  //양식장을 비우시겠습니까?
        expand,     //양식장을 확장?
        repay,      //송금하시겠습니까?
        go_sea,     //바다로 가시겠습니까?
        interest_warning,   //이자가 붙습니다. 그래도 송금?
        no_money,       //소지금이 부족합니다.
        ask_quit,       //게임을 종료?
        setting,        //설정 UI창
        restart,        //게임을 초기화?
        ending,     
        no_item,
        None
    }

    //public Image quest_list;

    // Start is called before the first frame update
    void Start()
    {

        //quest_list.gameObject.SetActive(false);
    }

    // Update is called once per frame
    public void quest_ui()
    {
        //사운드 넣기
        //quest_list.gameObject.SetActive(false);
    }

    public void AllUIoff()
    {
        for(int i=0; i<UIs.Length; i++)
        {
            UIs[i].gameObject.SetActive(false);
        }
        currentState = UIstate.None;
    }

    //UI 창 오픈하는 코드와 이펙트
    IEnumerator UI_On(UIstate uistate)
    {
        currentState = uistate;
        yield return new WaitForSeconds(0.1f);
        UIs[(int)UIstate.UIbackground].gameObject.SetActive(true);
        UIs[(int)uistate].gameObject.SetActive(true);
        for (int i = 0; i < 5; i++)
        {
            UIs[(int)uistate].rectTransform.localScale = new Vector3((float)(0.95 + i * 0.01), (float)(0.95 + i * 0.01), (float)(0.95 + i * 0.01));
            yield return 0;
        }

    }

    public void UIoff(UIstate index)
    {
        UIs[(int)index].gameObject.SetActive(false);
        // 여기선 current UI State를 어떻게 지정해야할지...?
    }
}
