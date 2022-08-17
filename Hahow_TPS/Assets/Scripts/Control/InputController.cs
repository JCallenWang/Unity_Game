using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//管理所有輸入的介面
public class InputController : MonoBehaviour
{
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        ChangeCursorState();
    }

    public Vector3 GetMoveInput()
    {
        if (CheckCursorState())
        {
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            //限制斜對角向量長度不超過1
            move = Vector3.ClampMagnitude(move, 1f);
            return move;
        }
        return Vector3.zero;
    }

    //檢查是否按下左Shift
    public bool GetSprintInput()
    {
        if (CheckCursorState())
        {
            return Input.GetKey(KeyCode.LeftShift);
        }
        return false;
    }
    //檢查是否按下左Ctrl
    public bool GetCrouchInput()
    {
        if (CheckCursorState())
        {
            return Input.GetKey(KeyCode.LeftControl);
        }
        return false;
    }
    //檢查是否按下Space
    public bool GetJumpInput()
    {
        if (CheckCursorState())
        {
            return Input.GetKeyDown(KeyCode.Space);
        }
        return false;
    }

    //回傳滑鼠水平
    public float GetMouseXAxis()
    {
        if (CheckCursorState())
        {
            return Input.GetAxis("Mouse X");
        }
        return 0;
    }
    //回傳滑鼠垂直
    public float GetMouseYAxis()
    {
        if (CheckCursorState())
        {
            return Input.GetAxis("Mouse Y");
        }
        return 0;
    }
    //回傳滑鼠滾輪
    public float GetMouseScrollWheel()
    {
        if (CheckCursorState())
        {
            return Input.GetAxis("Mouse ScrollWheel");
        }
        return 0;
    }

    //檢查滑鼠是否鎖定
    private bool CheckCursorState()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
            return true;
        return false;       
    }
    //ESC改變滑鼠鎖定狀態
    private void ChangeCursorState()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.None)
                Cursor.lockState = CursorLockMode.Locked;
            else
                Cursor.lockState = CursorLockMode.None;
        }
    }
}
