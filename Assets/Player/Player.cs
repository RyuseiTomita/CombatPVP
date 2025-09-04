using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

	[SerializeField]
	float m_speed;

	[SerializeField]
	Camera m_camera;

	 Animator m_animator;
	 Transform m_transform;
	 PlayerInput m_playerInput;
	 Vector2 m_inputMove;
	 CharacterController m_characterController;
	 float m_verticalVelocity;

	bool m_canMove;

	private void Awake()
	{
		m_transform = GetComponent<Transform>();
		m_animator = GetComponent<Animator>();
		m_playerInput = GetComponent<PlayerInput>();
		m_characterController = GetComponent<CharacterController>();
	}
	// Start is called before the first frame update
	void Start()
    {
		m_canMove = true;
	}

	public void OnEnable()
	{
		m_playerInput.actions["Move"].performed += OnMove;
		m_playerInput.actions["Attack"].performed += OnAttack;
		m_playerInput.actions["Move"].canceled += OnMoveCancel;
	}

	public void OnDisable()
	{
		m_playerInput.actions["Move"].performed -= OnMove;
		m_playerInput.actions["Move"].canceled -= OnMoveCancel;
	}

	public void OnMove(InputAction.CallbackContext context)
	{
		m_inputMove = context.ReadValue<Vector2>();
	}

	public void OnMoveCancel(InputAction.CallbackContext context)
	{
		m_inputMove = context.ReadValue<Vector2>();

		m_animator.SetBool("Run", false);
	}

	public void OnAttack(InputAction.CallbackContext context)
	{
		m_animator.SetTrigger("Attack");
		m_canMove = false;
	}

	public void OnJump(InputAction.CallbackContext context)
	{

	}

	public void ResetTrigger()
	{
		m_canMove = true;
		m_animator.ResetTrigger("Attack");
	}

	// Update is called once per frame
	void FixedUpdate()
    {
		if (!m_canMove) return;

        // カメラの向き(角度[deg])取得
		var cameraAngleY = m_camera.transform.eulerAngles.y;

		// 操作入力と鉛直方向速度から、現在速度を計算
		var moveVelocity = new Vector3(
			m_inputMove.x * m_speed,
			m_verticalVelocity,
			m_inputMove.y * m_speed
		);

		// カメラの角度部分だけ移動量を回転
		moveVelocity = Quaternion.Euler(0, cameraAngleY, 0) * moveVelocity;

		// 現フレームの移動量を移動速度から計算
		var moveDelta = moveVelocity * Time.deltaTime;

		//CharactorControllerに移動量を指定し、オブジェクトを動かす
		m_characterController.Move(moveDelta);

		if(m_inputMove != Vector2.zero)
		{
			m_animator.SetBool("Run", true);

			transform.rotation = Quaternion.Lerp(
				transform.rotation,
				Quaternion.LookRotation(Vector3.Scale(moveVelocity, new Vector3(1, 0, 1))),
				0.5f);
		}
    }
}
