using UnityEngine;
using System.Collections;

public class MovingMultiblockGenerator : MonoBehaviour {

	public GameObject movingMultiblock;

	private MovingMultiblock activeMultiblock;
	
	void Start () 
	{
		MakeABlock ();
	}

	void Update () 
	{
		if (!activeMultiblock.IsActive ())
		{
			if (!Board.CurrentBoard().StartingSpaceClear())
			{
				TetragridInfo.CurrentInfo().LevelOver();
				return;
			}
			MakeABlock();
		}
	}

	private void MakeABlock()
	{
		GameObject newMultiblock = Instantiate (movingMultiblock);
		activeMultiblock = newMultiblock.GetComponent<MovingMultiblock> ();
		if (activeMultiblock == null)
		{
			throw new UnityException("A multiblock with no multiblock script... bad.");
		}
		newMultiblock.transform.SetParent (this.transform);
	}



}
