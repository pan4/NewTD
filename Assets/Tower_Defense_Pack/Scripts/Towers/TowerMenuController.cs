using UnityEngine;
using System.Collections;

public class TowerMenuController : MonoBehaviour
{
    TowerController _towerController;

    [SerializeField]
    tk2dButton _upgradeButton;

    [SerializeField]
    tk2dButton _sellButton;

    [SerializeField]
    GameObject _flagButton;

    [SerializeField]
    SpriteRenderer _upgradeLabel;

    private void OnEnable()
    {
        _upgradeButton.ButtonDownEvent += OnTowerUpgrade;
        _sellButton.ButtonDownEvent += OnTowerSell;

    }

    private void OnDisable()
    {
        _upgradeButton.ButtonDownEvent -= OnTowerUpgrade;
        _sellButton.ButtonDownEvent -= OnTowerSell;
    }

    public void SetTower(TowerController tower)
    {
        _towerController = tower;
        if (_towerController.Level == TowerController.MAX_TOWER_LEVEL)
        {
            SpriteRenderer sr = _upgradeButton.GetComponent<SpriteRenderer>();
            SetDisabledView(sr);
            SetDisabledView(_upgradeLabel);
            _upgradeButton.enabled = false;
        }

        if (_towerController is KT_Controller || _towerController is CannonTowerController)
            _flagButton.SetActive(true);
    }

    private void SetDisabledView(SpriteRenderer spriteRenderer)
    {
        Color color = spriteRenderer.color;
        color.a = 0.334f;
        spriteRenderer.color = color;
    }

    public void OnTowerUpgrade(tk2dButton sourse)
    {
        _towerController.Upgrade();
    }

    public void OnTowerSell(tk2dButton sourse)
    {
        _towerController.Sell();
    }

    public void OnTowerFlag(tk2dButton sourse)
    {
    }

}
