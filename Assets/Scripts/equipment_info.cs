using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class equipment_info : MonoBehaviour//, IPointerDownHandler, IPointerUpHandler
{
    public Image equipment_info_obj; // 장비 레이아웃 
    public int next_upgrade_price;    // 다음 장비로 업그레이드에 필요한 가격. -1로 설정할 경우 다음 업그레이드가 없음.
    public int speed_up; // 오리발만 해당, 나머지는 -1로 설정.
    public int time_up; // 해녀복, 물안경 해당, 오리발은 -1로 설정.
    public Text equip_name;
    public bool isInfoClick = false;

    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    isInfoClick = true;
    //}

    //public void OnPointerUp(PointerEventData eventData)
    //{
    //    isInfoClick = false;
    //}

    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
