using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntroController : MonoBehaviour
{
    [SerializeField] private DialogueData Introduccion;
    private void Start()
    {
        DialogueManager.Instance.StartDialogue(
            Introduccion,FinishIntro);
    }

   
    private void FinishIntro()
    {
        SceneLoader.Instance.LoadScene("Tutorial");
    }
}