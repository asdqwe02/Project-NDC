using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TestButtonClick : MonoBehaviour
{
    public TextMeshProUGUI inputText;
    [SerializeField] Button button;
    [SerializeField] TextMeshProUGUI textMesh;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => { OnClickButton(); });
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnClickButton()
    {
        textMesh.text = inputText.text;
    }
}
