using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private Stack<Menu> menuStack = new Stack<Menu>();

    public static MenuManager Instance { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public void OpenMenu(Menu instance)
    {

 
        Menu destroyedMenu = null;

        if (menuStack.Count > 0)
        {
             if (instance.DisableMenusUnderneath)
            {
                foreach (var menu in menuStack)
                {
                    menu.gameObject.SetActive(false);
                    if (menu.DestroyWhenClosed && menuStack.Peek() == menu)
                        destroyedMenu = menu;

                    if (menu.DisableMenusUnderneath)
                        break;
                }
            }

            if (destroyedMenu != null)
                menuStack.Pop();

            //TODO: sorting order can be changed here.
        }

        menuStack.Push(instance);
    }

    public void CloseMenu(Menu instance)
    {
        if (menuStack.Count == 0)
        {
            Debug.LogErrorFormat("{0} cannot be closed because the stack is empty.", instance.GetType());
            return;
        }

        if (menuStack.Peek() != instance)
        {
            Debug.LogErrorFormat("{0} cannot be closed because it is not on top of the stack.", instance.GetType());
            return;
        }

        CloseTopMenu();
    }

    public void CloseTopMenu()
    {
        var instance = menuStack.Pop();

        if (instance.DestroyWhenClosed)
            Destroy(instance.gameObject);
        else
            instance.gameObject.SetActive(false);

        foreach (var menu in menuStack)
        {
            menu.gameObject.SetActive(true);
            if (menu.DisableMenusUnderneath)
                break;
        }
    }

    public void CreateInstance<T>() where T : Menu
    {
         var prefab = GetPrefab<T>();
         var menu = prefab.GetComponent<Menu>();
         if (menu.KeepInSafeArea)
            Instantiate(prefab, transform);
        else
            Instantiate(prefab, transform.parent);
     }

    private MenuManagerSettings GetMenuManagerSettings()
    {
        return Resources.Load<MenuManagerSettings>("MenuManager/MenuSettings");
    }

    private T GetPrefab<T>() where T : Menu
    {
        var settings = GetMenuManagerSettings();
        if (settings != null)
        {
            foreach (var menu in settings.Menus)
            {
                if (menu.GetType() == (typeof(T)))
                    return menu as T;
            }
            throw new MissingReferenceException("Prefab not found for type " + typeof(T));
        }
        else
        {
            throw new MissingComponentException("MenuManagerSettings could not found.");
        }
    }
}
