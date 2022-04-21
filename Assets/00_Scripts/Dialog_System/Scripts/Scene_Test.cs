using UnityEngine;

public class Scene_Test : MonoBehaviour
{
    [SerializeField]
    private RectTransform canvas;
    [SerializeField]
    private string storyLabel;

    private void Start()
    {
        Dialog_Panel.StartDialog(canvas, storyLabel);
        Dialog_GameLoop.DialogEnd += End;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Dialog_GameLoop.Instance.NewStroy("Test_Dialog");
        }
    }

    private void End()
    {
#if UNITY_EDITOR
        Debug.Log("Do things at the end");
#endif
    }

    private void OnDestroy()
    {
        Dialog_GameLoop.DialogEnd -= End;
    }
}
