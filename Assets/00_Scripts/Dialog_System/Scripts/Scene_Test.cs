using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene_Test : MonoBehaviour
{

    public RectTransform canvas;
    public string storyLabel;

    private void Start()
    {
        Dialog_Panel.StartDialog(canvas, storyLabel);
        Dialog_GameLoop.dialogEnd += End;
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
        Debug.Log("End Move");
    }

    private void OnDestroy()
    {
        Dialog_GameLoop.dialogEnd -= End;
    }
}
