using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI_Assignments.EventBased
{
    public abstract class BaseUI : MonoBehaviour
    {
        public abstract void OnSelect ();
        public abstract void OnDeselect ();
        public abstract void OnPress ();

        public abstract void SetText ( string text );
        public abstract void SetColor ( Color color );
    }
}