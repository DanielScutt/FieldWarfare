using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickup
{
    void PickupItem(GameObject item,Transform[] itemPostions);
    void DropItem();
    void SetCanPickup(bool CanPickup);
    bool GetCanPickup();
    string GetItemName();
    string GetItemAmount();
    string GetItemControls();
    void ChangeItemPosition(Transform pos);
}

public interface ITurret
{
    void PutOnHolder(GameObject holder, Transform position);
    void TurnOn();
    void TurnOff();
}

public interface ISeed
{
    string GetCropType();
    void UpdateSeedAmount();
}

public interface ICrop
{
    void Grow();
    float GetPrice();
}

public interface IWeapon
{
    void Fire();
    void Reload();
}

public interface IAmmo
{
    void UsedAmmo(int remainingAmmo);
    int GetAmmo();
}

public interface IUpgrade
{
    float UpgradeStat(float currentStat);
}