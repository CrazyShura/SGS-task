using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
	#region Fields
	[SerializeField]
	Enemy target;
	[SerializeField]
	HealthIndicator healthIndicatorPrefab;
	[SerializeField]
	Color onColor;
	[SerializeField]
	Color offColor;

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
		}
		target.HealthChanged.AddListener(OnHealthChange);
	}
	void OnHealthChange(int health)
	{
		for (int _i = 0; _i < target.Health; _i++)
		{
			if (health > 0)
			{
				healthIndicators[_i].TurnOn();
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
