using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameBoard")]
public class GameBoardBase : ScriptableObject {

	//Detect Piece Interactions
	//Piece Location
	//Holds Pieces
	public Transform CurrentPiece;
	public Transform PieceReLocation;

	public void MovePiece()
	{
		CurrentPiece.position = PieceReLocation.position;
	}
}
