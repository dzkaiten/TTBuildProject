using UnityEngine;
using System.Collections;

public class Question {
	string question;
	string[] answers;
	int cIndex;
	
	public Question(string q, string[] ans, int index) {
		question = q;
		cIndex = index;
		answers = ans;
	}
	
	public string getQuestion() {
		return question;
	}
	
	public string[] getAnswers() {
		return answers;
	}
}
