using UnityEngine;
using UnityEngine.UI;

public class HealthIndicator : MonoBehaviour
{
	#region Fields
	Color onColor, offColor, curretntColor, targetColor;
	Image image;
	float blinkTime = .5f, blinkTimer;
	bool isOn = true;
	bool isBlinking;
	#endregion

	#region Properties
	public Color OnColor
	{
		get => onColor;
		set
		{
			onColor = value;
			if(isOn)
			{
				image.color = onColor;
			}
		}
	}
	public Color OffColor { get => offColor; set => offColor = value; }
	public float BlinkTime { get => blinkTime; set => blinkTime = value; }
	public bool IsBlinking { get => isBlinking; }
	#endregion

	#region Methods
	private void Awake()
	{
		image = GetComponent<Image>();
	}
	public void TurnOff()
	{
		image.color = offColor;
		isOn = false;
	}
	public void TurnOn()
	{
		image.color = onColor;
		isOn = true;
	}
	public void Blink()
	{
		if (isBlinking)
		{
			isBlinking = false;
			if (isOn)
			{
				image.color = onColor;
			}
			else
			{
				image.color = offColor;
			}
		}
		else
		{
			isBlinking = true;
			blinkTimer = blinkTime;
			curretntColor = onColor;
			targetColor = offColor;
		}
	}
	private void Update()
	{
		if (isBlinking)
		{
			blinkTimer -= Time.deltaTime;
			image.color = Color.Lerp(curretntColor, targetColor, 1 - (blinkTimer / blinkTime));
			if (blinkTimer <= 0)
			{
				targetColor = curretntColor;
				curretntColor = image.color;
				blinkTimer = blinkTime;
			}
		}
	}
	#endregion
}
