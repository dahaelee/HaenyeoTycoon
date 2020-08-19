using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sea_camera_manager : MonoBehaviour
{
    public GameObject target; //카메라가 따라갈 대상 (해녀)
    public float camera_speed, half_height, clamped_1, clamped_2, clamped_3; 
    public Vector3 target_position, min_bound1, max_bound1, min_bound2, max_bound2, min_bound3, max_bound3; 
    public BoxCollider2D bound1, bound2, bound3; 
    public Camera cam;
    public int level;

    void Start()
    {
        level = Haenyeo.level; 

        camera_speed = 0.3f;
        half_height = cam.orthographicSize; //카메라의 반높이
        min_bound1 = bound1.bounds.min;
        max_bound1 = bound1.bounds.max;
        min_bound2 = bound2.bounds.min;
        max_bound2 = bound2.bounds.max;
        min_bound3 = bound3.bounds.min;
        max_bound3 = bound3.bounds.max;
    }

    void Update()
    {
        if (target.gameObject != null)
        {
            target_position.Set(this.transform.position.x, target.transform.position.y, this.transform.position.z);
            this.transform.position = Vector3.Lerp(this.transform.position, target_position, camera_speed); //자신의 위치에서 대상의 위치까지 해당 속도로 이동

            //레벨 별로 카메라 이동 범위 제한
            if (level == 1)
            {
                clamped_1 = Mathf.Clamp(this.transform.position.y, min_bound1.y + half_height, max_bound1.y - half_height);
                this.transform.position = new Vector3(this.transform.position.x, clamped_1, this.transform.position.z);
            }
            if (level == 2)
            {
                clamped_2 = Mathf.Clamp(this.transform.position.y, min_bound2.y + half_height, max_bound2.y - half_height);
                this.transform.position = new Vector3(this.transform.position.x, clamped_2, this.transform.position.z);
            }
            if (level == 3)
            {
                clamped_3 = Mathf.Clamp(this.transform.position.y, min_bound3.y + half_height, max_bound3.y - half_height);
                this.transform.position = new Vector3(this.transform.position.x, clamped_3, this.transform.position.z);
            }
        }
    }
}
