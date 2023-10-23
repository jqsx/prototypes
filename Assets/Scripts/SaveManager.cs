using JQUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class SaveManager
{
    public static GameData gameData;
    public static FileSave save = new FileSave(Application.persistentDataPath, "GameData.data");

    public static bool wasThereAnError = false;

    public static void FetchData()
    {
        try
        {
            gameData = save.Load();
        } catch (Exception e)
        {
            wasThereAnError = true;
            if (UIController.instance != null)
            {
                UIController.instance.LoadError(e.Message);
            }
        }

        if (gameData != null)
        {
            GAMEINITIALIZER.globalGameLevel = gameData.globalGameLevel;
            Player.setLoadedPosition = true;
            Player.loadedPosition = gameData.playerPosition;
            InventoryController.inventory = gameData.playerInventory;
        }
        else
        {
            Debug.Log("No Game Data");
        }
    }

    public static void SaveData()
    {
        gameData = new GameData();
        if (PlayerController.instance != null)
        {
            gameData.playerPosition = PlayerController.instance.transform.position;
        }
        gameData.playerInventory = InventoryController.inventory;
        gameData.globalGameLevel = GAMEINITIALIZER.globalGameLevel;
        gameData.playerArmorInventory = new Inventory(1);

        try
        {
            save.Save(gameData);
        } 
        catch (Exception e)
        {
            wasThereAnError = true;
            if (UIController.instance != null)
            {
                UIController.instance.SaveError(e.Message);
            }
        }
    }

    public static void resetSave()
    {
        gameData = new GameData();
        if (PlayerController.instance != null)
        {
            gameData.playerPosition = Vector2.zero;
        }
        gameData.playerInventory = new Inventory(15);
        gameData.globalGameLevel = 1;
        gameData.playerArmorInventory = new Inventory(5);

        try
        {
            save.Save(gameData);
        }
        catch (Exception e)
        {
            wasThereAnError = true;
            if (UIController.instance != null)
            {
                UIController.instance.SaveError(e.Message);
            }
        }
    }
}
