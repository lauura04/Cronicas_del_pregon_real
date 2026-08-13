using UnityEngine;

public class WoolInteraction : MonoBehaviour
{
   private GiveItem giveItem;
   private bool woolCollected;

   public void TryGetWool()
    {
        if (woolCollected)
        {
            return;
        }

        if(TutorialController.Instance == null)
        {
            return;
        }

        if (!TutorialController.Instance.CanCollectWool())
        {
            Debug.Log("Todavía no puedes conseguir la lana");
            return;
        }

        GiveWool();
    }

    private void GiveWool()
    {
        woolCollected = true;
        if(giveItem != null)
        {
            giveItem.Give();
        }

        Debug.Log("Lana conseguida");
    }
}
