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
    public int rotationIndex { get; private set; }
    private int horizontal;
    private int vertical;


    public void Initialize(Board board, Vector3Int position, TetrominoData data)
    {
        this.board = board;
        this.position = position;
        this.data = data;
        this.rotationIndex = 0;

        if (this.cells == null)
            this.cells = new Vector3Int[data.cells.Length];
        for (int i = 0; i < data.cells.Length; i++)
            this.cells[i] = (Vector3Int)data.cells[i];
    }

    private void Update()
    {
        // this.board.Clear(this);
        // this.board.Set(this);
        // need to clear and set whenever moved
    }

    public void Move(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            this.board.Clear(this);
            horizontal = (int)context.ReadValue<Vector2>().x;
            vertical   = (int)context.ReadValue<Vector2>().y;

            MoveTile(new Vector2Int (horizontal, vertical));
            this.board.Set(this); 
        }
        //return false;
        //changed from bool to void
        //bool is now MoveTile
    }

    private bool MoveTile(Vector2Int translation)
    {
        Vector3Int newPosition = this.position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool valid = this.board.IsValidPosition(this, newPosition);

        if (valid)
            this.position = newPosition;

        return valid;
        // return true if actually moved
    }

    public void Drop(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            this.board.Clear(this);
            HardDrop();
            this.board.Set(this); 
        }
    }

    private void HardDrop()
    {
        while (MoveTile(Vector2Int.down))
            continue;
        
    }

    public void RotateLeft(InputAction.CallbackContext context)
    {
        Rotate(-1);
    }

    public void RotateRight(InputAction.CallbackContext context)
    {
        Rotate(1);
    }

    private void Rotate (int direction)
    {
        this.board.Clear(this);

        this.rotationIndex += direction;



        this.board.Set(this);
    }

    private int Wrap (int input, int min, int max)
    {
        if (input < min)
            return max - (min - input) % (max - min);
        else
            return min + (input - min) % (max - min);
    }
}