using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class sea : MonoBehaviour
{
    public float time_remaining, time_max, countdown;
    public Image return_to_farms_ui, show_mesh_ui, timeover_return_ui, block_touch, start_button, item_button_lock, block_start;
    public Image text_window, next_triangle;
    public Sprite[] depth_arr; // 깊이바 이미지 배열
    public Image depth, location; // 깊이바 이미지, 위치 표시 이미지
    public Text countdown_text, hp_num_text;
    public GameObject[] items_got; //그물망 속 자원 배열 
    public int[] item_num; //그물망 속 자원개수 배열
    public Text[] item_num_text; //그물망 속 자원개수 텍스트 배열 
    public SpriteRenderer sunlight_render; //햇빛 이미지의 렌더러
    public Vector3 pos;
    public int level;
    public GameObject haenyeo, gagebar, tutorial_parent, sunlight;
    public AudioSource bgm, icon_click, button_click, ready, go, double_sound, hp_sound;
    public GameObject[] tutorials;
    public static bool is_double;

    void Start()
    {
        int IsSeaNew = PlayerPrefs.GetInt("IsSeaNew", 1);
        if (IsSeaNew == 1)
        {
            tutorial_parent.SetActive(true);
            start_button.gameObject.SetActive(false);
            PlayerPrefs.SetInt("IsSeaNew", 0);
        }
        else
        {
            tutorial_parent.SetActive(false);
            start_button.gameObject.SetActive(true);
        }

        block_touch.gameObject.SetActive(true);
        return_to_farms_ui.gameObject.SetActive(false);
        show_mesh_ui.gameObject.SetActive(false);
        timeover_return_ui.gameObject.SetActive(false);
        timeover_return_ui.gameObject.SetActive(false);
        item_button_lock.gameObject.SetActive(false);
        block_start.gameObject.SetActive(false);

        StartCoroutine("triangle_effect");

        countdown = 10.0f;
        level = Haenyeo.level;

        //그물망 속 자원 개수 전부 0으로 초기화 
        for (int i = 0; i < 9; i++)
            item_num[i] = 0;

        bgm.volume = PlayerPrefs.GetFloat("Bgm_volume", 1);
        icon_click.volume = PlayerPrefs.GetFloat("Effect_volume", 1);
        button_click.volume = PlayerPrefs.GetFloat("Effect_volume", 1);
        ready.volume = PlayerPrefs.GetFloat("Effect_volume", 1);
        go.volume = PlayerPrefs.GetFloat("Effect_volume", 1);
        double_sound.volume = PlayerPrefs.GetFloat("Effect_volume", 1);
        hp_sound.volume = PlayerPrefs.GetFloat("Effect_volume", 1);

        // 해녀 레벨에 따라 깊이바 이미지 변경
        depth.gameObject.GetComponent<Image>().sprite = depth_arr[Haenyeo.level-1];

        is_double = false;
    }

    void Update()
    {
        sunlight_transparency_change(); //햇빛 투명도 조정 함수 호출

        if (!block_touch.gameObject.activeSelf) //터치 방지가 비활성화 상태일 때만
            Haenyeo.hp -= Time.deltaTime; //1초당 1씩 체력 감소

        if (Haenyeo.hp <= 0) //체력이 0보다 작아지면
        {
            block_touch.gameObject.SetActive(true); //팝업창 외의 영역 터치 방지
            timeover_return_ui.gameObject.SetActive(true); //카운트다운 팝업창 활성화

            // 체력템 없으면 버튼 비활성화
            if (Haenyeo.item_inven[3] < 1)  
                item_button_lock.gameObject.SetActive(true);
            else
                item_button_lock.gameObject.SetActive(false);

            countdown -= Time.deltaTime; //시간의 흐름에 따라 카운트다운 감소
            countdown_text.text = Mathf.CeilToInt(countdown).ToString(); //남은 카운트다운을 countdown_text에 담기 

            hp_num_text.text = Haenyeo.item_inven[3].ToString(); //hp 아이템 개수

            if (countdown <= 0) //카운트다운이 0보다 작아지면
            {
                countdown = 0; //마이너스값이 뜨지 않도록 0으로 
                timeover_return_ui.gameObject.SetActive(false); //카운트다운 팝업창 비활성화
                PlayerPrefs.Save();
                load_farm(); //양식장으로 이동
            }
        }

        // 깊이바 추가로 인해 왼쪽 끝으로 갈때 수치 조정함 (475->415)
        if (haenyeo.transform.position.x < -415) //해녀의 위치가 화면 왼쪽 끝으로 가면
            gagebar.transform.localPosition = new Vector3((-415 - haenyeo.transform.position.x)/100, 0.85f, -1f); //게이지바가 화면을 넘어가지 않도록 조정
        if (haenyeo.transform.position.x >= -415 && haenyeo.transform.position.x <= 475) //해녀의 위치가 화면 양 끝이 아니면
            gagebar.transform.localPosition = new Vector3(0f, 0.85f, -1f); //게이지바가 원래 위치에 있도록
        if (haenyeo.transform.position.x > 475) //해녀의 위치가 화면 오른쪽 끝으로 가면
            gagebar.transform.localPosition = new Vector3((475 - haenyeo.transform.position.x)/100, 0.85f, -1f); //게이지바가 화면을 넘어가지 않도록 조정

        // 해녀 깊이바 위치표시 이동
        location.transform.localPosition = new Vector3(3f, haenyeo.transform.position.y * 0.182f - 131.04f, 0f);
    }

    //깊어질수록 햇빛의 투명도 증가시키기  
    public void sunlight_transparency_change()
    {
        pos = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane)); //pos는 메인카메라의 월드좌표계
        
        float y = (float)pos[1]; //pos[1]은 메인카메라의 월드좌표계의 y좌표
        float y1;

        if (is_double)
            y1 = (float)(y * 0.00024306 + 0.65); //적당하게 보정한 투명도 값 y1
        else
            y1 = (float)(y * 0.000445 + 0.312); //적당하게 보정한 투명도 값 y1

        sunlight_render.color = new Color(1, 1, 1, y1); //빛의 투명도를 y1으로 하여 sunlight 렌더링 
    }

    //바다 첫 화면에서 버튼 눌러 시작하기 
    public void sea_start()
    {
        block_start.gameObject.SetActive(true);
        StartCoroutine("start_effect");
    }
    IEnumerator start_effect()
    {
        float shrink_time = 0.2f;
        float time = 0f;

        ready.PlayOneShot(ready.clip);
        yield return StartCoroutine("start_effect_in"); //버튼이 다 커질때까지 대기

        while (start_button.transform.localScale.x > 0)
        {
            time += Time.deltaTime / shrink_time;
            float scale = Mathf.Lerp(2f, 0f, time);
            start_button.transform.localScale = new Vector3(scale, scale, 1);
            yield return null;
        }

        block_start.gameObject.SetActive(false);
        go.PlayOneShot(go.clip);
        yield return new WaitForSeconds(0.1f);

        block_touch.gameObject.SetActive(false); //팝업창 외의 영역 터치 방지 해제
        start_button.gameObject.SetActive(false); //시작 버튼 비활성화
    }

    IEnumerator start_effect_in()
    {
        float expand_time = 1.2f;
        float time = 0f;

        while (start_button.transform.localScale.x < 1.7f)
        {
            time += Time.deltaTime / expand_time;
            float scale = Mathf.Lerp(1f, 1.7f, time);
            start_button.transform.localScale = new Vector3(scale, scale, 1);
            yield return null;
        }
    }

    //양식장으로 돌아가기 
    public void return_to_farms()
    {
        icon_click.PlayOneShot(icon_click.clip); 
        return_to_farms_ui.gameObject.SetActive(true);//물음 팝업창 활성화
        block_touch.gameObject.SetActive(true); //팝업창 외의 영역 터치 방지
    }

    //물음 팝업창에서 yes 선택 
    public void return_to_farms_yes()
    {
        return_to_farms_ui.gameObject.SetActive(false); //물음 팝업창 비활성화 
        farm_manager.is_repay_locked = false;
        farm_manager.is_sea_locked = true;
        PlayerPrefs.SetInt("is_repay_locked", 0);
        PlayerPrefs.Save();
        load_farm(); //양식장으로 이동
    }

    //물음 팝업창에서 no 선택 
    public void return_to_farms_no()
    {
        return_to_farms_ui.gameObject.SetActive(false); //물음 팝업창 비활성화
        block_touch.gameObject.SetActive(false); //팝업창 외의 영역 터치 방지 해제 
    }

    // 체력 0 됐을때 카운트다운 팝업창
    public void use_hp_item()
    {
        hp_sound.PlayOneShot(hp_sound.clip);
        Haenyeo.item_inven[3] -= 1; // 체력템 개수 -1
        // load_farm 함수에 템 개수 저장하기
        block_touch.gameObject.SetActive(false); //팝업창 외의 영역 터치 방지 해제 
        Haenyeo.hp += 10; // 체력 추가
        countdown = 10.0f; // 카운트다운 복구
        timeover_return_ui.gameObject.SetActive(false); //카운트다운 팝업창 비활성화
    }

    //그물망 보여주기
    public void show_mesh()
    {
        button_click.PlayOneShot(button_click.clip);
        show_mesh_ui.gameObject.SetActive(true); //그물망 팝업창 활성화 
        block_touch.gameObject.SetActive(true); //팝업창 외의 영역 터치 방지

        for (int i = 0; i < 9; i++)
            item_num_text[i].text = item_num[i].ToString(); //각 자원 개수를 item_num_text에 담기 

        for (int i = 0; i < 9; i++)
        {
            if (item_num[i] > 0) //한 개 이상 있으면
                items_got[i].SetActive(true); //자원 활성화

            else //한 개도 없으면 
                items_got[i].SetActive(false); // 자원 비활성화
        }
    }

    //그물망 닫기
    public void close_mesh()
    {
        if (show_mesh_ui.gameObject.activeSelf) //그물망 팝업창이 활성화돼 있을 때만 
        {
            show_mesh_ui.gameObject.SetActive(false); //그물망 팝업창 비활성화
            block_touch.gameObject.SetActive(false); //팝업창 외의 영역 터치 방지 해제
        }
    }

    public void item_double()
    {
        Haenyeo.item_inven[2] -= 1;
        StartCoroutine(start_double());
    }

    public IEnumerator start_double()
    {
        double_sound.PlayOneShot(double_sound.clip);

        sunlight.GetComponent<Animator>().SetBool("isDouble", true);
        is_double = true;

        yield return StartCoroutine(wait(10f));

        IEnumerator wait(float delay) //delay만큼 대기 (WaitForSeconds는 타이머 일시정지가 안돼서 만듦)
        {
            float time = 0f;
            while (time < delay) //wait의 실행시간이 delay가 될때까지
            {
                yield return null; //프레임은 변하지 않으면서
                if (!block_touch.gameObject.activeSelf) //터치 방지가 비활성화 상태여야만
                {
                    time += Time.deltaTime; //시간이 가게 함
                }
            }
        }

        is_double = false;
        sunlight.GetComponent<Animator>().SetBool("isDouble", false); 
    }

    //양식장으로 이동할때 쓰는 함수
    public void load_farm()
    {
        PlayerPrefs.SetInt("Haenyeo_sea_item_number0", Haenyeo.sea_item_number[0]);
        PlayerPrefs.SetInt("Haenyeo_sea_item_number1", Haenyeo.sea_item_number[1]);
        PlayerPrefs.SetInt("Haenyeo_sea_item_number2", Haenyeo.sea_item_number[2]);
        PlayerPrefs.SetInt("Haenyeo_sea_item_number3", Haenyeo.sea_item_number[3]);
        PlayerPrefs.SetInt("Haenyeo_sea_item_number4", Haenyeo.sea_item_number[4]);
        PlayerPrefs.SetInt("Haenyeo_sea_item_number5", Haenyeo.sea_item_number[5]);
        PlayerPrefs.SetInt("Haenyeo_sea_item_number6", Haenyeo.sea_item_number[6]);
        PlayerPrefs.SetInt("Haenyeo_sea_item_number7", Haenyeo.sea_item_number[7]);
        PlayerPrefs.SetInt("Haenyeo_sea_item_number8", Haenyeo.sea_item_number[8]);

        PlayerPrefs.SetInt("Haenyeo" + "_" + "money", Haenyeo.money); // 바다에서 코인으로 번 돈 저장용
        PlayerPrefs.SetFloat("Haenyeo_hp", Haenyeo.hp); // 체력 저장

        PlayerPrefs.SetInt("Haenyeo_item_inven_number0", Haenyeo.item_inven[0]);
        PlayerPrefs.SetInt("Haenyeo_item_inven_number1", Haenyeo.item_inven[1]);
        PlayerPrefs.SetInt("Haenyeo_item_inven_number2", Haenyeo.item_inven[2]);
        PlayerPrefs.SetInt("Haenyeo_item_inven_number3", Haenyeo.item_inven[3]);

        StartCoroutine("fade_out"); //화면 페이드아웃 후 양식장으로 전환
    }
    public IEnumerator fade_out()
    {
        float fade_time = 0.5f;
        float start = 0.75f;
        float end = 0.85f;
        float time = 0f;
        Color fade_color = block_touch.color;

        while (fade_color.a < end)
        {
            time += Time.deltaTime / fade_time;
            fade_color.a = Mathf.Lerp(start, end, time);
            block_touch.color = fade_color;
            yield return null;
        }

        SceneManager.LoadScene("farm"); //양식장 씬으로 전환
    }

    public void next_tutorial(int index)
    {
        tutorials[index].gameObject.SetActive(false);
    }

    public void last_tutorial()
    {
        start_button.gameObject.SetActive(true);
    }

    IEnumerator triangle_effect()
    {
        while (text_window.IsActive())
        {
            next_triangle.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            next_triangle.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.3f);
        }
    }
}