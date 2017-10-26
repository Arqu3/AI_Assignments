using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class SetTextToFloat : MonoBehaviour
{
    [Header ("Extra text")]
    [SerializeField]
    string m_BeforeValueText = "";
    [SerializeField]
    string m_AfterValueText = "";

    Text m_Text;

    private void Awake ()
    {
        m_Text = GetComponent<Text> ();
    }

    public void SetText(float value)
    {
        m_Text.text = m_BeforeValueText + value + m_AfterValueText;
    }
}
