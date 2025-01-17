﻿using COTL_API.CustomStructures;

namespace COTL_API.Helpers;

public static class TaskUtils
{
    public static Structure? GetAvailableStructureOfType<T>()
    {
        return Structure.Structures.FirstOrDefault(str =>
            str.Structure_Info.Type == CustomStructureManager.GetStructureByType<T>() &&
            str.Brain is { ReservedForTask: false });
    }
}