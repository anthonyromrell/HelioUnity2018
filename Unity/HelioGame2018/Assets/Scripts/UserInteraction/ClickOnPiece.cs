using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickOnPiece : MonoBehaviour
{

	public GamePieceBase GamePiece;

	private void OnMouseDown()
	{
		GamePiece.StartMove();
	}
}
