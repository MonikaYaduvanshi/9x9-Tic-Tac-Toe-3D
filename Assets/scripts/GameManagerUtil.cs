using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// cellWins criterions 
/// 0 - not available
/// 1 - x won
/// 2 - o won
/// </summary>


/// <summary>
// -- important to know 
// 0 - disable at the moment
// 1 - enable at the moment
// 2 - selected by cross and disable onwards
// 3 - selected by zero and disable onwards
/// </summary>

public class GameManagerUtil : MonoBehaviour
{
    public GameManager gameManager;
    public List<int> cellWins = new List<int>();
    void Start(){
        for(int i = 0; i<9; i++){
            cellWins.Add(0);
        }
    }

    public void CheckWinningCondition(int cellNum){
        int counter_o = 0;
        int counter_x = 0;

        // checking in rows
        for(int i = 0; i<3; i++)
        {
            counter_o = counter_x = 0;
            for(int j = 0; j<3; j++)
            {
                if(gameManager.data[cellNum][3*i + j].Value == 2)
                    counter_x += 1;
                if(gameManager.data[cellNum][3*i + j].Value == 3)
                    counter_o += 1;        
            }
            Debug.Log("Rows: " + cellNum + " counterX: " + counter_x + " counterO: " + counter_o);
            if(checkWon(cellNum, counter_x, counter_o)) return;
        }

        // checking in columns
        for(int i = 0; i<3; i++)
        {
            counter_o = counter_x = 0;
            for(int j = 0; j<3; j++)
            {
                if(gameManager.data[cellNum][i + 3*j].Value == 2)
                    counter_x += 1;
                if(gameManager.data[cellNum][i + 3*j].Value == 3)
                    counter_o += 1;        
            }
            Debug.Log("Columns: " + cellNum + " counterX: " + counter_x + " counterO: " + counter_o);
            if(checkWon(cellNum, counter_x, counter_o)) return;
        }
        
        // diagonal top-left to bottom-right
        counter_o = counter_x = 0;
        for(int i = 0; i< 3; i++)
        {
            if(gameManager.data[cellNum][4*i].Value == 2)
                counter_x += 1;
            if(gameManager.data[cellNum][4*i].Value == 3)
                counter_o += 1;    
        }
        Debug.Log("TL to BR: " + cellNum + " counterX: " + counter_x + " counterO: " + counter_o);
        if(checkWon(cellNum, counter_x, counter_o)) return;

        // diagonal top-right to botton-left
        counter_o = counter_x = 0;
        for(int i = 0; i< 3; i++)
        {
            if(gameManager.data[cellNum][2*i + 2].Value == 2)
                counter_x += 1;
            if(gameManager.data[cellNum][2*i + 2].Value == 3)
                counter_o += 1;    
        }
        Debug.Log("BL to TR: " + cellNum + " counterX: " + counter_x + " counterO: " + counter_o);
        if(checkWon(cellNum, counter_x, counter_o)) return;
        checkTie(cellNum);
    }

    bool checkWon(int cell, int counter_x, int counter_o){
        if(counter_x == 3){
            gameManager.Won(cell, gameManager.crossPressedSprite);
            cellWins[cell] = 1;
            checkFinalWinningCondition();
            return true;
        }
        if(counter_o == 3){
            gameManager.Won(cell, gameManager.zeroPressedSprite);
            cellWins[cell] = 2;
            checkFinalWinningCondition();
            return true;
        }
        return false;
    }

    void checkTie(int cell){
        for(int i = 0; i<9; i++){
            if(gameManager.data[cell][i].Value == 1)
                return;
        }
        Debug.Log("Tie on cell: " + cell);
        gameManager.cellTie(cell);
    }

    void checkFinalTie(){
        for(int i = 0; i<9; i++){
            if(cellWins[i] == 0)
                return;
        }
        Debug.Log("Tie on final cell");
        gameManager.finalTie();
    }

    void checkFinalWinningCondition(){
        int final_o = 0;
        int final_x = 0;

        // checking in rows
        for(int i = 0; i<3; i++)
        {
            final_o = final_x = 0;
            for(int j = 0; j<3; j++)
            {
                if(cellWins[3*i + j] == 1)
                    final_x += 1;
                if(cellWins[3*i + j] == 2)
                    final_o += 1;        
            }
            if(checkFinalWon(final_x, final_o)) return;
        }
        // checking in columns
        for(int i = 0; i<3; i++)
        {
            final_o = final_x = 0;
            for(int j = 0; j<3; j++)
            {
                if(cellWins[i + 3*j] == 1)
                    final_x += 1;
                if(cellWins[i + 3*j] == 2)
                    final_o += 1;        
            }
           if(checkFinalWon(final_x, final_o)) return;
        }

        // diagonal top-left to bottom-right
        final_o = final_x = 0;
        for(int i = 0; i< 3; i++)
        {
            if(cellWins[4*i] == 1)
                final_x += 1;
            if(cellWins[4*i] == 2)
                final_o += 1;    
        }
       if(checkFinalWon(final_x, final_o)) return;

        // diagonal top-right to botton-left
        final_o = final_x = 0;
        for(int i = 0; i< 3; i++)
        {
            if(cellWins[2*i + 2] == 1)
                final_x += 1;
            if(cellWins[2*i + 2] == 2)
                final_o += 1;    
        }
       if(checkFinalWon(final_x, final_o)) return; 

        // nobody won
        checkFinalTie();
    }

    bool checkFinalWon(int x, int o){
        if(x == 3){
            // x won
            gameManager.finalWon("x");
            return true;
        }
        else if(o == 3){
            // o won 
            gameManager.finalWon("o");
            return true;
        }
        return false;
    }
}