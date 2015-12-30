using UnityEngine;
using System.Collections;

public class CubeTrigger : MonoBehaviour {
    string message = "";
    void OnTriggerEnter(Collider other) {
        print("enter");
        message = "hi";
        //Call another objects function
        GameObject.Find("Player").GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>().changeText(message);
    }

    void OnTriggerExit(Collider other) {
        print("exit");
        message = "bye";
        //Call another objects function
        GameObject.Find("Player").GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>().changeText(message);
    }
}
