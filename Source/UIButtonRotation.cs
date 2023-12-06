using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Rotation")]
public class UIButtonRotation : MonoBehaviour
{
	public float duration = 0.2f;

	public Vector3 hover = Vector3.zero;

	private bool mHighlighted;

	private Quaternion mRot;

	private bool mStarted;

	public Vector3 pressed = Vector3.zero;

	public Transform tweenTarget;

	private void OnDisable()
	{
		if (mStarted && tweenTarget != null)
		{
			TweenRotation component = tweenTarget.GetComponent<TweenRotation>();
			if (component != null)
			{
				component.rotation = mRot;
				component.enabled = false;
			}
		}
	}

	private void OnEnable()
	{
		if (mStarted && mHighlighted)
		{
			OnHover(UICamera.IsHighlighted(base.gameObject));
		}
	}

	private void OnHover(bool isOver)
	{
		if (base.enabled)
		{
			if (!mStarted)
			{
				Start();
			}
			TweenRotation.Begin(tweenTarget.gameObject, duration, (!isOver) ? mRot : (mRot * Quaternion.Euler(hover))).method = UITweener.Method.EaseInOut;
			mHighlighted = isOver;
		}
	}

	private void OnPress(bool isPressed)
	{
		if (base.enabled)
		{
			if (!mStarted)
			{
				Start();
			}
			TweenRotation.Begin(tweenTarget.gameObject, duration, isPressed ? (mRot * Quaternion.Euler(pressed)) : ((!UICamera.IsHighlighted(base.gameObject)) ? mRot : (mRot * Quaternion.Euler(hover)))).method = UITweener.Method.EaseInOut;
		}
	}

	private void Start()
	{
		if (!mStarted)
		{
			mStarted = true;
			if (tweenTarget == null)
			{
				tweenTarget = base.transform;
			}
			mRot = tweenTarget.localRotation;
		}
	}
}
