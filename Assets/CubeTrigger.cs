using UnityEngine;
using System.Collections;

public class CubeTrigger : MonoBehaviour {
    string message = "";
	Question question;
	public float corrDir;
	public float incorrDir;

	string[] nothing = {"", ""};
	
	void Start() {
		init();
	}
	
	void init() {
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
	
	void changeArrow2(float angle) {
		GameObject.Find("Player").GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>().changeArrow2(angle);
	}

	void changeAnswers(string[] ans) {
		GameObject.Find("Player").GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>().changeAns(ans);
	}
	
    void OnTriggerEnter(Collider other) {
        //Call another objects function
		changeAnswers(question.getAnswers());
        changeMessage(question.getQuestion());
		if(question.getIndex() == 0) {
			changeArrow(corrDir);
			changeArrow2(incorrDir);
		}
		else {
			changeArrow2(corrDir);
			changeArrow(incorrDir);
		}
		isVisible(true);
    }

    void OnTriggerExit(Collider other) {
        message = "";
        //Call another objects function
        changeMessage(message);
		isVisible(false);
		changeAnswers(nothing);
    }
}
