using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour 
{
	public GameObject gridBox;
	public GameObject startingSpaceIndicator;

	private GameObject[,] boxGrid = new GameObject[0,0];
	private static Board currentBoard;

	public static Board CurrentBoard()
	{
		if (currentBoard == null)
			currentBoard = FindObjectOfType<Board> ();
		if (currentBoard == null)
			throw new UnityException ("No current board");
		return currentBoard;
	}

	public Vector2 GetCenterPoint()
	{
		return new Vector2 ((int)(boxGrid.GetLength (0) / 2), (int)(boxGrid.GetLength (1) / 2));
	}

	public bool GetBoxExists(int x, int y)
	{
		if (x >= boxGrid.GetLength (0)|| 
		    x < 0 ||
		    y >= boxGrid.GetLength (1)||
		    y < 0)
			return false;
		return true;
	}

	public bool GetBoxState(int x, int y)
	{
		if (!GetBoxExists (x, y))
			return false;
		return boxGrid [x, y].activeSelf;
	}

	public void SetBoxState(int x, int y, bool active)
	{
		if (!GetBoxExists (x, y))
			throw new UnityException ("Our board isn't that big, or you entered a negative number");
		boxGrid [x, y].SetActive (active);
	}

	private void ClearBoard()
	{
		foreach (GameObject gridBox in boxGrid)
		{
			Destroy(gridBox);
		}
		boxGrid = new GameObject[0,0];
	}

	public void GenerateBoard(int width, int height)
	{
		ClearBoard ();
		BuildBoxGrid (width, height);
		IndicateStartingSpace ();
	}

	private void BuildBoxGrid(int width, int height)
	{
		float boxWidth = 0.8f / (float)width;
		float boxHeight = 1f / (float)height;
		
		boxGrid = new GameObject[width, height];
		
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				GameObject newBox = Instantiate(gridBox) as GameObject;
				if (newBox.GetComponent<RectTransform>() == null)
					throw new UnityException("gridBox must have a rect transform");
				newBox.GetComponent<RectTransform>().SetParent(gameObject.transform);
				newBox.GetComponent<RectTransform>().anchorMin = new Vector2(x * boxWidth,
				                                                             y * boxHeight);
				newBox.GetComponent<RectTransform>().anchorMax = new Vector2((x + 1)* boxWidth,
				                                                             (y + 1) * boxHeight);
				newBox.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
				newBox.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
				newBox.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
				newBox.SetActive(false);
				boxGrid[x, y] = newBox;
			}
		}
	}

	private void IndicateStartingSpace()
	{
		int size = TetragridInfo.CurrentInfo ().CurrentBlockSize() - 1;
		int xMin = (int)GetCenterPoint ().x - size;
		int yMin = (int)GetCenterPoint ().y - size;
		int xMax = (int)GetCenterPoint ().x + size;
		int yMax = (int)GetCenterPoint ().y + size;
		startingSpaceIndicator.GetComponent<RectTransform>().anchorMin
			= boxGrid[xMin, yMin].GetComponent<RectTransform>().anchorMin;
		startingSpaceIndicator.GetComponent<RectTransform>().anchorMax
			= boxGrid[xMax, yMax].GetComponent<RectTransform>().anchorMax;
		startingSpaceIndicator.GetComponent<RectTransform> ().offsetMin = Vector2.zero;
		startingSpaceIndicator.GetComponent<RectTransform> ().offsetMax = Vector2.zero;
	}

	public bool StartingSpaceClear()
	{
		int size = TetragridInfo.CurrentInfo ().CurrentBlockSize() - 1;
		int xMin = (int)GetCenterPoint ().x - size;
		int yMin = (int)GetCenterPoint ().y - size;
		int xMax = (int)GetCenterPoint ().x + size;
		int yMax = (int)GetCenterPoint ().y + size;

		for (int x = xMin; x <= xMax; x++)
			for (int y = yMin; y <= yMax; y++)
		{
			if (GetBoxState(x, y))
				return false;
		}

		return true;
	}

	public int BoardSize()
	{
		return boxGrid.Length;
	}

	public int CountActiveCells()
	{
		int activeCells = 0;
		foreach(GameObject cell in boxGrid)
		{
			if (cell.activeSelf)
				activeCells++;
		}
		return activeCells;
	}
}

