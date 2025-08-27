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

	private Rigidbody rb;
	private Transform m_transform;
	private PlayerInput m_playerInput;
	private Vector2 m_inputMove;
	private CharacterController m_characterController;
	private float m_verticalVelocity;
	private float m_turnVelocity;


	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		m_transform = GetComponent<Transform>();
		m_playerInput = GetComponent<PlayerInput>();
		m_characterController = GetComponent<CharacterController>();
	}
	// Start is called before the first frame update
	void Start()
    {
      

	}

	public void OnEnable()
	{
		m_playerInput.actions["Move"].performed += OnMove;
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
	}

	public void OnJump(InputAction.CallbackContext context)
	{

	}

	// Update is called once per frame
	void FixedUpdate()
    {
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
			// 操作入力からY軸周りの目標角度[deg]を計算
			var targetAngleY = -Mathf.Atan2(m_inputMove.y, m_inputMove.x) * Mathf.Rad2Deg + 90;

			// カメラの角度分だけ振り向く角度を補正
			targetAngleY += cameraAngleY;

			// イージングしながら次の回転速度[deg]を計算
			var angleY = Mathf.SmoothDampAngle(
				m_transform.eulerAngles.y,
				targetAngleY,
				ref m_turnVelocity,
				0.1f
			);

			// オブジェクトの回転を更新
			m_transform.rotation = Quaternion.Euler(0, angleY, 0);
		}
    }
}
