﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Specialized;
using System.Security.Cryptography.X509Certificates;
using System.ComponentModel;
using System;

public class Inventory : MonoBehaviour
{
    public DataBase data;
    public List<ItemInventory> items = new List<ItemInventory>();
    public GameObject GameObjectShow;
    public GameObject InventoryMainObj;
    public int maxcount;
    public Camera cam;
    public EventSystem es;
    public int currentId;
    public ItemInventory currentItem;
    public RectTransform movingObject;
    public Vector3 offset;
    public GameObject backGround;
    public void Start()
    {
        backGround.SetActive(false);
        if (items.Count == 0)
        {
            AddGraphics();
        }
        for (int i = 0; i < maxcount; i++)
        {
            AddItem(i, data.items[UnityEngine.Random.Range(0, data.items.Count)],UnityEngine.Random.Range(1, 99));
        }
        UpdateInventory();
    }
    public void Update()
    {
        if(currentId !=-1)
        {
            MoveObject();
        }
        if(Input.GetKeyDown(KeyCode.I))
        {
            backGround.SetActive(!backGround.activeSelf);
            if(backGround.activeSelf)
            {
                UpdateInventory();
            }
        }
    }
    public void SerchForSameItem(Item item, int count)
    {
        for(int i=0; i<maxcount;i++)
        {
            if(items[i].id==item.id)
            {
                if(items[0].count<10)
                {
                    items[i].count += count;
                    if(items[i].count>10)
                    {
                        count = items[i].count - 10;
                    }
                }
            }    
        }
    }
    public void AddItem(int id, Item item, int count)
    {
        items[id].id = item.id;
        items[id].count = count;
        items[id].itemGameObj.GetComponent<Image>().sprite = item.img;
        if(count > 1 && item.id !=0 )
        {
            items[id].itemGameObj.GetComponentInChildren<Text>().text = count.ToString();
        }
        else
        {
            items[id].itemGameObj.GetComponentInChildren<Text>().text = "";
        }
    }
    public void AddInventoryItem(int id, ItemInventory invItem)
    {
        items[id].id = invItem.id;
        items[id].count = invItem.count;
        items[id].itemGameObj.GetComponent<Image>().sprite = data.items[invItem.id].img; 
        if (invItem.count > 1 && invItem.id != 0)
        {
            items[id].itemGameObj.GetComponentInChildren<Text>().text = invItem.count.ToString();
        }
        else
        {
            items[id].itemGameObj.GetComponentInChildren<Text>().text = "";
        }
    }
    public void AddGraphics()
    {
    for(int i=0; i<maxcount;i++)
        {
            GameObject newItem = Instantiate(GameObjectShow, InventoryMainObj.transform) as GameObject;
            newItem.name = i.ToString();
            ItemInventory ii = new ItemInventory();
            ii.itemGameObj = newItem;
            RectTransform rt = newItem.GetComponent<RectTransform>();
            rt.localPosition = new Vector3(0, 0, 0);
            rt.localScale = new Vector3(1, 1, 1);
            newItem.GetComponentInChildren<RectTransform>().localScale = new Vector3(1, 1, 1);
            Button tempButton = newItem.GetComponent<Button>();
            tempButton.onClick.AddListener(delegate { SelectObject(); });
            items.Add(ii);
        }

    }
    public void UpdateInventory()
    {
        for(int i=0;i<maxcount;i++)
        {
            if((items[i].id !=0) && (items[i].count > 1))
            {
                items[i].itemGameObj.GetComponentInChildren<Text>().text = items[i].count.ToString();
            }
            else 
            {
                items[i].itemGameObj.GetComponentInChildren<Text>().text = "";
            }
            items[i].itemGameObj.GetComponent<Image>().sprite = data.items[items[i].id].img;
        }
    }
    public void SelectObject()
    {
        if(currentId==-1)
        {
            currentId =int.Parse(es.currentSelectedGameObject.name);
            currentItem = CopyInventoryItem(items[currentId]);
            movingObject.gameObject.SetActive(true);
            movingObject.GetComponent<Image>().sprite = data.items[currentItem.id].img;
            AddItem(currentId, data.items[0], 0);
        }
        else
        {
            AddInventoryItem(currentId, items[int.Parse(es.currentSelectedGameObject.name)]);
            AddInventoryItem(int.Parse(es.currentSelectedGameObject.name), currentItem);
            currentId = -1;
            movingObject.gameObject.SetActive(false);
        }
    }
    public void MoveObject()
    {
        Vector3 pos = Input.mousePosition + offset;
        pos.z = InventoryMainObj.GetComponent<RectTransform>().position.z;
        movingObject.position = cam.ScreenToWorldPoint(pos);
    }
    public ItemInventory CopyInventoryItem (ItemInventory old)
    {
        ItemInventory New = new ItemInventory();
        New.id = old.id;
        New.itemGameObj = old.itemGameObj;
        New.count = old.count;
        return New; 
    }
}
[System.Serializable]
public class ItemInventory 
{
    public int id;
    public GameObject itemGameObj;
    public int count;

    
}