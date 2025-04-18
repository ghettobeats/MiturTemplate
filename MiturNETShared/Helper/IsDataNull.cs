﻿namespace MiturNetShared.Helper;
public class IsDataNull
{
    public static T Check<T>(string data)
    {
        return data == null ? default : System.Text.Json.JsonSerializer.Deserialize<T>(data);
    }
    public static T CreateInstanceIfIsNull<T>(T data) where T : new()
    {
        return data == null ? new T() : data;
    }
}
