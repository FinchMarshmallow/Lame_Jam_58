using System;
using UnityEngine;
using UnityEngine.UI;

public class ShipTestInput : MonoBehaviour
{
	[SerializeField] private ShipInputSlider forward, left, up;
	[SerializeField] private GameObject offObj, onObj;

	private ShipTestMove _move;

	private void Awake()
	{
		TryGetComponent(out _move);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			forward.ResetValue();
			left.ResetValue();
			up.ResetValue();
			_move.isFullForward = true;
			offObj.SetActive(false);
			onObj.SetActive(true);
		}

		if (Input.GetKeyUp(KeyCode.Space))
		{
			_move.isFullForward = false;
			offObj.SetActive(true);
			onObj.SetActive(false);
		}



		InputRot();
		InputLinear();
	}

	private void InputRot()
	{
		Vector2 input = new();

		if (Input.GetKey(KeyCode.W)) input.y++;
		if (Input.GetKey(KeyCode.S)) input.y--;
		if (Input.GetKey(KeyCode.A)) input.x--;
		if (Input.GetKey(KeyCode.D)) input.x++;

		_move.InputRot = input;
	}

	private void InputLinear()
	{
		Vector3 input = new();

		input.x = left.Value;
		input.y = up.Value;
		input.z = forward.Value;

		_move.inputLinear = input;
	}
}
