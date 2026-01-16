using System;
using UnityEngine;
using UnityEngine.UI;

public class ShipTestInput : MonoBehaviour
{
	[SerializeField] private Slider forward, left, up;

	private ShipTestMove _move;

	private void Awake()
	{
		TryGetComponent(out _move);
	}

	private void Update()
	{
		InputRot();
		InputLinear();
	}

	private void InputRot()
	{
		Vector2 input = new();

		if (Input.GetKey(KeyCode.W)) input.y++;
		if (Input.GetKey(KeyCode.S)) input.y--;
		if (Input.GetKey(KeyCode.A)) input.x++;
		if (Input.GetKey(KeyCode.D)) input.x--;

		_move.InputRot = input;
	}

	private void InputLinear()
	{
		float min = 0.2f;

		if (-min < forward.value && forward.value < min) forward.value = 0f;
		if (-min < left.value && left.value < min) left.value = 0f;
		if (-min < up.value && up.value < min) up.value = 0f;

		Vector3 input = new();

		input.x = left.value;
		input.y = up.value;
		input.z = forward.value;

		_move.inputLinear = input;
	}
}
