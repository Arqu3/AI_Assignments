using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AI_Assignments.EventBased
{
    public class Button : BaseUI, IPointerEnterHandler, IPointerExitHandler
    {
        #region Private fields

        MenuBehaviour m_Menu;
        Image m_Image;
        Text m_Text;
        Animator m_Anim;

        bool m_Selected = false;
        bool m_CanPress = true;

        #endregion

        void Start ()
        {
            m_Menu = FindObjectOfType<MenuBehaviour> ();
            m_Image = GetComponent<Image> ();
            m_Text = GetComponentInChildren<Text> ();
            m_Anim = GetComponent<Animator> ();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if ( m_Selected && m_CanPress ) OnPress ();
            }
        }

        public override void SetText(string text)
        {
            m_Text.text = text;
        }

        public override void SetColor(Color color)
        {
            m_Image.color = color;
        }

        public void OnPointerEnter(PointerEventData data)
        {
            OnSelect ();
        }

        public void OnPointerExit(PointerEventData data)
        {
            OnDeselect ();
        }

        public override void OnDeselect ()
        {
            m_Selected = false;
            m_Menu.OnElementDeselected (new SelectEvent (this, SelectEvent.Type.DESELECT));
        }

        public override void OnPress ()
        {
            m_Menu.OnElementPressed (new PressEvent (this));
            StartCoroutine (Select (0.1f));
        }

        public override void OnSelect ()
        {
            m_Selected = true;
            m_Menu.OnElementSelected (new SelectEvent(this, SelectEvent.Type.SELECT));
        }

        IEnumerator Select(float time)
        {
            m_CanPress = false;
            m_Image.color = Color.red;

            yield return new WaitForSeconds (time);

            m_Image.color = m_Selected ? Color.green : Color.white;
            m_CanPress = true;
        }

        public override void SetAnimBool ( string name, bool value )
        {
            m_Anim.SetBool (name, value);
        }

        public override void SetAnimTrigger ( string name )
        {
            m_Anim.SetTrigger (name);
        }
    }
}