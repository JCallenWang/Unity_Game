using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//控制玩家行為
public class PlayerController : MonoBehaviour
{
    [Header("速度參數")]
    [SerializeField] float speed = 5;
    [Range(1, 5)] [SerializeField] float sprintSpeedModifier = 3;
    [Range(0, 1)] [SerializeField] float crouchSpeedModifier = 0.5f;
    [SerializeField] float rotateSpeed = 5;
    [Tooltip("動畫切換參數比例")] [Range(0.01f, 0.1f)] [SerializeField] float AnimationTransitionRatio = 0.05f;
    [Header("跳躍參數")]
    [SerializeField] float jumpSpeed = 15;
    [SerializeField] float gravityForce = 50;
    [SerializeField] float distanceToGround = 0.01f;

    //接收來自InputController的通報
    InputController main_Input;
    CharacterController controller;
    Animator animator;
    //下一幀移動的目標
    Vector3 targetMovement;
    //下一幀跳躍的方向
    Vector3 jumpDirection;
    //要移動的速度（動畫檢測）
    float lastFrameSpeed;

    private void Awake()
    {
        main_Input = GameManagerSingleton.Instance.InputController;
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        MoveBehaviour();
        JumpBehaviour();
    }

    private void MoveBehaviour()
    {
        targetMovement = Vector3.zero;
        //讓角色隨著相機的視角方向移動
        targetMovement += main_Input.GetMoveInput().z * GetCameraForward();
        targetMovement += main_Input.GetMoveInput().x * GetCameraRight();
        //避免對角線超過1
        targetMovement = Vector3.ClampMagnitude(targetMovement, 1);

        //切換動畫的速度參數
        float nextFrameSpeed = 0;

        //移動速度判斷條件
        if (targetMovement == Vector3.zero)
        {
            nextFrameSpeed = 0f;
            lastFrameSpeed = Mathf.Lerp(lastFrameSpeed, 0, AnimationTransitionRatio);

        }
        else if (main_Input.GetSprintInput())
        {
            nextFrameSpeed = 1;

            targetMovement *= sprintSpeedModifier;
            SmoothRotation(targetMovement);
        }
        else
        {
            nextFrameSpeed = 0.5f;

            SmoothRotation(targetMovement);
        }

        //如果按下蹲走
        if (main_Input.GetCrouchInput())
        {
            targetMovement *= crouchSpeedModifier;
            animator.SetBool("IsCrouch", true);
        }
        else
        {
            animator.SetBool("IsCrouch", false);
        }

        //避免動畫切換太快
        if (lastFrameSpeed != nextFrameSpeed)
            lastFrameSpeed = Mathf.Lerp(lastFrameSpeed, nextFrameSpeed, AnimationTransitionRatio);
        
        animator.SetFloat("WalkSpeed", lastFrameSpeed);
        controller.Move(targetMovement * speed * Time.deltaTime);
    }

    private void JumpBehaviour()
    {
        //如果按下跳躍就施加向上的力
        if (main_Input.GetJumpInput() && IsGrounded())
        {
            animator.SetTrigger("IsJump");
            jumpDirection = Vector3.zero;
            jumpDirection += Vector3.up * jumpSpeed;
        }

        jumpDirection.y -= gravityForce * Time.deltaTime;
        //檢測jumpDirection不會超過50
        jumpDirection.y = Math.Max(jumpDirection.y, -gravityForce);


        controller.Move(jumpDirection * Time.deltaTime);
    }

    //檢查是否在地上
    private bool IsGrounded()
    {
        //transform.up 物件中心的下方，會隨物件旋轉而改變中心軸
        //Vector3.up 下方的向量方向，始終固定
        return Physics.Raycast(transform.position, -Vector3.up, distanceToGround);
    }
    //平滑的轉向目標
    private void SmoothRotation(Vector3 targetMove)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetMove, Vector3.up), rotateSpeed * Time.deltaTime);
    }


    //取得相機的水平方向
    private Vector3 GetCameraRight()
    {
        Vector3 cameraRight = Camera.main.transform.right;
        cameraRight.y = 0;
        cameraRight.Normalize();
        return cameraRight;
    }
    //取得相機的前方方向
    private Vector3 GetCameraForward()
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        //避免相機俯角使前方向量包含Y軸(transform.up)
        cameraForward.y = 0;
        //將相機前方向量最大值設為1
        cameraForward.Normalize();
        return cameraForward;
    }
}
