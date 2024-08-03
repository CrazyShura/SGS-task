using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Sword : MonoBehaviour
{
	#region Fields
	Transform idleTransform, readyTransform, endOfStrikeTransform;
	float timeToIdle, timeToReady, timeToStrike;

	Collider2D hitDetection;
	UnityEvent<GameObject> targetHit = new UnityEvent<GameObject>();

	SwordState currentState;
	UnityEvent<SwordState> swordStateChanged = new UnityEvent<SwordState>();
	bool inTransition = false;
	float timer;
	#endregion

	#region Properties
	public UnityEvent<GameObject> TargetHit { get => targetHit; }
	public UnityEvent<SwordState> SwordStateChanged { get => swordStateChanged; }
	#endregion

	#region Methods
	private void Awake()
	{
		currentState = SwordState.Idle;
		hitDetection = GetComponent<BoxCollider2D>();
		hitDetection.enabled = false;
	}
	public void SetUp(Transform idleTransform, float timeToIdle, Transform readyTransform, float timeToReady, Transform endOfStrikeTransform, float timeToStrike)
	{
		this.idleTransform = idleTransform;
		this.readyTransform = readyTransform;
		this.endOfStrikeTransform = endOfStrikeTransform;
		this.timeToIdle = timeToIdle;
		this.timeToReady = timeToReady;
		this.timeToStrike = timeToStrike;

		transform.position = idleTransform.position;
		transform.rotation = idleTransform.rotation;
	}
	private void Update()
	{
		if (inTransition)
		{
			timer -= Time.deltaTime;
			if (timer <= 0)
			{
				inTransition = false;
				swordStateChanged.Invoke(currentState);
			}
		}
	}
	public void GetReady()
	{
		if (currentState == SwordState.Idle && !inTransition)
		{
			inTransition = true;
			timer = timeToReady;
			transform.DOLocalMove(readyTransform.localPosition, timeToReady);
			transform.DOLocalRotateQuaternion(readyTransform.localRotation, timeToReady);
			currentState = SwordState.Ready;
			hitDetection.enabled = false;
		}
	}
	public void Strike()
	{
		if (currentState == SwordState.Ready && !inTransition)
		{
			inTransition = true;
			timer = timeToStrike;
			transform.DOLocalMove(endOfStrikeTransform.localPosition, timeToStrike);
			transform.DOLocalRotateQuaternion(endOfStrikeTransform.localRotation, timeToStrike);
			currentState = SwordState.EndOfStrike;
			hitDetection.enabled = true;
		}
	}
	public void BackToIdle()
	{
		if (!inTransition)
		{
			inTransition = true;
			timer = timeToIdle;
			transform.DOLocalMove(idleTransform.localPosition, timeToIdle);
			transform.DOLocalRotateQuaternion(idleTransform.localRotation, timeToIdle);
			currentState = SwordState.Idle;
			hitDetection.enabled = false;
		}
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		int _layer = collision.gameObject.layer;
		if (_layer == 8 || _layer == 7)
		{
			TargetHit.Invoke(collision.gameObject);
			switch (_layer)
			{
				case 7:
					collision.GetComponent<PlayerController>().TakeDamage();
					break;
				case 8:
					collision.GetComponent<Enemy>().TakeDamage();
					break;
				default:
					break;
			}
		}
	}
	#endregion
}
