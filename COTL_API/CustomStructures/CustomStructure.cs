﻿using Lamb.UI.BuildMenu;
using COTL_API.Helpers;
using COTL_API.Prefabs;
using UnityEngine;
using I2.Loc;

namespace COTL_API.CustomStructures;

public abstract class CustomStructure : StructureBrain
{
    public abstract string InternalName { get; }

    internal TYPES StructureType;
    internal string ModPrefix = "";

    public virtual Sprite Sprite { get; } =
        TextureHelper.CreateSpriteFromPath(PluginPaths.ResolveAssetPath("placeholder.png"));

    public virtual FollowerCategory.Category Category => FollowerCategory.Category.Misc;

    public virtual string PrefabPath => CustomPrefabManager.GetOrCreateBuildingPrefab(this);
    public virtual int BuildDurationMinutes => 30;

    public virtual Type? Interaction => null;

    public virtual Categories StructureCategories => Categories.CULT;

    public virtual TypeAndPlacementObjects.Tier Tier => TypeAndPlacementObjects.Tier.Zero;

    internal TypeAndPlacementObject GetTypeAndPlacementObject()
    {
        TypeAndPlacementObject tpo = new()
        {
            IconImage = Sprite,
            Category = StructureCategories,
            PlacementObject = CustomPrefabManager.CreatePlacementObjectFor(this),
            Type = StructureType,
            Tier = Tier,
        };
        return tpo;
    }

    internal StructuresData StructuresData => new()
    {
        PrefabPath = PrefabPath,
        Bounds = Bounds,
        TILE_WIDTH = Bounds.x,
        TILE_HEIGHT = Bounds.y
    };

    public virtual Vector2Int Bounds => new(1, 1);

    public virtual List<StructuresData.ItemCost> Cost => new();

    internal string GetLocalizedNameStatic()
    {
        return GetLocalizedName();
    }

    internal string LocalizedName()
    {
        return GetLocalizedNameStatic();
    }

    internal string LocalizedDescription()
    {
        return GetLocalizedDescription();
    }

    public virtual string LocalizedPros()
    {
        return LocalizationManager.GetTranslation($"Structures/{StructureType}/Pros");
    }

    public virtual string LocalizedCons()
    {
        return LocalizationManager.GetTranslation($"Structures/{StructureType}/Cons");
    }

    public virtual string GetLocalizedName()
    {
        return LocalizationManager.GetTranslation($"Structures/{StructureType}");
    }

    public virtual string GetLocalizedName(bool plural, bool withArticle, bool definite)
    {
        var article = definite ? "/Definite" : "/Indefinite";

        var text = $"Structures/{StructureType}{(plural ? "/Plural" : "")}{(!withArticle ? "" : article)}";
        return LocalizationManager.GetTranslation(text);
    }

    public virtual string GetLocalizedDescription()
    {
        return LocalizationManager.GetTranslation($"Structures/{StructureType}/Description");
    }

    public virtual string GetLocalizedLore()
    {
        return LocalizationManager.GetTranslation("Structures/" + StructureType + "/Lore");
    }

    public virtual int GetResearchCost()
    {
        return 5;
    }

    public virtual bool RequiresTempleToBuild()
    {
        return true;
    }

    public virtual bool GetBuildOnlyOne()
    {
        return false;
    }

    public virtual string GetBuildSfx()
    {
        return "event:/building/finished_wood";
    }

    public virtual bool HiddenUntilUnlocked()
    {
        return false;
    }

    public virtual bool CanBeFlipped()
    {
        return true;
    }
}