using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Data : MonoBehaviour {
    List<Question> questions = new List<Question>();
    
    //string[,] array2Db = new string[3, 2]

    // Use this for initialization
    void Awake() {
        init();
    }
    
    void init() {
        Question q1 = new Question("question", new string[] {"true", "false"}, 1);
		Question q2 = new Question("question32", new string[] {"true", "false"}, 1);
		Question q3 = new Question("question45", new string[] {"true", "false"}, 1);
        // define q1 & q2 up here
        questions.Add(q1);
		questions.Add(q2);
		questions.Add(q3);
		print("Datacube Initialized");
    }
    
    //call this thang
    public Question getQuestion() {
        int rand = Random.Range(0, questions.Count);
		//print(questions.Count);
		//print(rand);
        Question re = questions[rand];
        questions.RemoveAt(rand);
        return re;
    }
}