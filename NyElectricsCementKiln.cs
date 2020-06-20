

using System;
using System.Collections.Generic;
using Eco.Gameplay.Components;
using Eco.Gameplay.Components.Auth;
using Eco.Gameplay.Housing;
using Eco.Gameplay.Items;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Skills;
using Eco.Gameplay.Systems.TextLinks;
using Eco.Gameplay.Pipes.LiquidComponents;
using Eco.Gameplay.Pipes.Gases;
using Eco.Gameplay.Property;
using Eco.Gameplay.Systems.Tooltip;
using Eco.Shared.Math;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using Eco.Shared.Utils;
using Eco.Mods.TechTree;
using Eco.Core.Plugins.Interfaces;



namespace NyElectrics
{




[Serialized]
[RequireComponent(typeof(ChimneyComponent))]
[RequireComponent(typeof(LiquidProducerComponent))]
[RequireComponent(typeof(AttachmentComponent))]
[RequireComponent(typeof(PropertyAuthComponent))]
[RequireComponent(typeof(MinimapComponent))]
[RequireComponent(typeof(LinkComponent))]
[RequireComponent(typeof(CraftingComponent))]
[RequireComponent(typeof(PowerGridComponent))]
[RequireComponent(typeof(PowerConsumptionComponent))]
[RequireComponent(typeof(HousingComponent))]
[RequireComponent(typeof(SolidGroundComponent))]
[RequireComponent(typeof(RoomRequirementsComponent))]
[RequireRoomContainment]
[RequireRoomVolume(45)]
[RequireRoomMaterialTier(1.8f, typeof(CementLavishReqTalent), typeof(CementFrugalReqTalent))]
public partial class NyElectricCementKilnObject : WorldObject, IRepresentsItem
{
  public override LocString DisplayName { get { return Localizer.DoStr("Electric Cement Kiln"); } }

  public virtual Type RepresentedItemType { get { return typeof(NyElectricCementKilnItem); } }

  protected override void Initialize()
  {

      this.GetComponent<MinimapComponent>().Initialize(Localizer.DoStr("Crafting"));
      this.GetComponent<HousingComponent>().Set(NyElectricCementKilnItem.HousingVal);
      this.GetComponent<PowerGridComponent>().Initialize(10, new ElectricPower());
      this.GetComponent<LiquidProducerComponent>().Setup(typeof(SmogItem), (int)(0.8 * 1000f), this.NamedOccupancyOffset("ChimneyOut"));
      
      this.GetComponent<PowerConsumptionComponent>().Initialize(3000f);
  }

  public override void Destroy()
  {
      base.Destroy();
  }

  static NyElectricCementKilnObject()
  {
	WorldObject.AddOccupancy<NyElectricCementKilnObject>(new List<BlockOccupancy>(){new BlockOccupancy(new Vector3i(0, 1, 0), typeof(PipeSlotBlock), new Quaternion(-0.7071068f, 0f, 0f, 0.7071068f), "ChimneyOut"),new BlockOccupancy(new Vector3i(0, 0, 0)),new BlockOccupancy(new Vector3i(1, 0, 0)),new BlockOccupancy(new Vector3i(1, 1, 0)), new BlockOccupancy(new Vector3i(2, 0, 0)),new BlockOccupancy(new Vector3i(2, 1, 0)),new BlockOccupancy(new Vector3i(3, 0, 0)),new BlockOccupancy(new Vector3i(3, 1, 0)),});
      
	/* Linked Recipes */
	CraftingComponent.AddRecipe(typeof(NyElectricCementKilnObject), Recipe.Get<Eco.Mods.TechTree.ReinforcedConcreteRecipe>());
	CraftingComponent.AddRecipe(typeof(NyElectricCementKilnObject), Recipe.Get<Eco.Mods.TechTree.ConcreteRecipe>());
	CraftingComponent.AddRecipe(typeof(NyElectricCementKilnObject), Recipe.Get<Eco.Mods.TechTree.BasaltConcreteRecipe>());
	CraftingComponent.AddRecipe(typeof(NyElectricCementKilnObject), Recipe.Get<Eco.Mods.TechTree.GneissConcreteRecipe>());
	CraftingComponent.AddRecipe(typeof(NyElectricCementKilnObject), Recipe.Get<Eco.Mods.TechTree.LimestoneConcreteRecipe>());
	CraftingComponent.AddRecipe(typeof(NyElectricCementKilnObject), Recipe.Get<Eco.Mods.TechTree.ShaleConcreteRecipe>());

  }

}


[Serialized]
public partial class NyElectricCementKilnItem : WorldObjectItem<NyElectricCementKilnObject>
{
  public override LocString DisplayName { get { return Localizer.DoStr("Electric Cement Kiln"); } }
  public override LocString DisplayDescription { get { return Localizer.DoStr("A superior replacement for the cement kiln that use electric power."); } }

  static NyElectricCementKilnItem()
  {

  }

  [TooltipChildren] public HousingValue HousingTooltip { get { return HousingVal; } }
  [TooltipChildren]
  public static HousingValue HousingVal
  {
      get
      {
          return new HousingValue()
          {
              Category = "Industrial",
              TypeForRoomLimit = "",
          };
      }
  }
}

[RequiresSkill(typeof(IndustrySkill), 0)]
public partial class NyElectricCementKilnRecipe : Recipe
{
    public NyElectricCementKilnRecipe()
    {
        this.Products = new CraftingElement[]
        {
            new CraftingElement<NyElectricCementKilnItem>(),
        };
        this.Ingredients = new CraftingElement[] {
		new CraftingElement<Eco.Mods.TechTree.CircuitItem>(1f),
		new CraftingElement<Eco.Mods.TechTree.BrickItem>(typeof(IndustrySkill), 80f, IndustrySkill.MultiplicativeStrategy, typeof(IndustryLavishResourcesTalent)),
		new CraftingElement<Eco.Mods.TechTree.SteelItem>(typeof(IndustrySkill), 50f, IndustrySkill.MultiplicativeStrategy, typeof(IndustryLavishResourcesTalent)),

	};

        this.ExperienceOnCraft = 7f;

        this.CraftMinutes = CreateCraftTimeValue(typeof(NyElectricCementKilnRecipe), Item.Get<NyElectricCementKilnItem>().UILink(), 10f, typeof(IndustrySkill), typeof(IndustryFocusedSpeedTalent), typeof(IndustryParallelSpeedTalent));
        this.Initialize(Localizer.DoStr("Electric Cement Kiln"), typeof(NyElectricCementKilnRecipe));

        CraftingComponent.AddRecipe(typeof(ElectricMachinistTableObject), this);
    }
}



}