using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GamePieceBase : ScriptableObject {

	//Parameters
	//Art (GameObject)
	//Piece Location
	public GameBoardBase GameBoard;
	public GameObject Art;
	public GameObject MovePattern;
	public GameSide GameSide;

	public abstract void StartMove();

	public abstract void MovePiece();

	public abstract void EndPiece();

	//Functions
	//Move Function
	//Each will move differently
	//Each Has Rules on Move

	//if Other Game Piece Hit Kill this

	//Can Die



}
