using System;
using System.Collections.Generic;
using Godot;
using Array = Godot.Collections.Array;

namespace FusionGodot;

#pragma warning disable CS8632

/// <summary>Small conversion helpers shared by the wrappers and the <c>Fusion</c> facade.</summary>
internal static class VariantExt
{
    /// <summary>Map a Godot <see cref="Array"/> of objects into typed wrappers via a factory.</summary>
    internal static List<T> ToWrappers<T>(this Variant arrayVariant, Func<GodotObject, T> factory)
    {
        var result = new List<T>();
        Array arr = arrayVariant.AsGodotArray();
        foreach (Variant item in arr)
        {
            GodotObject? obj = item.AsGodotObject();
            if (obj != null)
            {
                result.Add(factory(obj));
            }
        }
        return result;
    }

    /// <summary>Wrap a Variant holding a GodotObject, returning null when the object is null.</summary>
    internal static T? AsWrapper<T>(this Variant variant, Func<GodotObject, T> factory) where T : class
    {
        GodotObject? obj = variant.AsGodotObject();
        return obj == null ? null : factory(obj);
    }
}
