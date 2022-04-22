using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Dialog_System;

public class Dialog_GameLoop : MonoBehaviour
{
    #region Public Members
    [System.Serializable]
    public struct Dialog
    {
        public string chrLabel;//Image addressed label
        public string chrName;
        public string chrPos;//mid, left and right
        public string words;
    }

    public struct ChrImage
    {
        public Image image;
        public string pos;
    }

    public static Dialog_GameLoop Instance;
    public List<Dialog> Dialogs = new List<Dialog>();
    public string StoryLabel;
    public delegate void DialogEndMethod();
    public static DialogEndMethod DialogEnd;
    public RectTransform TargetCanvas;
    public bool IsEnd;
    #endregion

    #region Private Members
    private List<ChrImage> chrImages = new List<ChrImage>();
    private Text guiText;
    private Text chrNameText;
    private TextAsset textFile = null;
    private Animator animator;
    private Button skipButton;
    private Dialog_Animation ani;
    private DialogSystem dialogSystem;
    private float delay = 1.5f;
    #endregion

    #region MonoBehaviour Methods
    void Awake()
    {
        if (Instance != null) {
            DestroyImmediate(Instance.gameObject);
        }
        Instance = this;
        animator = GetComponent<Animator>();
        skipButton = UIGet.GetUIComponent<Button>(gameObject, "Skip_Button");
        chrNameText = UIGet.GetUIComponent<Text>(gameObject, "Chr_Name");
        skipButton.onClick.AddListener(() => Skip());
        ani = new Dialog_Animation(animator);
        chrImages.Add(new ChrImage
        {
            image = UIGet.GetUIComponent<Image>(gameObject, "Chr_Left"),
            pos = "left"
        });
        chrImages.Add(
            new ChrImage
            {
                image = UIGet.GetUIComponent<Image>(gameObject, "Chr_Right"),
                pos = "right"
            });
        chrImages.Add(
            new ChrImage
            {
                image = UIGet.GetUIComponent<Image>(gameObject, "Chr_Mid"),
                pos = "mid"
            });
        foreach(ChrImage chr in chrImages)
        {
            chr.image.gameObject.SetActive(false);
        }
        
    }

    void Start()
    {
        if (StoryLabel == "")
        {
            StoryLabel = "Test_Dialog";
        }
        guiText = UIGet.GetUIComponent<Text>(TargetCanvas.gameObject, "Dialog_Text");
        LoadTextFile();
        if(textFile == null)
        {
            Disappear();
            return;
        }
        ani.Appear();
        Invoke("StartStory", delay);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            dialogSystem.IsWaiting = false;
        }
    }

    #endregion

    #region Public Methods
    private void LoadTextFile()
    {
        textFile = Resources.Load<TextAsset>("Json/" + StoryLabel);
        if (textFile == null)
        {
#if UNITY_EDITOR
            Debug.LogWarning("No text file found");
#endif
            return;
        }
        JsonUtility.FromJsonOverwrite(textFile.text, this);
    }

    public void NewStroy(string label)
    {
        if(label == "") { return; }
        IsEnd = false;
        guiText.text = "";
        foreach(ChrImage chr in chrImages)
        {
            chr.image.sprite = null;
            chr.image.gameObject.SetActive(false);
        }
        StoryLabel = label;
        LoadTextFile();
        if (textFile == null)
        {
            Disappear();
            return;
        }
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            ani.Appear();
            Invoke("StartStory", delay);
            return;
        }
        StartStory();
    }

    public void StartStory()
    {
        if (gameObject.activeSelf)
        {
            StartCoroutine(StartDialog());
        }
    }

    public void Skip()
    {
        ani.Disappear();
        chrNameText.text = "";
    }

    public void Next()
    {
        if (dialogSystem != null)
        {
            dialogSystem.IsWaiting = false;
        }
    }

    public void Disappear()
    {
        StoryLabel = "";
        if (DialogEnd != null)
        {
            DialogEnd();
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("DialogEnd is null");
#endif
        }
        gameObject.SetActive(false);
    }
    #endregion

    #region Private Methods

    private IEnumerator StartDialog()
    {
        foreach(Dialog dialog in Dialogs)
        {
            Sprite sp;
            if (dialog.chrLabel != "")
            {
                sp = AdsObjGet.LoadSprite(dialog.chrLabel);
            }
            else
            {
                sp = null;
            }
            if(sp == null)
            {
#if UNITY_EDITOR
                Debug.LogWarning("Cannot find the image of the label");
#endif
            }
            SetSpeakChr(dialog.chrPos, sp, dialog.chrName);
            dialogSystem = new DialogSystem(dialog.words, guiText, this);
            StartCoroutine(dialogSystem.PrintTextContent());
            yield return new WaitUntil(() => dialogSystem.IsEnd == true);
            dialogSystem.IsEnd = false;
        }
        IsEnd = true;
        chrNameText.text = "";
        ani.Disappear();
    }

    private void SetSpeakChr(string pos, Sprite sp, string name)
    {
        chrNameText.text = name;
        if (pos == "") {
            return;
        }
        foreach (ChrImage chr in chrImages)//Set sprite, position and color
        {
            if(chr.pos == pos)
            {
                if (!chr.image.gameObject.activeSelf)
                {
                    chr.image.gameObject.SetActive(true);
                }
                chr.image.sprite = sp;
                chr.image.color = Color.white;
            }
            else
            {
                chr.image.color = new Color(1, 1, 1, 0.5f);
            }
        }
    }
    #endregion
}
