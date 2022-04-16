using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{

    [SerializeField] private Image FillImage;

    private RectTransform  rectT;



    private void Awake()
    {
        rectT = GetComponent<RectTransform>();
    }
    void onPlayerHPChanged(float currentHP , float maxHP)
    {
        FillImage.fillAmount = currentHP / maxHP;
        Debug.Log("current = " + currentHP + " \n MaxHP = " + maxHP);
    }

    void onPlayerPosChanged(Vector3 wordlPos)
    {
        rectT.position = Camera.main.WorldToScreenPoint(wordlPos);
    }

    private void OnEnable()
    {
        EventManager.OnPlayerHPChanged += onPlayerHPChanged;
        EventManager.OnPlayerPosChanged += onPlayerPosChanged;
    }
    private void OnDisable()
    {
        EventManager.OnPlayerHPChanged -= onPlayerHPChanged;
        EventManager.OnPlayerPosChanged -= onPlayerPosChanged;
    }

     
}
