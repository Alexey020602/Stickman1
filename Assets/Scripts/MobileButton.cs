using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;



public class MobileButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler, ISubmitHandler
{
	/// The different possible states for the button : 
	/// Off - выключено, ButtonDown -кнопка нажата в первый раз, ButtonPressed -кнопка зажата, ButtonUp кнопка была опущена, Disabled выключена
	/// ButtonDown and ButtonUp выполняются только 1 раз
	public enum ButtonStates { Off, ButtonDown, ButtonPressed, ButtonUp, Disabled }
	[Header("Button")]
	public UnityEvent ButtonPressedFirstTime;
	public UnityEvent ButtonReleased;
	public UnityEvent ButtonPressed;



	[Header("Animation")]
	public string PressedFirstTime = "FirstTime";
	public string Released = "Released";

	[Header("Mouse Mode")]
	/// If you set this to true, you'll need to actually press the button for it to be triggered, otherwise a simple hover will trigger it (better for touch input).
	public bool MouseMode = false;


	/// the current state of the button (off, down, pressed or up)
	public ButtonStates CurrentState { get; protected set; }

	protected Animator _animator;
	protected Selectable _selectable;


	
	protected virtual void Awake()
	{

		_selectable = GetComponent<Selectable>();
		_animator = GetComponent<Animator>();

		CurrentState = ButtonStates.Off;
	}


	/// <summary>
	/// Every frame, if the touch zone is pressed, we trigger the OnPointerPressed method, to detect continuous press
	/// </summary>
	protected virtual void Update()
	{
		switch (CurrentState)
		{
			case ButtonStates.Off:
				if (_selectable != null)
				{
					_selectable.interactable = true;
				}
				break;

			case ButtonStates.Disabled:
				
				if (_selectable != null)
				{
					_selectable.interactable = false;
				}
				break;


			case ButtonStates.ButtonPressed:
				OnPointerPressed();
				break;

		}
	}

	/// <summary>
	/// At the end of every frame, we change our button's state if needed
	/// </summary>
	protected virtual void LateUpdate()
	{
		if (CurrentState == ButtonStates.ButtonUp)
		{
			CurrentState = ButtonStates.Off;
		}
		if (CurrentState == ButtonStates.ButtonDown)
		{
			CurrentState = ButtonStates.ButtonPressed;
		}
	}

	/// <summary>
	/// Triggers the bound pointer down action
	/// </summary>
	public virtual void OnPointerDown(PointerEventData data)
	{

		if (CurrentState != ButtonStates.Off)
		{
			return;
		}
		CurrentState = ButtonStates.ButtonDown;

		if (_animator)
		{
			_animator.SetTrigger(PressedFirstTime);
		}

		ButtonPressedFirstTime.Invoke();
	}

	protected virtual void InvokePressedFirstTime()
	{
		if (ButtonPressedFirstTime != null)
		{
			if (_animator)
			{
				_animator.SetTrigger(PressedFirstTime);
			}
			ButtonPressedFirstTime.Invoke();
		}
	}

	/// <summary>
	/// Triggers the bound pointer up action
	/// </summary>
	public virtual void OnPointerUp(PointerEventData data)
	{
		if (CurrentState != ButtonStates.ButtonPressed && CurrentState != ButtonStates.ButtonDown)
		{
			return;
		}

		CurrentState = ButtonStates.ButtonUp;

		if (_animator)
		{
			_animator.SetTrigger(Released);
		}

		ButtonReleased.Invoke();
	}

	protected virtual void InvokeReleased()
	{
		if (ButtonReleased != null)
		{
			if (_animator)
			{
				_animator.SetTrigger(Released);
			}
			ButtonReleased.Invoke();
		}
	}

	/// <summary>
	/// Triggers the bound pointer pressed action
	/// </summary>
	public virtual void OnPointerPressed()
	{
		CurrentState = ButtonStates.ButtonPressed;
		if (ButtonPressed != null)
		{
			ButtonPressed.Invoke();
		}
	}


	/// <summary>
	/// Triggers the bound pointer enter action when touch enters zone
	/// </summary>
	public virtual void OnPointerEnter(PointerEventData data)
	{
		if (!MouseMode)
		{
			OnPointerDown(data);
		}
	}

	/// <summary>
	/// Triggers the bound pointer exit action when touch is out of zone
	/// </summary>
	public virtual void OnPointerExit(PointerEventData data)
	{
		if (!MouseMode)
		{
			OnPointerUp(data);
		}
	}
	/// <summary>
	/// OnEnable, we reset our button state
	/// </summary>
	protected virtual void OnEnable()
	{
		CurrentState = ButtonStates.Off;
	}





	public virtual void OnSubmit(BaseEventData eventData)
	{
		Debug.Log("lol");
		ButtonPressedFirstTime.Invoke();
		ButtonReleased.Invoke();
	}
}
