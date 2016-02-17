using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartButtonController : MonoBehaviour
{
    private Button _startButton;
    Master_Instance _masterInstance;

    private void Awake()
    {
        _startButton = GetComponent<Button>();
        _masterInstance = FindObjectOfType<Master_Instance>();
    }

    private void OnEnable()
    {
        _startButton.onClick.AddListener(_masterInstance.OnPlay);
    }

    private void OnDisable()
    {
        _startButton.onClick.RemoveListener(_masterInstance.OnPlay);
    }
}
