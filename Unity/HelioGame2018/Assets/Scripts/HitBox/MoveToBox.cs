using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToBox : MonoBehaviour
{

	public GameBoardBase GameBoard;
	
	private void OnMouseUp()
	{
		GameBoard.PieceReLocation = transform;
		GameBoard.MovePiece();
	}
}
