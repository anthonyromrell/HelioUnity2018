using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Knight")]
public class GamePiece : GamePieceBase {



	public override void StartMove()
	{
		MovePattern.SetActive(true);
	}

	public override void MovePiece()
	{
		throw new System.NotImplementedException();
	}

	public override void EndPiece()
	{
		throw new System.NotImplementedException();
	}
}
