using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakePiece : MonoBehaviour
{
	private void OnTriggerEnter(Collider other) {
        if (other.tag == "Wall" || other.tag == "SnakePiece")
        {
            GameObject.FindObjectOfType<GameManager>().gameOver = true;
        }
        else if(other.tag == "Food")
        {
            GameObject.FindObjectOfType<GameManager>().EateFood();
            Destroy(other.gameObject);
		}
	}
}
