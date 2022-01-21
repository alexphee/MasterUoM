using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(Application.persistentDataPath);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Save();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Load();
        }
    }

    private void Save()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + "SaveTest.dat", FileMode.Create); //creates the file in hard drive //THIS LINE DOESNT SAVE //there is also OpenOrCreate but it appends on old files

            SaveData data = new SaveData();
            SavePlayer(data);
            bf.Serialize(file, data); //this line actually saves the game by serializing the data
            file.Close();
        }
        catch (System.Exception)
        {

            //this is for handling errors
        }
    }

    private void SavePlayer(SaveData data)
    {
        data.MyPlayerData = new PlayerData(Player.MyInstance.MyLevel, Player.MyInstance.MyXP.MyCurrentValue, Player.MyInstance.MyXP.MyMaxValue,
                                            Player.MyInstance.MyHealth.MyCurrentValue, Player.MyInstance.MyHealth.MyMaxValue, Player.MyInstance.MyMana.MyCurrentValue,
                                            Player.MyInstance.MyMana.MyMaxValue, Player.MyInstance.transform.position);
    }

    private void Load()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + "SaveTest.dat", FileMode.Open); //finds teh saved file and opens it

            SaveData data = (SaveData)bf.Deserialize(file); //deserializes data and casts it as SaveData so i can load it
            
            file.Close();

            LoadPlayer(data);
        }
        catch (System.Exception)
        {

            //this is for handling errors
        }
    }

    private void LoadPlayer(SaveData data)
    {
        Player.MyInstance.MyLevel = data.MyPlayerData.MyLevel;
        Player.MyInstance.UpdateLevel();
        Player.MyInstance.MyHealth.Initialize(data.MyPlayerData.MyHealth, data.MyPlayerData.MyMaxHealth);
        Player.MyInstance.MyMana.Initialize(data.MyPlayerData.MyMana, data.MyPlayerData.MyMaxMana);
        Player.MyInstance.MyXP.Initialize(data.MyPlayerData.MyXP, data.MyPlayerData.MyMaxXP);
        Player.MyInstance.transform.position = new Vector2(data.MyPlayerData.MyX, data.MyPlayerData.MyY);
    }
}
