using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Piece : MonoBehaviour
{
    public Board board         { get; private set; }
    public TetrominoData data  { get; private set; }
    public Vector3Int position { get; private set; }
    public Vector3Int[] cells  { get; private set; }
    private int horizontal;
    private int vertical;

    public void Initialize(Board board, Vector3Int position, TetrominoData data)
    {
        this.board = board;
        this.position = position;
        this.data = data;

        if (this.cells == null)
            this.cells = new Vector3Int[data.cells.Length];
        for (int i = 0; i < data.cells.Length; i++)
            this.cells[i] = (Vector3Int)data.cells[i];
    }

    private void Update()
    {
        // this.board.Clear(this);
        // this.board.Set(this);
    }

    public bool Move(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            this.board.Clear(this);
            horizontal = (int)context.ReadValue<Vector2>().x;
            vertical   = (int)context.ReadValue<Vector2>().y;

            print("horizontal: " + horizontal);
            print("vertical: " + vertical);
            Vector3Int newPosition = this.position;
            newPosition.x += horizontal;
            newPosition.y += vertical;

            bool valid = this.board.IsValidPosition(this, newPosition);

            if (valid)
            {
                this.position = newPosition;
                print("is valid");
            }
                
            this.board.Set(this);
            return valid;
        }
        return false;
    }

    public void Drop(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            while(Move(context))
                continue;
        }
    }
}
