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
        // �J�����̌���(�p�x[deg])�擾
		var cameraAngleY = m_camera.transform.eulerAngles.y;

		// ������͂Ɖ����������x����A���ݑ��x���v�Z
		var moveVelocity = new Vector3(
			m_inputMove.x * m_speed,
			m_verticalVelocity,
			m_inputMove.y * m_speed
		);

		// �J�����̊p�x���������ړ��ʂ���]
		moveVelocity = Quaternion.Euler(0, cameraAngleY, 0) * moveVelocity;

		// ���t���[���̈ړ��ʂ��ړ����x����v�Z
		var moveDelta = moveVelocity * Time.deltaTime;

		//CharactorController�Ɉړ��ʂ��w�肵�A�I�u�W�F�N�g�𓮂���
		m_characterController.Move(moveDelta);

		if(m_inputMove != Vector2.zero)
		{
			// ������͂���Y������̖ڕW�p�x[deg]���v�Z
			var targetAngleY = -Mathf.Atan2(m_inputMove.y, m_inputMove.x) * Mathf.Rad2Deg + 90;

			// �J�����̊p�x�������U������p�x��␳
			targetAngleY += cameraAngleY;

			// �C�[�W���O���Ȃ��玟�̉�]���x[deg]���v�Z
			var angleY = Mathf.SmoothDampAngle(
				m_transform.eulerAngles.y,
				targetAngleY,
				ref m_turnVelocity,
				0.1f
			);

			// �I�u�W�F�N�g�̉�]���X�V
			m_transform.rotation = Quaternion.Euler(0, angleY, 0);
		}
    }
}
