using UnityEngine;
using System.Collections;

public class TetragridInfo : MonoBehaviour {

	public UnityEngine.UI.Text gridCellsSoFarText;
	public UnityEngine.UI.Text gridCellsNeededText;
	public UnityEngine.UI.Text currentLevelText;

	private int filledCells;
	private int neededCells;
	private static TetragridInfo currentInfo;
	private int currentBlockSize = 2;
	private Vector2 currentBoardSize = new Vector2(9, 6);
	private int currentLevel = 0;

	public static TetragridInfo CurrentInfo()
	{
		if (currentInfo == null)
			currentInfo = FindObjectOfType<TetragridInfo>();
		return currentInfo;
	}

	void Start()
	{
		NextLevel ();
	}

	public int CurrentBlockSize()
	{
		return currentBlockSize;
	}

	public void UpdateFilledCells()
	{
		filledCells = Board.CurrentBoard ().CountActiveCells ();
		gridCellsSoFarText.text = filledCells.ToString();
	}

	public void SetCellsNeeded()
	{
		neededCells = (int)(Board.CurrentBoard ().BoardSize () * 0.8f) - (currentBlockSize*currentBlockSize*4);
		gridCellsNeededText.text = neededCells.ToString ();
	}

	public void LevelOver()
	{
		if (filledCells < neededCells)
			Application.LoadLevel (Application.loadedLevel);
		else
		{
			NextLevel();
		}
	}

	public void DisplayCurrentLevel()
	{
		currentLevelText.text = currentLevel.ToString ();
	}

	private void NextLevel()
	{
		currentBoardSize.x = (int)(currentBoardSize.x * 1.6f);
		currentBoardSize.y = (int)(currentBoardSize.y * 1.6f);
		currentBlockSize++;
		currentLevel++;
		Board.CurrentBoard ().GenerateBoard ((int)currentBoardSize.x, (int)currentBoardSize.y);
		SetCellsNeeded ();
		UpdateFilledCells ();
		DisplayCurrentLevel ();
	}
}
