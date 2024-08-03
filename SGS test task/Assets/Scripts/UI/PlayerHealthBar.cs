using UnityEngine;

public class PlayerHealthBar : MonoBehaviour
{
	#region Fields
	[SerializeField]
	PlayerController target;
	[SerializeField]
	HealthIndicator healthIndicatorPrefab;
	[SerializeField]
	Color onColor;
	[SerializeField]
	Color offColor;
	[SerializeField]
	float blinkTime;

	HealthIndicator[] healthIndicators;
	#endregion

	#region Properties
	#endregion

	#region Methods
	private void Start()
	{
		healthIndicators = new HealthIndicator[target.Health];
		for (int _i = 0; _i < target.Health; _i++)
		{
			healthIndicators[_i] = Instantiate(healthIndicatorPrefab, this.transform);
			healthIndicators[_i].OnColor = onColor;
			healthIndicators[_i].OffColor = offColor;
			healthIndicators[_i].BlinkTime = blinkTime;
		}
		target.HealthChanged.AddListener(OnHealthChange);
	}
	void OnHealthChange(int health)
	{
		for (int _i = 0; _i < target.Health; _i++)
		{
			if(healthIndicators[_i].IsBlinking)
			{
				healthIndicators[_i].Blink(); //HACK i dont have a better way to get the blinking off the indicator in my mind roght now
			}
			if (health > 0)
			{
				healthIndicators[_i].TurnOn();
				if (health == 1 && target.BloodbornState)
				{
					healthIndicators[_i].Blink();
				}
			}
			else
			{
				healthIndicators[_i].TurnOff();
			}
			health--;
		}
	}
	#endregion
}
