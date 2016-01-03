using UnityEngine;
using System.Collections;

public class CubeTrigger : MonoBehaviour {
    string message = "";
	Question question;
	public float corrDir;
	public float incorrDir; 
	
	void Start() {
		init();
	}
	
	void init() {
		print("Cube Initialized");
		getQuestion();
	}
	
	void getQuestion() {
		question = GameObject.Find("DataCube").GetComponent<Data>().getQuestion();
	}
	
	void changeMessage(string message) {
		GameObject.Find("Player").GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>().changeText(message);
	}
	
	void changeArrow(float angle) {
		GameObject.Find("Player").GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>().changeArrow(angle);
	}
	
	void isVisible(bool vis) {
		GameObject.Find("Player").GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>().changeVis(vis);
	}	
	
	void changeInArrow(float angle) {
		GameObject.Find("Player").GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>().changeInArrow(angle);
	}	
	
    void OnTriggerEnter(Collider other) {
        //Call another objects function
        changeMessage(question.getQuestion());
		changeArrow(corrDir);
		changeInArrow(incorrDir);
		isVisible(true);
    }

    void OnTriggerExit(Collider other) {
        message = "";
        //Call another objects function
        changeMessage(message);
		isVisible(false);
    }
}
