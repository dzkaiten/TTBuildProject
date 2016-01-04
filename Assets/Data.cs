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
/*
Kento Hayworth -Environmental Engineering

Follow the following format please.
Q: Which of these are not considered greenhouse gases?
a) Methane
b) Water Vapor
c) Diatomic Nitrogen

--------------------------------------------------------------------------------------------------------

Daniel Zhang - CS

Q: What is the Big - O time complexity for QuickSort’s best case scenario ?
  a)
            n log(n)
         b) log(n)

         -------------------------------------------------------------------------------------------------------- -

         Audrey Olson - CS

Q: Which of the following is not a greedy algorithm?
a) Djikstra’s Algorithm
b) Binary Search
c) Kruskal’s Algorithm

Q: What program converts symbolic assembly into binary machine code?
a) Compiler
b) Binary Converter
c) Assembler

Q: In MIPs, the following instruction: 
add $t1, $t2, $t3
does what?
a) $t1 = $t2 + $t3
b) $t2 = $t1 + $t3
c) $t3 = $t1 + $t2

*/

        Question q1 = new Question("The Big - O time complexity for QuickSort’s best case scenario is log(n).", new string[] {"True", "False"}, 1);
		Question q2 = new Question("Diatomic nitrogen is considered a greenhouse gas.", new string[] {"True", "False"}, 1);
		Question q3 = new Question("Binary search is not a greedy algorithm.", new string[] {"True", "False"}, 0);
		Question q4 = new Question("The Norton equivalent current can be obtained by short circuiting all inductors and open circuiting all capacitors.", new string[] {"True", "False"}, 1);
		Question q5 = new Question("For a molecule with three centers of chirality, the number of stereoisomers can be no more than 2,", new string[] {"True", "False"}, 1);
		Question q6 = new Question("When the center of gravity of a body lies at the point of suspension or support, the body is said to be in unstable equilibrium.", new string[] {"True", "False"}, 1);
		Question q7 = new Question("Velocity is zero when an object reaches its apex.", new string[] {"True", "False"}, 0);
		Question q8 = new Question("If apples are bananas and bananas are grumpy cats, then apples are grumpy cats.", new string[] {"True", "False"}, 0);
		Question q9 = new Question("If a linear network is driven by a sinusoidal source, in a steady state every voltage and every current has the same phase.", new string[] {"True", "False"}, 1);
		Question q10 = new Question("A particle with zero instantaneous acceleration must have zero instantaneous velocity.", new string[] {"True", "False"}, 1);
		Question q11 = new Question("Assembler converts symbolic assembly into binary machine code.", new string[] {"True", "False"}, 0);
		Question q12 = new Question("Velocity can occur in a direction perpendicular of net force.", new string[] {"True", "False"}, 0);
		Question q13 = new Question("The work done on an object can be negative.", new string[] {"True", "False"}, 0);
		Question q14 = new Question("Spontaneous reaction will occur if standard potential and Gibbs free energy are both negative.", new string[] {"True", "False"}, 1);
		Question q15 = new Question("In a galvanic cell, the anode undergoes reduction and the cathode undergoes oxidation. ", new string[] {"True", "False"}, 1);
		Question q16 = new Question("Between iron and zinc, zinc is the element that has the higher first ionization energy.", new string[] {"True", "False"}, 0);
		Question q17 = new Question("Purines have two rings while pyrimidines have 1.", new string[] {"True", "False"}, 0);
		Question q18 = new Question("snRNP U1 and U2 are responsible for the initial steps in splicing.", new string[] {"True", "False"}, 0);
		Question q19 = new Question("Central dogma is the process of going from DNA to pre-mRNA to RNA to protein.", new string[] {"True", "False"}, 0);
		Question q20 = new Question("At high frequencies, a capacitors acts like an open circuit.", new string[] {"15", "20"}, 1);
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