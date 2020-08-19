using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class sea_movement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public RectTransform rect_background, rect_joystick, rect_screen;
    public float radius, speed, distance, clamped_1, clamped_2, clamped_3, clamped_length;
    public GameObject player;
    public Vector3 move_position; //이동 좌표
    public Vector2 value;
    public Camera cam;
    public SpriteRenderer player_render; //해녀 이미지의 렌더러 
    public Sprite front_sprite; //해녀 정면 이미지
    public Vector3 min_bound1, max_bound1, min_bound2, max_bound2, min_bound3, max_bound3;
    public BoxCollider2D bound1, bound2, bound3;
    public int level;
    public Animator haenyeo_anim;

    void Start()
    {
        player_render.sprite = front_sprite; //해녀 이미지 정면으로 
        radius = rect_background.rect.width * 0.5f; //배경의 반지름
        speed = (float)Haenyeo.moving_speed; 
        level = Haenyeo.level; 

        min_bound1 = bound1.bounds.min;
        max_bound1 = bound1.bounds.max;
        min_bound2 = bound2.bounds.min;
        max_bound2 = bound2.bounds.max;
        min_bound3 = bound3.bounds.min;
        max_bound3 = bound3.bounds.max;

        haenyeo_anim.SetBool("is_moving", false);
        haenyeo_anim.SetBool("front", false);
        haenyeo_anim.SetBool("right", false);
        haenyeo_anim.SetBool("left", false);
    }

    void Update()
    {
        player.GetComponent<Rigidbody2D>().AddForce(move_position*100);

        if (level == 1) 
        {
            clamped_length = Mathf.Clamp(player.transform.position.x, min_bound1.x + 50, max_bound1.x - 50);
            clamped_1 = Mathf.Clamp(player.transform.position.y, min_bound1.y + 80, max_bound1.y - 80);
            player.transform.position = new Vector3(clamped_length, clamped_1, player.transform.position.z);
        }
        if (level == 2)
        {
            clamped_length = Mathf.Clamp(player.transform.position.x, min_bound1.x + 50, max_bound1.x - 50);
            clamped_2 = Mathf.Clamp(player.transform.position.y, min_bound2.y + 80, max_bound2.y - 80);
            player.transform.position = new Vector3(clamped_length, clamped_2, player.transform.position.z);
        }
        if (level == 3)
        {
            clamped_length = Mathf.Clamp(player.transform.position.x, min_bound1.x + 50, max_bound1.x - 50);
            clamped_3 = Mathf.Clamp(player.transform.position.y, min_bound3.y + 80, max_bound3.y - 80);
            player.transform.position = new Vector3(clamped_length, clamped_3, player.transform.position.z);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //터치 좌표를 anchoredPosition으로 변환
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rect_screen, eventData.position, cam, out pos))
        {
            Vector2 pivotOffset = rect_screen.pivot * rect_screen.sizeDelta;
            pos = pos - (rect_background.anchorMax * rect_screen.sizeDelta) + pivotOffset;
        }
    }

    public void OnDrag(PointerEventData eventData) //eventData는 스크린좌표
    {

        //터치 좌표를 anchoredPosition으로 변환
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rect_screen, eventData.position, cam, out pos))
        {
            Vector2 pivotOffset = rect_screen.pivot * rect_screen.sizeDelta;
            pos = pos - (rect_background.anchorMax * rect_screen.sizeDelta) + pivotOffset;
        }

        value = pos - (Vector2)rect_background.anchoredPosition; //value = 터치 좌표 - 배경 좌표
        value = Vector2.ClampMagnitude(value, radius/2); //스틱을 배경에 가두기 
        rect_joystick.localPosition = value; //스틱을 배경에서 value만큼 떨어지도록

        distance = Vector2.Distance((Vector2)rect_background.position, (Vector2)rect_joystick.position) / radius * 2; //distance는 배경과 스틱의 거리차와 비례 (0~1)
        value = value.normalized; //value의 속도값은 빼고 방향값만 남기기
        move_position = new Vector3(value.x * speed * distance, value.y * speed * distance, 0f); //이동 좌표를 방향*속도*거리 로 구하기 
        //move_position = new Vector3(value.x * speed * Time.deltaTime * distance, value.y * speed * Time.deltaTime * distance, 0f); //델타타임 포함된 이동좌표 

        haenyeo_anim.SetBool("is_moving", true);

        if (value.x > 0.3)
        {
            player.transform.rotation = Quaternion.Euler(0, 0, value.y * 30); //해녀 이미지 회전하기
            haenyeo_anim.SetBool("front", false);
            haenyeo_anim.SetBool("right", true);
            haenyeo_anim.SetBool("left", false);
        }
        if (value.x > -0.3 && value.x < 0.3)
        {
            player.transform.rotation = Quaternion.Euler(0, 0, 0); //해녀 이미지 회전 초기화
            haenyeo_anim.SetBool("front", true);
            haenyeo_anim.SetBool("right", false);
            haenyeo_anim.SetBool("left", false);
        }
        if (value.x < -0.3)
        {
            player.transform.rotation = Quaternion.Euler(0, 0, -value.y * 30); //해녀 이미지 회전하기
            haenyeo_anim.SetBool("front", false);
            haenyeo_anim.SetBool("right", false);
            haenyeo_anim.SetBool("left", true);
        }
    }

    public void OnPointerUp(PointerEventData eventData)  //터치가 끝나면
    {
        rect_joystick.localPosition = Vector3.zero; //스틱을 원위치
        move_position = Vector3.zero; //이동 좌표 초기화
        player_render.sprite = front_sprite; //해녀 이미지 정면으로
        player.transform.rotation = Quaternion.Euler(0, 0, 0); //해녀 이미지 회전 초기화 

        haenyeo_anim.SetBool("is_moving", false);
    }
}
