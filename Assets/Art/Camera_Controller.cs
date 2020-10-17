using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera MainCamera;
    public float speed = 10f;
    public float maxView = 90;
    public float minView = 30;
    public float moveSpeed = 10f;
    public Vector3 maxPos;
    public Vector3 minPos;
    private Vector3 normalPos;
    void Start()
    {
        normalPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float offsetView = -Input.GetAxis("Mouse ScrollWheel") * speed;
        float tmpView = offsetView + MainCamera.fieldOfView;
        tmpView = Mathf.Clamp(tmpView, minView, maxView);
        MainCamera.fieldOfView = tmpView;
        if(Input.GetKeyDown(KeyCode.Space))
            transform.position = normalPos;
        if(moveSpeed!=0)
            CameraMove();
        else
        {
            float _mouseX = Input.GetAxis("Mouse X");
            float _mouseY = Input.GetAxis("Mouse Y");

            if(Input.GetKey(KeyCode.Mouse2))
                CameraMove(_mouseX, _mouseY);
        }
    }

    void CameraMove(float _mouseX,float _mouseY)
    {
        if (Input.GetMouseButton(2))
        {
            //相机位置的偏移量（Vector3类型，实现原理是：向量的加法）
            Vector3 moveDir = (_mouseX * -MainCamera.transform.right + _mouseY  * -MainCamera.transform.forward); 
			
			//限制y轴的偏移量
            moveDir.y = 0;
            MainCamera.transform.position += moveDir * 0.5f;
        }
        else
        {
            //鼠标恢复默认图标，置null即可
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
        float x = Mathf.Clamp(transform.position.x, minPos.x, maxPos.x);
        float y = Mathf.Clamp(transform.position.y, minPos.y, maxPos.y);
        float z = Mathf.Clamp(transform.position.z, minPos.z, maxPos.z);
        transform.position = new Vector3(x,y,z);
    }

    void CameraMove()
    {
        if (Input.GetKey(KeyCode.A)) //左移
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D)) //右移
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.W)) //前移
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.S)) //后移
        {
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.World);
        }
    }
}
