using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextContent
{
    public string Title;
    public string Description;
    public string ObjectStatus;
    public TextContent()
    {
        Title = "";
        Description = "";
        ObjectStatus = "";
    }

    public TextContent(string title,string description,string objectStatus)
    {
        Title = title;
        Description = description;
        ObjectStatus = objectStatus;
    }
    
}