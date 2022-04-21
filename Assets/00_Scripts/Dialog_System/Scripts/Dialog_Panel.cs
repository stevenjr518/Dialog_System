using UnityEngine;

public static class Dialog_Panel
{
    public static void StartDialog(RectTransform _canvas, string _storyLabel)
    {
        if (Dialog_GameLoop.Instance != null)
        {
            Dialog_GameLoop.Instance.NewStroy(_storyLabel);
            return;
        }
        GameObject obj = Object.Instantiate(Resources.Load<GameObject>("Dialog_Panel"), _canvas);
        Dialog_GameLoop dialog = obj.GetComponent<Dialog_GameLoop>();
        dialog.TargetCanvas = _canvas;
        dialog.StoryLabel = _storyLabel;
    }
}
