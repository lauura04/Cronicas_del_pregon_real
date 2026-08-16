using UnityEngine;

public class MapLocationUI : MonoBehaviour
{
    [SerializeField] private RectTransform locationMarker;

    [Header("Posiciones por capítulo")]
    [SerializeField] private Vector2 chapter1Position;
    [SerializeField] private Vector2 chapter2Position;
    [SerializeField] private Vector2 chapter3Position;

    private void OnEnable()
    {
        UpdateLocation();
    }

    public void UpdateLocation()
    {
        if(ChapterManager.Instance == null)
        {
            Debug.LogWarning("No se ha encontrado ChapterManager");
            return;
        }
        GameChapter currentChapter = ChapterManager.Instance.CurrentChapter;

        switch (currentChapter)
        {
            case GameChapter.Chapter1:
                locationMarker.anchoredPosition = chapter1Position;
                break;
             case GameChapter.Chapter2:
                locationMarker.anchoredPosition = chapter2Position;
                break;
             case GameChapter.Chapter3:
                locationMarker.anchoredPosition = chapter3Position;
                break;
        }
    }

}
