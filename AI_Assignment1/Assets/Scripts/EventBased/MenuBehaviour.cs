using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AI_Assignments.EventBased
{
    public interface MenuHandler : IEventSystemHandler
    {
        void OnElementSelected (SelectEvent sEvent);
        void OnElementDeselected (SelectEvent sEvent);
        void OnElementPressed (PressEvent pEvent);
    }

    public class SelectEvent
    {
        public enum Type
        {
            SELECT,
            DESELECT
        }

        BaseUI m_Element;
        Type m_Type;

        public SelectEvent (BaseUI element, Type type)
        {
            m_Element = element;
            m_Type = type;
        }

        public BaseUI Element
        {
            get { return m_Element; }
        }

        public Type SType
        {
            get { return m_Type; }
        }

        public string Output
        {
            get { return "Select event triggered, type: " + SType.ToString() + " by element: " + Element.ToString(); }
        }
    }

    public class PressEvent
    {
        BaseUI m_Element;

        public PressEvent (BaseUI element)
        {
            m_Element = element;
        }

        public BaseUI Element
        {
            get { return m_Element; }
        }

        public string Output
        {
            get { return "Press event triggered by element: " + Element.ToString(); }
        }
    }

    public class MenuBehaviour : MonoBehaviour, MenuHandler
    {
        [SerializeField]
        Text m_OutputText;

        void Start()
        {

        }

        public void OnElementDeselected ( SelectEvent sEvent )
        {
            sEvent.Element.SetColor (Color.white);
            sEvent.Element.SetAnimBool ("Hovered", false);
            sEvent.Element.SetText ("Button");
            AddOutput (sEvent.Output);
        }

        public void OnElementPressed ( PressEvent pEvent )
        {
            pEvent.Element.SetAnimTrigger ("Pressed");
            AddOutput (pEvent.Output);
        }

        public void OnElementSelected ( SelectEvent sEvent )
        {
            sEvent.Element.SetColor (Color.green);
            sEvent.Element.SetAnimBool ("Hovered", true);
            sEvent.Element.SetText ("Hovered");
            AddOutput (sEvent.Output);
        }

        void AddOutput(string text)
        {
            if ( m_OutputText.text.Length > 1500 ) m_OutputText.text = "";

            m_OutputText.text += text + "\n\n";
        }
    }
}