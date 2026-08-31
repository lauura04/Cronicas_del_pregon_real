using UnityEngine;

public class CatPuzzle : MonoBehaviour
{
    [Header("POSITIONS")]
    [SerializeField] private Transform secondPosition;
    [SerializeField] private Transform thirdPosition;

    [Header("DIALOGUES")]
    [SerializeField] private DialogueData firstEncounterDialogue;
    [SerializeField] private DialogueData secondEncounterDialogue;
    [SerializeField] private DialogueData thirdEncounterDialogue;

    [Header("CHARM")]
    [SerializeField] private CharmableNPC charmableNPC;

    private int encounter = 0;

    public void Interact()
    {
        switch (encounter)
        {
            case 0:
                FirstEncounter();
                break;

            case 1:
                SecondEncounter();
                break;

            case 2:
                ThirdEncounter();
                break;
        }
    }

    private void FirstEncounter()
    {
        encounter++;

        DialogueManager.Instance.StartDialogue(
            firstEncounterDialogue,
            () => TeleportCat(secondPosition)
        );
    }

    private void SecondEncounter()
    {
        encounter++;

        DialogueManager.Instance.StartDialogue(
            secondEncounterDialogue,
            () => TeleportCat(thirdPosition)
        );
    }

    private void ThirdEncounter()
    {
        encounter++;

        DialogueManager.Instance.StartDialogue(
            thirdEncounterDialogue,
            UnlockCatCharm
        );
    }

    private void UnlockCatCharm()
    {
        charmableNPC.UnlockCharm();
    }

    private void TeleportCat(Transform destination)
    {
        if (destination == null)
            return;

        transform.position = destination.position;
    }
}