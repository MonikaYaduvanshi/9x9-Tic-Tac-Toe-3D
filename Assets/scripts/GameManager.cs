using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// -- important to know
// 0 - disable at the moment
// 1 - enable at the moment
// 2 - selected by cross and disable onwards
// 3 - selected by zero and disable onwards

public class GameManager : MonoBehaviour
{
    public List<List<KeyValuePair<Button, int>>> data = new List<List<KeyValuePair<Button, int>>>();

    //List<int> cellWins = new List<int>();
    public GameObject canvas;
    public GameObject cell;
    public Sprite crossPressedSprite;
    public Sprite zeroPressedSprite;
    public Sprite highlightSprite;
    public Sprite nonHighlightSprite;
    public Sprite disabledSprite;
    public GameObject zeroTurn;
    public GameObject crossTurn;
    public Text playerText;
    public GameObject ZeroWin;
    public GameObject CrossWin;
    int turn=0;
    Button clickedButton;
    public GameManagerUtil Util;

    void Start()
    {
        for(int i=0;i<cell.transform.childCount;i++){
            Transform cellChild = cell.transform.GetChild(i);
            List<KeyValuePair<Button, int>> temp = new List<KeyValuePair<Button, int>>();
            for(int j=0; j<cellChild.childCount; j++){
                temp.Add(new KeyValuePair<Button, int>(cellChild.GetChild(j).gameObject.GetComponent<Button>(), 1));
            }
            data.Add(temp);
        }
        // default turn
        zeroTurn.SetActive(false);
        crossTurn.SetActive(true);
    }

    public void OnClick() {
        clickedButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        Image btngo = clickedButton.GetComponent<Image>();
        // convert clickedButton.name[0] into int
        int posx = (int)clickedButton.name[0] - (int)'0' - 1;
        int posy = (int)clickedButton.name[1] - (int)'0' - 1;
            
        // assign-set value
        if(turn==0){
            DisableAll();
        }

        if(data[posx][posy].Value == 1){
            if(turn%2 == 0){
                btngo.sprite = crossPressedSprite;
                // toggle turn
                zeroTurn.SetActive(true);
                crossTurn.SetActive(false);
                data[posx][posy] = new KeyValuePair<Button, int>(clickedButton, 2);

            }
            else{
                btngo.sprite = zeroPressedSprite;
                // toggle turn
                zeroTurn.SetActive(false);
                crossTurn.SetActive(true);
                data[posx][posy] = new KeyValuePair<Button, int>(clickedButton, 3);
            }

            Debug.Log("Onclicked");
            Util.CheckWinningCondition((int)clickedButton.name[0] - (int)'0' - 1);
            HighlightNextMove(posx, posy);
            clickedButton.interactable = false;
            turn++;
        }
    }

    public void Won(int winningCell, Sprite Win){
        Debug.Log("----- Akshat Did it ------ ");  
        // GameObject winner = Instantiate(Win);
        for(int i=0;i<cell.transform.childCount;i++){
            GameObject cellChild = cell.transform.GetChild(i).gameObject;
            Image winBtn = cellChild.GetComponent<Image>();
            if(cellChild.name == (winningCell+1).ToString()){
                winBtn.sprite = Win;
                for(int j=0;j<9;j++){
                    cellChild.transform.GetChild(j).gameObject.SetActive(false);
                }
                break;
            }
        }
        // winner.SetActive(true);
        return;
    }
    public void finalWon(string x){
        cell.SetActive(false);
        crossTurn.SetActive(false);
        zeroTurn.SetActive(false);

        if(x == "x"){
            canvas.GetComponent<Image>().sprite = crossPressedSprite;
            playerText.text = "X XX XXX Won thy Game";
        }
        else {
            canvas.GetComponent<Image>().sprite = zeroPressedSprite;
            playerText.text = "O O OOOOOO wOn";
        }

    }
    void HighlightNextMove(int posx, int posy){
        if(Util.cellWins[posy] != 0){
            //give free pass
            for(int i=0;i<9;i++){
                for(int j = 0; j<9; j++){
                    if(data[i][j].Value == 2 || data[i][j].Value == 3) continue;
                    else{
                        data[i][j].Key.image.sprite = highlightSprite;
                        data[i][j].Key.interactable = true;
                        data[i][j] = new KeyValuePair<Button, int>(data[i][j].Key, 1);
                    }
                }
            }
            return;
        }
        DisableAll();
        for(int i=0;i<9;i++){
            if(data[posx][i].Value == 2 || data[posx][i].Value == 3) continue;
            else{
                data[posx][i].Key.image.sprite = nonHighlightSprite;
                data[posx][i].Key.interactable = false;
                data[posx][i] = new KeyValuePair<Button, int>(data[posx][i].Key, 0);
            }
        }

        for(int i=0;i<9;i++){
            if(data[posy][i].Value == 2 || data[posy][i].Value == 3) continue;
            else{
                data[posy][i].Key.image.sprite = highlightSprite;
                data[posy][i].Key.interactable = true;
                data[posy][i] = new KeyValuePair<Button, int>(data[posy][i].Key, 1);

            }
        }
    }
    void DisableAll(){
        for(int i=0;i<9;i++){
            for(int j=0;j<9;j++){
                if(data[i][j].Value == 2 || data[i][j].Value == 3) continue;

                data[i][j].Key.interactable = false;
                data[i][j].Key.image.sprite = nonHighlightSprite;

            }
        }
    }

}