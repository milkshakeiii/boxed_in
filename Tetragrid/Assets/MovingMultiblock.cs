using UnityEngine;
using System.Collections;

public class MovingMultiblock : MonoBehaviour {

	public float secondsPerMove = 1f;
	public float fastForwardFactor = 4f;

	private float lastMove = 0f;

	private Vector2 firstMove = new Vector2(-2, -2);
	private Vector2 position;
	private Vector2[] blocks = new Vector2[0];
	private bool active = true;

	void Start () 
	{
		position = Board.CurrentBoard ().GetCenterPoint ();
		BuildBlocks ();
		DrawBlocks (true);
	}

	private void BuildBlocks()
	{
		int blockSize = TetragridInfo.CurrentInfo ().CurrentBlockSize ();
		blocks = new Vector2[blockSize];
		for (int i = 0; i < blockSize-1; i++)
		{
			int randomDirection = UnityEngine.Random.Range(0, 4);
			Vector2[] randomDirections = new Vector2[4]{new Vector2(1, 0),
														new Vector2(-1, 0),
														new Vector2(0, 1),
														new Vector2(0, -1)};
			blocks[i+1] = blocks[i] + randomDirections[randomDirection];
		}
	}

	private void DrawBlocks(bool onOrOff)
	{
		foreach(Vector2 blockPosition in blocks)
		{
			Board.CurrentBoard().SetBoxState((int)(position.x + blockPosition.x),
			                                 (int)(position.y + blockPosition.y),
			                                 onOrOff);
		}
	}

	// Update is called once per frame
	void Update () 
	{
		float thisMoveSpeed;
		if (TetragridInput.FastForward())
		{
			thisMoveSpeed = secondsPerMove / fastForwardFactor;
		}
		else
		{
			thisMoveSpeed = secondsPerMove;
		}

		if (!FirstMoveTaken())
		{
			thisMoveSpeed += 1f;
		}

		if (active && Time.timeSinceLevelLoad > lastMove + thisMoveSpeed)
		{
			active = TryMove();
			if (!active)
				TetragridInfo.CurrentInfo().UpdateFilledCells();
			lastMove = Time.timeSinceLevelLoad;
		}
	}

	private bool TryMove()
	{
		Vector2 actualMovement = NegotiatedMovement ();

		foreach(Vector2 block in blocks)
		{
			int targetX = (int)position.x + (int)block.x + (int)actualMovement.x;
			int targetY = (int)position.y + (int)block.y + (int)actualMovement.y;
			if (Board.CurrentBoard().GetBoxState(targetX, targetY) ||
			    !Board.CurrentBoard().GetBoxExists(targetX, targetY))
			{
				DrawBlocks(true);
				return false;
			}
		}

		if (FirstMoveTaken())
			position = position + actualMovement;
		DrawBlocks(true);
		return true;
	}

	private Vector2 NegotiatedMovement()
	{
		Vector2 desiredMovement = TetragridInput.CurrentMovement ();
		
		DrawBlocks(false);
		
		if (desiredMovement != Vector2.zero && !FirstMoveTaken())
			firstMove = desiredMovement;
		
		Vector2 actualMovement = firstMove;
		
		if (firstMove.x == 0 || firstMove.y == 0)
		{
			if ((int)firstMove.x == 0)
				actualMovement.x = desiredMovement.x;
			if ((int)firstMove.y == 0)
				actualMovement.y = desiredMovement.y;
		}
		else
		{
			actualMovement = desiredMovement;
			if (desiredMovement.x == -firstMove.x)
			{
				actualMovement.x = 0;
				actualMovement.y = firstMove.y;
			}
			if (desiredMovement.y == -firstMove.y)
			{
				actualMovement.y = 0;
				actualMovement.x = firstMove.x;
			}
			if (desiredMovement.x == -firstMove.x && desiredMovement.y == -firstMove.y)
				actualMovement = firstMove;
			if (desiredMovement == Vector2.zero)
				actualMovement = firstMove;
		}

		return actualMovement;
	}

	private bool FirstMoveTaken()
	{
		return firstMove != new Vector2 (-2, -2);
	}

	public bool IsActive()
	{
		return active;
	}
}
