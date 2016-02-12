using UnityEngine;
using System.Collections;

public class TowerMenuController : MonoBehaviour
{
    TowerController _towerController;

    [SerializeField]
    tk2dButton _upgradeButton;

    [SerializeField]
    tk2dButton _sellButton;
     
    private void OnEnable()
    {
        _upgradeButton.ButtonPressedEvent += OnTowerUpgrade;
    }

    private void OnDisable()
    {
        _upgradeButton.ButtonPressedEvent -= OnTowerUpgrade;
    }

    public void SetTower(TowerController tower)
    {
        _towerController = tower;
    }

    public void OnTowerUpgrade(tk2dButton sourse)
    {
        _towerController.Upgrade();
    }

}
