using System;
using UnityEngine;

public class TypeSearcher
{
    public static Type FindType(string typeString)
    {
        Type foundType = null;
        try
        {
            foundType = Type.GetType(typeString);
        }
        catch (TypeLoadException e)
        {
            Debug.LogError(e.Message);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
        return foundType;
    }
}
