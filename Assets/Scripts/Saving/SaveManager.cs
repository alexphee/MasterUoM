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
            Save();
        }
    }

    private void Save()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + "SaveTest.dat", FileMode.OpenOrCreate); //creates the file in hard drive. if exist open it, if doesnt then create it //THIS LINE DOESNT SAVE

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
        data.MyPlayerData = new PlayerData(Player.MyInstance.MyLevel);
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
    }
}
