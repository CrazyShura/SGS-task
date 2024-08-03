using UnityEngine;

[CreateAssetMenu(fileName = "New Settings", menuName = "Create settings")]
public class Settings : ScriptableObject
{
	#region Fields
	[Header("Player")]
	[Space(3)]
	[Header("    Movement")]
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
	[SerializeField, Range(1f, 3f)]
	float playerGravityWhileFalling = 3f;
	[Space(3)]
	[Header("    Combat")]
	[SerializeField, Min(.1f)]
	float swordTimeToIdle;
	[SerializeField, Min(.1f)]
	float swordTimeToReady;
	[SerializeField, Min(.1f)]
	float swordTimeToStrike;
	[SerializeField, Min(1f)]
	float bloodbornStateDuration = 3f;
	[SerializeField, Min(1)]
	int health = 5;
	[Space(10)]

	[Header("Enemy")]
	[Space(3)]
	[SerializeField, Min(.1f)]
	float enemySwordTimeToIdle;
	[SerializeField, Min(.1f)]
	float enemySwordTimeToReady;
	[SerializeField, Min(.1f)]
	float enemySwordTimeToStrike;
	[SerializeField, Min(1)]
	int enemyHealth = 3;
	[SerializeField, Min(1f)]
	float enemyAttackRate = 3f;
	#endregion

	#region Properties
	public float JumpStrength { get => jumpStrength;}
	public float WallJumpStrength { get => wallJumpStrength;}
	public float MoveSpeed { get => moveSpeed;}
	public float MoveSmoothenes { get => moveSmoothenes;}
	public float MaxSpeedOnWallSlide { get => maxSpeedOnWallSlide;}
	public float PlayerGravityWhileFalling { get => playerGravityWhileFalling; }
	public float SwordTimeToIdle { get => swordTimeToIdle; }
	public float SwordTimeToReady { get => swordTimeToReady; }
	public float SwordTimeToStrike { get => swordTimeToStrike; }
	public float BloodbornStateDuration { get => bloodbornStateDuration; }
	public int Health { get => health; }
	public float EnemySwordTimeToIdle { get => enemySwordTimeToIdle; }
	public float EnemySwordTimeToReady { get => enemySwordTimeToReady; }
	public float EnemySwordTimeToStrike { get => enemySwordTimeToStrike; }
	public int EnemyHealth { get => enemyHealth; }
	public float EnemyAttackRate { get => enemyAttackRate; }
	#endregion
}
