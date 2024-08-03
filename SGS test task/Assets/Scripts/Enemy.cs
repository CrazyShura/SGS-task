using System;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
	#region Fields
	[Header("Toggle this to ignore settings")]
	[SerializeField]
	bool IgnoreSettings = false;
	[SerializeField]
	Sword enemySword;
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
	int health = 3;
	[SerializeField, Min(1f)]
	float attackRate = 3f;

	UnityEvent<int> healthChanged = new UnityEvent<int>();

	int currentHealth;
	float timer;
	#endregion

	#region Properties
	public UnityEvent<int> HealthChanged { get => healthChanged;}
	public int Health { get => health;}
	#endregion

	#region Methods
	private void Awake()
	{
		if (!IgnoreSettings)
		{
			ReadSettings();
		}

		enemySword.SetUp(swordIdleTransform, swordTimeToIdle, swordReadyTransform, swordTimeToReady, swordEndOfStrikeTransform, swordTimeToStrike);
		timer = attackRate;
		enemySword.SwordStateChanged.AddListener(CycleAttack);
		currentHealth = health;
	}
	private void Update()
	{
		timer -= Time.deltaTime;
		if(timer <=  0)
		{
			enemySword.GetReady();
			timer = attackRate;
		}
	}
	void CycleAttack(SwordState swordState)
	{
		switch (swordState)
		{
			case SwordState.Idle:
				break;
			case SwordState.Ready:
				enemySword.Strike();
				break;
			case SwordState.EndOfStrike:
				enemySword.BackToIdle();
				break;
		}
	}
	public void TakeDamage()
	{
		currentHealth--;
		healthChanged.Invoke(currentHealth);
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
		swordTimeToIdle = _settingsToRead.EnemySwordTimeToIdle;
		swordTimeToReady = _settingsToRead.EnemySwordTimeToReady;
		swordTimeToStrike = _settingsToRead.EnemySwordTimeToStrike;
		health = _settingsToRead.EnemyHealth;
		attackRate  = _settingsToRead.EnemyAttackRate;
	}
		#endregion
	}
