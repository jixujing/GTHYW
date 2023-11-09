using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    DataDefinition GetDataID();
    void RegisterSaveData()
    {
        DataManager.instance.RegisterDataSave(this);
    }
    void UnRegisterSaveData()
    {
        DataManager.instance.UnRegisterDataSave(this);
    }

    void GetSaveData(Data data);

    void LoadData(Data data);
}
