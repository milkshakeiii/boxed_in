using UnityEngine;
using System.Collections;

public class TetragridInput : MonoBehaviour {

	static public Vector2 CurrentMovement()
	{
		Vector2 newMovement = new Vector2 (0, 0);

		if (LeftKey ())
			newMovement = newMovement + new Vector2 (-1, 0);
		if (RightKey ())
			newMovement = newMovement + new Vector2 (1, 0);
		if (UpKey ())
			newMovement = newMovement + new Vector2 (0, 1);
		if (DownKey ())
			newMovement = newMovement + new Vector2 (0, -1);

		return newMovement;
	}

	static private bool LeftKey()
	{
		return UnityEngine.Input.GetKey(KeyCode.LeftArrow);
	}
	
	static private bool DownKey()
	{
		return UnityEngine.Input.GetKey(KeyCode.DownArrow);
	}
	
	static private bool RightKey()
	{
		return UnityEngine.Input.GetKey(KeyCode.RightArrow);
	}
	
	static private bool UpKey()
	{
		return UnityEngine.Input.GetKey(KeyCode.UpArrow);
	}

	static public bool FastForward()
	{
		return UnityEngine.Input.GetKey(KeyCode.S);
	}
}
