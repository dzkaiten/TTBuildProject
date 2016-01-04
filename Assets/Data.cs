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
        Question q1 = new Question("5+5", new string[] {"10", "3"}, 0);
		Question q2 = new Question("5+8", new string[] {"5", "13"}, 1);
		Question q3 = new Question("5+10", new string[] {"15", "20"}, 0);
		Question q4 = new Question("5+10", new string[] {"15", "20"}, 0);
		Question q5 = new Question("5+10", new string[] {"15", "20"}, 0);
		Question q6 = new Question("5+10", new string[] {"15", "20"}, 0);
		Question q7 = new Question("5+10", new string[] {"15", "20"}, 0);
		Question q8 = new Question("5+10", new string[] {"15", "20"}, 0);
		Question q9 = new Question("5+10", new string[] {"15", "20"}, 0);
		Question q10 = new Question("5+10", new string[] {"15", "20"}, 0);
		Question q11 = new Question("5+10", new string[] {"15", "20"}, 0);
		Question q12 = new Question("5+10", new string[] {"15", "20"}, 0);
		Question q13 = new Question("5+10", new string[] {"15", "20"}, 0);
		Question q14 = new Question("5+10", new string[] {"15", "20"}, 0);
		Question q15 = new Question("5+10", new string[] {"15", "20"}, 0);
		Question q16 = new Question("5+10", new string[] {"15", "20"}, 0);
		Question q17 = new Question("5+10", new string[] {"15", "20"}, 0);
		Question q18 = new Question("5+10", new string[] {"15", "20"}, 0);
		Question q19 = new Question("5+10", new string[] {"15", "20"}, 0);
		Question q20 = new Question("5+10", new string[] {"15", "20"}, 0);
        // define q1 & q2 up here
        questions.Add(q1);
		questions.Add(q2);
		questions.Add(q3);
		questions.Add(q4);
		questions.Add(q5);
		questions.Add(q6);
		questions.Add(q7);
		questions.Add(q8);
		questions.Add(q9);
		questions.Add(q10);
		questions.Add(q11);
		questions.Add(q12);
		questions.Add(q13);
		questions.Add(q14);
		questions.Add(q15);
		questions.Add(q16);
		questions.Add(q17);
		questions.Add(q18);
		questions.Add(q19);
		questions.Add(q20);
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