using System.Collections;
using UnityEngine;
using UnityEngine.UI;
namespace Dialog_System
{
    public class DialogSystem : IDialog
    {
        #region Public members
        public bool IsWaiting = false;
        public bool IsEnd = false;
        public DialogSystem(string _textContent, Text _guiTarget, MonoBehaviour _mono)
        {
            SplitSlashN(_textContent);
            guiTarget = _guiTarget;
            gameLoop = _mono;
            Initialize();
        }
        #endregion

        #region Private members
        private Text guiTarget = null;
        private float dialogSpeed = 0.01f;
        private MonoBehaviour gameLoop = null;
        private const char CM_CHAR_START = '[';
        private const char CM_CHAR_END = ']';
        private char lastChar;
        private string command;
        private bool isStartingCommand = false;
        private enum SpecialCharType
        {
            StartChar,
            CommandChar,
            EndChar,
            NormalChar
        }
        #endregion

        #region Public Methods
        public override IEnumerator PrintTextContent()
        {
            guiTarget.text = "";
            for (int i = 0; i < textContent.Length; ++i)
            {
                SpecialCharType type = SpecialCharCheck(textContent[i]);
                if (type == SpecialCharType.NormalChar)
                {
                    guiTarget.text += textContent[i];
                    lastChar = textContent[i];
                    yield return new WaitForSeconds(dialogSpeed);
                }
                lastChar = textContent[i];
                yield return new WaitUntil(() => IsWaiting == false);
            }
            IsEnd = true;
        }
        #endregion

        #region Private Methods
        private void SplitSlashN(string text)
        {
            string[] s = text.Split('\n');
            foreach (string c in s)
            {
                textContent += c;
            }
        }

        private void Initialize()
        {
            commandMap.Add("l", () => gameLoop.StartCoroutine(CM_l_WaitAndPrint()));
            commandMap.Add("r", CM_r_ChangeLine);
            commandMap.Add("w", () => gameLoop.StartCoroutine(CM_w_WaitAndClean()));
            commandMap.Add("lr", () => gameLoop.StartCoroutine(CM_lr_WaitAndChangeLine()));
        }

        private SpecialCharType SpecialCharCheck(char _char)
        {
            if (_char == CM_CHAR_START)
            {
                if (lastChar != CM_CHAR_START)
                {
                    isStartingCommand = true;
                    return SpecialCharType.StartChar;
                }
                else
                {
                    isStartingCommand = false;
                    return SpecialCharType.NormalChar;
                }
            }
            else if (_char == CM_CHAR_END)
            {
                if (isStartingCommand)
                {
                    isStartingCommand = false;
                    commandMap[command]();
                    command = "";
                    //Debug.Log(command);
                    return SpecialCharType.EndChar;
                }
            }
            else if (_char != CM_CHAR_START)
            {
                if (lastChar == CM_CHAR_START)
                {
                    if (_char != CM_CHAR_END)
                    {
                        if (isStartingCommand)
                        {
                            command += _char;
                            return SpecialCharType.CommandChar;
                        }
                    }
                    else
                    {
                        if (isStartingCommand) { isStartingCommand = false; }
                        return SpecialCharType.NormalChar;
                    }
                }
                else
                {
                    if (isStartingCommand)
                    {
                        command += _char;
                        return SpecialCharType.CommandChar;
                    }
                }
            }
            return SpecialCharType.NormalChar;
        }

        private void CM_r_ChangeLine()
        {
            guiTarget.text += '\n';
        }
        #endregion

        #region Protected Methods
        protected override IEnumerator CM_lr_WaitAndChangeLine()
        {
            IsWaiting = true;
            yield return new WaitUntil(() => IsWaiting == false);
            guiTarget.text += '\n';
        }

        protected override IEnumerator CM_l_WaitAndPrint()
        {
            IsWaiting = true;
            yield return new WaitUntil(() => IsWaiting == false);
        }

        protected override IEnumerator CM_w_WaitAndClean()
        {
            IsWaiting = true;
            yield return new WaitUntil(() => IsWaiting == false);
            guiTarget.text = "";
        }
        #endregion 
    }
}
