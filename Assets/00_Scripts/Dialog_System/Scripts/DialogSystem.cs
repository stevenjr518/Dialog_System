using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Dialog_System
{
    public class DialogSystem : IDialog
    {
        private Text guiTarget = null;
        private float dialogSpeed = 0.01f;
        private MonoBehaviour gameLoop = null;
        private const char CM_CHAR_START = '[';
        private const char CM_CHAR_END = ']';
        private char lastChar;
        private string command;
        private bool isStartingCommand = false;
        public bool isWaiting = false;
        public bool isEnd = false;
        private enum SpecialCharType
        {
            StartChar,
            CommandChar,
            EndChar,
            NormalChar
        }
        public DialogSystem(string _textContent, Text _guiTarget, MonoBehaviour _mono)
        {
            SplitSlashN(_textContent);
            guiTarget = _guiTarget;
            gameLoop = _mono;
            Initialize();
        }

        private void SplitSlashN(string text) {
            string[] s = text.Split('\n');
            foreach (string c in s) {
                textContent += c;
            }
        }

        protected void Initialize()
        {
            commandMap.Add("l", () => gameLoop.StartCoroutine(CM_l_WaitAndPrint()));
            commandMap.Add("r", CM_r_ChangeLine);
            commandMap.Add("w", () => gameLoop.StartCoroutine(CM_w_WaitAndClean()));
            commandMap.Add("lr", () => gameLoop.StartCoroutine(CM_lr_WaitAndChangeLine()));
        }

        public override IEnumerator PrintTextContent() {
            guiTarget.text = "";
            for (int i = 0; i < textContent.Length; ++i) {
                SpecialCharType type = SpecialCharCheck(textContent[i]);
                if (type == SpecialCharType.NormalChar)
                {
                    guiTarget.text += textContent[i];
                    lastChar = textContent[i];
                    yield return new WaitForSeconds(dialogSpeed);
                }
                lastChar = textContent[i];
                yield return new WaitUntil(() => isWaiting == false);
            }
            isEnd = true;
        }

        private SpecialCharType SpecialCharCheck(char _char) {
            //�p�G�r���O�R�O�}�Y[����
            if (_char == CM_CHAR_START)
            {
                if (lastChar != CM_CHAR_START)
                {
                    isStartingCommand = true;
                    return SpecialCharType.StartChar;//�Y�W�@�Ӧr�����O[���ܫh���R�O�}�Y
                }
                else
                {
                    isStartingCommand = false;
                    return SpecialCharType.NormalChar;
                }
            }
            else if (_char == CM_CHAR_END)
            //�p�G�r���O�R�O����]����
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
            //�p�G�r�����O�R�O[����
            {
                if (lastChar == CM_CHAR_START)
                {
                    if (_char != CM_CHAR_END)
                    {
                        if (isStartingCommand) {
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

        protected override IEnumerator CM_lr_WaitAndChangeLine()
        {
            isWaiting = true;
            yield return new WaitUntil(()=> isWaiting == false);
            guiTarget.text += '\n';
        }

        protected override IEnumerator CM_l_WaitAndPrint()
        {
            isWaiting = true;
            yield return new WaitUntil(() => isWaiting == false);
        }

        private void CM_r_ChangeLine()
        {
            guiTarget.text += '\n';
        }

        protected override IEnumerator CM_w_WaitAndClean()
        {
            isWaiting = true;
            yield return new WaitUntil(() => isWaiting == false);
            guiTarget.text = "";
        }
    }
}
