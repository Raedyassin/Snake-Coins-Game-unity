using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // variables
    public GameObject snakePiece;
    public GameObject foodPiece;
    int startingCountPodySnake = 20;
    Vector3 direction = new Vector3(0.15f, 0, 0);
    public bool gameOver = false;
    //bool foodPlus = false;
    int timePerSecond = 150*60;
    int time;
	int foodEated = 0;
    int allFood = 20;
    bool isLocked = false;
    public TextMeshProUGUI displayFoodEated;
    public TextMeshProUGUI displayTime;
    public TextMeshProUGUI state;
    public GameObject resultGame;
    bool continueRun = true;
    int setTime;
    // Lists
    List<Vector3> positions = new List<Vector3>();
	List<Vector3> foodPositiions = new List<Vector3>();
    List<GameObject> snake = new List<GameObject>();
	void Start()
    {
		time = timePerSecond;
		displayFoodEated.text = "Eate: " + foodEated + "/" + allFood;
        setTime= time/60;
		displayTime.text = $"Time: {(setTime/60<= 9 ? "0" + (setTime/60).ToString() : (setTime/60).ToString())}:{(setTime%60 <= 9 ? "0" + (setTime % 60).ToString() : (setTime % 60).ToString())}";

		for (int i = 0; i < startingCountPodySnake; i++)
        {
			positions.Add(new Vector3((i - startingCountPodySnake) * 0.15f, 0, 0));
			GameObject newSnakePiece = Instantiate(snakePiece);
			newSnakePiece.transform.position = positions[i];
			snake.Add(newSnakePiece);
			if (i==startingCountPodySnake - 1)
            {
                newSnakePiece.AddComponent<SnakePiece>();
            }
            if (i >= startingCountPodySnake - 20)
            {
                newSnakePiece.tag = "Untagged";
			}

            
        }
        
        for(int i=0;i<allFood;i++)
        {
            StartCoroutine(CreateFood());
        }

        StartCoroutine(MoveSnake());
    }


    void Update()
    {
		Result();
		if (continueRun)
        {
			time--;
            displayFoodEated.text = "Eate: " + foodEated + "/" + allFood;
			setTime = time / 60;
			displayTime.text = $"Time: {(setTime / 60 <= 9 ? "0" + (setTime / 60).ToString() : (setTime / 60).ToString())}:{(setTime % 60 <= 9 ? "0" + (setTime % 60).ToString() : (setTime % 60).ToString())}";
		}
		if (!isLocked)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow)&& direction.z==0) { direction = new Vector3(0, 0, -0.15f); isLocked = true; }
            else if (Input.GetKeyDown(KeyCode.UpArrow) && direction.z == 0) { direction = new Vector3(0, 0, 0.15f); isLocked = true; }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && direction.x == 0) { direction = new Vector3(-0.15f, 0, 0); isLocked = true; }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && direction.x == 0) { direction = new Vector3(0.15f, 0, 0); isLocked = true; }
        }
        
	}
    public void Restart()
    {
        Time.timeScale= 1.0f;
		SceneManager.LoadScene("SampleScene");
	}
	private void Result()
	{
        if(time / 30 == 0 && foodEated != allFood)
        {
            state.text = "Game Over";
            state.color = Color.red;
            Stop();

		}
        else if (time / 30 != 0 && foodEated == allFood)
        {
			state.text = "Success";
            state.color = Color.green;
            Stop();
		}
	}
    private void Stop()
    {
		resultGame.SetActive(true);
		gameOver = true;
		continueRun = false;
	}
	IEnumerator MoveSnake()
    {
        yield return new WaitForSeconds(0.03f);

        if (gameOver) yield break;
        
		positions.Add(positions[positions.Count - 1] + direction);
		positions.RemoveAt(0);
		//      if (foodPlus)
		//      {
		//          GameObject newSnakePiece = Instantiate(snakePiece);
		//          newSnakePiece.transform.position = positions[0];
		//          snake.Insert(0, newSnakePiece);
		//          foodPlus = false;
		//}
		//      else positions.RemoveAt(0);

		for (int i = 0; i < positions.Count; i++)
        {
            snake[i].transform.position = positions[i];
        }
		isLocked = false;
		StartCoroutine(MoveSnake());
    }
    IEnumerator CreateFood()
    {
        yield return new WaitForSeconds(0.1f);
        int x, z;
        bool validLocation = true ;
        do {
            validLocation = true;
			x = UnityEngine.Random.Range(-10, 10);
			z = UnityEngine.Random.Range(-12, 13);
            for(int i=0;i<positions.Count;i++) {
                if (positions[i].x==x && positions[i].z==z) {
                    validLocation = false;
                }
            }
			for (int i = 0; i < foodPositiions.Count; i++)
			{
				if (foodPositiions[i].x == x && foodPositiions[i].z == z)
				{
					validLocation = false;
				}
			}
		} while( !validLocation );
        foodPositiions.Add(new Vector3(x, 0, z));
        GameObject newFood = Instantiate(foodPiece);
        newFood.transform.position = new Vector3(x, 0, z);

	}
    public void EateFood()
    {
        foodEated++;
	}


}
