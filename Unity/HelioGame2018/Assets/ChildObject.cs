using UnityEngine;

public class ChildObject : MonoBehaviour {

	public Transform Piece;

	private void Awake()
	{
		Piece = GetComponentInParent<ParentObject>().transform;
	}

	private void OnMouseDown()
	{
		Piece.position = transform.position;
	}
}