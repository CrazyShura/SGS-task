using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
	#region Fields
	[Header("Toggle this to ignore settings")]
	[SerializeField]
	bool IgnoreSettings = false;
	[Header("Movement")]
	[SerializeField, Range(5, 500)]
	float jumpStrength = 10f;
	[SerializeField, Range(5, 500)]
	float wallJumpStrength = 10f;
	[SerializeField, Range(1, 25f)]
	float moveSpeed = 10f;
	[SerializeField, Range(0, 1f)]
	float moveSmoothenes = .1f;
	[SerializeField, Range(1, 10f)]
	float maxSpeedOnWallSlide = 3f;
	[SerializeField]
	Transform groundCheckPosition;
	[SerializeField]
	Transform wallChechPositionLeft;
	[SerializeField]
	Transform wallChechPositionRight;
	[SerializeField]
	LayerMask groundMask;
	[SerializeField, Range(1f, 3f)]
	float playerGravityWhileFalling = 3f;
	[Space(5)]

	[Header("Combat")]
	[SerializeField]
	Sword playerSword;
	[SerializeField]
	Transform swordIdleTransform;
	[SerializeField, Min(.1f)]
	float swordTimeToIdle;
	[SerializeField]
	Transform swordReadyTransform;
	[SerializeField, Min(.1f)]
	float swordTimeToReady;
	[SerializeField]
	Transform swordEndOfStrikeTransform;
	[SerializeField, Min(.1f)]
	float swordTimeToStrike;
	[SerializeField, Min(1)]
	int health = 5;
	[SerializeField, Min(1f)]
	float bloodbornStateDuration = 3f;

	int currentHealth;
	bool bloodbornState = false;
	float bloodbornStateTimer;
	UnityEvent<int> healthChanged = new UnityEvent<int>();

	InputActions inputActions;

	Rigidbody2D rgbd;
	Vector3 temp = Vector3.zero;
	float moveValue;
	bool isGrounded, isTouchingWallLeft, isTouchingWallRight;
	#endregion

	#region Properties
	public int Health { get => health; }
	public bool BloodbornState { get => bloodbornState; }
	public UnityEvent<int> HealthChanged { get => healthChanged; }
	#endregion

	#region Methods
	private void Awake()
	{
		if (!IgnoreSettings)
		{
			ReadSettings();
		}
		rgbd = GetComponent<Rigidbody2D>();

		inputActions = new InputActions();
		inputActions.PlayerMovement.Jump.performed += Jump;
		inputActions.PlayerMovement.Attack.performed += Attack;
		inputActions.Enable();

		playerSword.SetUp(swordIdleTransform, swordTimeToIdle, swordReadyTransform, swordTimeToReady, swordEndOfStrikeTransform, swordTimeToStrike);
		playerSword.SwordStateChanged.AddListener(OnSwordStateChange);
		playerSword.TargetHit.AddListener(OnTargetHit);
		currentHealth = health;
	}
	private void Update()
	{
		if (Physics2D.OverlapCircle(groundCheckPosition.position, .1f, groundMask))
		{
			isGrounded = true;
		}
		else
		{
			isGrounded = false;
		}
		if (Physics2D.OverlapCircle(wallChechPositionLeft.position, .1f, groundMask))
		{
			isTouchingWallLeft = true;
		}
		else
		{
			isTouchingWallLeft = false;
		}
		if (Physics2D.OverlapCircle(wallChechPositionRight.position, .1f, groundMask))
		{
			isTouchingWallRight = true;
		}
		else
		{
			isTouchingWallRight = false;
		}

		if (!isGrounded && !wallChechPositionLeft && !wallChechPositionRight && rgbd.velocity.y < 0)
		{
			rgbd.gravityScale = playerGravityWhileFalling;
		}
		else
		{
			rgbd.gravityScale = 1;
		}

		moveValue = inputActions.PlayerMovement.Move.ReadValue<float>();

		if (bloodbornState)
		{
			bloodbornStateTimer -= Time.deltaTime;
			if (bloodbornStateTimer <= 0)
			{
				bloodbornState = false;
				currentHealth--;
				healthChanged.Invoke(currentHealth);
				if (currentHealth <= 0)
				{
					Destroy(gameObject);
					return;
				}
				playerSword.BackToIdle();
			}
		}
	}
	private void FixedUpdate()
	{
		Vector3 _targetVelocity = new Vector2(moveValue * moveSpeed, rgbd.velocity.y);
		if (isTouchingWallLeft || isTouchingWallRight)
		{
			_targetVelocity.y *= .8f;
			_targetVelocity.y = Mathf.Clamp(_targetVelocity.y, -maxSpeedOnWallSlide, maxSpeedOnWallSlide);
			if (isTouchingWallLeft && _targetVelocity.x < 0)
			{
				_targetVelocity.x = 0;
			}
			if (isTouchingWallRight && _targetVelocity.x > 0)
			{
				_targetVelocity.x = 0;
			}
		}
		rgbd.velocity = Vector3.SmoothDamp(rgbd.velocity, _targetVelocity, ref temp, moveSmoothenes);
	}
	private void Attack(InputAction.CallbackContext obj)
	{
		playerSword.Strike();
	}
	void Jump(InputAction.CallbackContext context)
	{
		if (isGrounded)
		{
			rgbd.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
		}
		else if (isTouchingWallLeft)
		{
			rgbd.AddForce((Vector2.up / 3 + Vector2.right).normalized * wallJumpStrength, ForceMode2D.Impulse);
		}
		else if (isTouchingWallRight)
		{
			rgbd.AddForce((Vector2.up / 3 + Vector2.left).normalized * wallJumpStrength, ForceMode2D.Impulse);
		}
	}
	void OnTargetHit(GameObject unused)
	{
		if (bloodbornState)
		{
			bloodbornState = false;
			healthChanged.Invoke(currentHealth);
		}
	}
	void OnSwordStateChange(SwordState swordState)
	{
		switch (swordState)
		{
			case SwordState.Idle:
				if (bloodbornState)
				{
					playerSword.GetReady();
				}
				break;
			case SwordState.Ready:
				break;
			case SwordState.EndOfStrike:
				playerSword.BackToIdle();
				break;
		}
	}
	public void TakeDamage()
	{
		if (bloodbornState)
		{
			bloodbornState = false;
			currentHealth -= 2;
			healthChanged.Invoke(currentHealth);
			playerSword.BackToIdle();
		}
		else
		{
			bloodbornState = true;
			bloodbornStateTimer = bloodbornStateDuration;
			healthChanged.Invoke(currentHealth);
			playerSword.GetReady();
		}
		if (currentHealth <= 0)
		{
			Destroy(gameObject);
		}
	}
	void ReadSettings()
	{
		Settings _settingsToRead = (Settings)Resources.Load("DefaultSettings");
		if (_settingsToRead == null)
		{
			Debug.Log("Could not load settings");
			throw new ArgumentNullException();
		}
		jumpStrength = _settingsToRead.JumpStrength;
		wallJumpStrength = _settingsToRead.WallJumpStrength;
		moveSpeed = _settingsToRead.MoveSpeed;
		moveSmoothenes = _settingsToRead.MoveSmoothenes;
		maxSpeedOnWallSlide = _settingsToRead.MaxSpeedOnWallSlide;
		playerGravityWhileFalling = _settingsToRead.PlayerGravityWhileFalling;
		swordTimeToIdle = _settingsToRead.SwordTimeToIdle;
		swordTimeToReady = _settingsToRead.SwordTimeToReady;
		swordTimeToStrike = _settingsToRead.SwordTimeToStrike;
		bloodbornStateDuration = _settingsToRead.BloodbornStateDuration;
		health = _settingsToRead.Health;
	}
	#endregion
}
