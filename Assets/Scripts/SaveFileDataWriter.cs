using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class SaveFileDataWriter : MonoBehaviour
{
    public string saveDataDirectoryPath = "";
    public string saveFileName = "";
    
    //  Before we create a new save file, we must check ro see if one of this character slot already Exist ( Max 10 Chracter Slots)
    public bool CheckToSeeIfFileExists()
    {
        if(File.Exists(Path.Combine(saveDataDirectoryPath,saveFileName)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //  USED TO DELETE CHARACTER SAVE FILES
    public void DeleteSaveFile()
    {
        File.Delete(Path.Combine(saveDataDirectoryPath,saveFileName));
    }

    //  USED TO CREATE A SAVE FILE UPON STARTING A NEW GAME
    public void CreateNewCharacterSaveFile(CharacterSaveData characterData)
    {
        //  MAKE A PATH TO SAVE THE FILE (A LOCATION ON THE MACHINE)
        string savePath = Path.Combine(saveDataDirectoryPath, saveFileName);

        try
        {
            //  CREATE THE DIRECTORY THE FILE WILL BE WRITEN TO, IF IT DOES NOT ALREADY EXIST
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            Debug.Log("Creating Save File, At Save Path: " + savePath);


            //  SERIALIZE THE C# GAME DATA OBJECT INTO JSON
            string dataToStore = JsonUtility.ToJson(characterData, true);

            //  WRITE THE FILE TO OUR SYSTEM
            using (FileStream stream = new FileStream(savePath, FileMode.Create))
            {
                using (StreamWriter fileWriter = new StreamWriter(stream))
                {
                    fileWriter.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("ERROR WHILST TRYING TO SAVE CHARACTER DATA, GAME NOT SAVED" + savePath + "\n" + e);
        }
    }
    
    //  USER TO LOAD A SAVE FILE UPON LOADING A PREVIOUS GAME
    public CharacterSaveData LoadSaveFile()
    {
        CharacterSaveData characterData = null;
        
        //  MAKE A PATH TO SAVE THE FILE (A LOCATION ON THE MACHINE)
        string loadPath = Path.Combine(saveDataDirectoryPath, saveFileName);

        if (File.Exists(loadPath))
        {
            try
            {
                string dataToLoad = "";
                
                using (FileStream stream = new FileStream(loadPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                
                //  DESERIALIZE THE DATA FROM JSON BACK TO UNITY C#
                characterData = JsonUtility.FromJson<CharacterSaveData>(dataToLoad);
            }
            catch (Exception e)
            {
                
            }
        }
        
        return characterData;
    }
}
