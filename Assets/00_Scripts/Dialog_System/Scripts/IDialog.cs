using System.Collections;
using System.Collections.Generic;
using System;

namespace Dialog_System
{
    public abstract class IDialog 
    {
        protected string textContent;
        protected Dictionary<string, Action> commandMap = new Dictionary<string, Action>();

        protected abstract IEnumerator CM_l_WaitAndPrint();
        protected abstract IEnumerator CM_w_WaitAndClean();
        protected abstract IEnumerator CM_lr_WaitAndChangeLine();
        public abstract IEnumerator PrintTextContent();
    }
}