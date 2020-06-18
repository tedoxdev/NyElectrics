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
using Eco.Gameplay.Systems.Tooltip;
using Eco.Shared.Math;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using Eco.Mods.TechTree;
using Eco.Core.Plugins.Interfaces;
using Eco.Shared.Utils;

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
[RequireComponent(typeof(LiquidConverterComponent))]
public partial class NyElectricBlastFurnaceObject : WorldObject, IRepresentsItem
{
  public override LocString DisplayName { get { return Localizer.DoStr("Electric Blast Furnace"); } }

  public virtual Type RepresentedItemType { get { return typeof(NyElectricBlastFurnaceItem); } }

  protected override void Initialize()
  {

      this.GetComponent<MinimapComponent>().Initialize(Localizer.DoStr("Crafting"));
      this.GetComponent<HousingComponent>().Set(BlastFurnaceItem.HousingVal);
      this.GetComponent<PowerGridComponent>().Initialize(10, new ElectricPower());
      this.GetComponent<LiquidProducerComponent>().Setup(typeof(SmogItem), (int)(1 * 1000f), this.NamedOccupancyOffset("ChimneyOut"));
      this.GetComponent<LiquidConverterComponent>().Setup(typeof(WaterItem), typeof(SewageItem), this.NamedOccupancyOffset("WaterInputPort"), this.NamedOccupancyOffset("SewageOutputPort"), 300, 0.9f);
      this.GetComponent<PowerConsumptionComponent>().Initialize(3000f);
  }

  public override void Destroy()
  {
      base.Destroy();
  }

  static NyElectricBlastFurnaceObject()
  {
      WorldObject.AddOccupancy<NyElectricBlastFurnaceObject>(new List<BlockOccupancy>(){
          new BlockOccupancy(new Vector3i(1, 4, 1), typeof(PipeSlotBlock), new Quaternion(-0.7071071f, 2.634177E-07f, 2.634179E-07f, 0.7071065f), "ChimneyOut"),
          new BlockOccupancy(new Vector3i(2, 0, 2), typeof(PipeSlotBlock), new Quaternion(0f, 0f, 0f, 1f), "WaterInputPort"),
          new BlockOccupancy(new Vector3i(0, 0, 0), typeof(PipeSlotBlock), new Quaternion(0f, -0.7071068f, 0f, 0.7071068f), "SewageOutputPort"),
          new BlockOccupancy(new Vector3i(0, 0, 1)),
          new BlockOccupancy(new Vector3i(0, 0, 2)),
          new BlockOccupancy(new Vector3i(0, 1, 0)),
          new BlockOccupancy(new Vector3i(0, 1, 1)),
          new BlockOccupancy(new Vector3i(0, 1, 2)),
          new BlockOccupancy(new Vector3i(0, 2, 0)),
          new BlockOccupancy(new Vector3i(0, 2, 1)),
          new BlockOccupancy(new Vector3i(0, 2, 2)),
          new BlockOccupancy(new Vector3i(0, 3, 0)),
          new BlockOccupancy(new Vector3i(0, 3, 1)),
          new BlockOccupancy(new Vector3i(0, 3, 2)),
          new BlockOccupancy(new Vector3i(0, 4, 0)),
          new BlockOccupancy(new Vector3i(0, 4, 1)),
          new BlockOccupancy(new Vector3i(0, 4, 2)),
          new BlockOccupancy(new Vector3i(1, 0, 0)),
          new BlockOccupancy(new Vector3i(1, 0, 1)),
          new BlockOccupancy(new Vector3i(1, 0, 2)),
          new BlockOccupancy(new Vector3i(1, 1, 0)),
          new BlockOccupancy(new Vector3i(1, 1, 1)),
          new BlockOccupancy(new Vector3i(1, 1, 2)),
          new BlockOccupancy(new Vector3i(1, 2, 0)),
          new BlockOccupancy(new Vector3i(1, 2, 1)),
          new BlockOccupancy(new Vector3i(1, 2, 2)),
          new BlockOccupancy(new Vector3i(1, 3, 0)),
          new BlockOccupancy(new Vector3i(1, 3, 1)),
          new BlockOccupancy(new Vector3i(1, 3, 2)),
          new BlockOccupancy(new Vector3i(1, 4, 0)),
          new BlockOccupancy(new Vector3i(1, 4, 2)),
          new BlockOccupancy(new Vector3i(2, 0, 0)),
          new BlockOccupancy(new Vector3i(2, 0, 1)),
          new BlockOccupancy(new Vector3i(2, 1, 0)),
          new BlockOccupancy(new Vector3i(2, 1, 1)),
          new BlockOccupancy(new Vector3i(2, 1, 2)),
          new BlockOccupancy(new Vector3i(2, 2, 0)),
          new BlockOccupancy(new Vector3i(2, 2, 1)),
          new BlockOccupancy(new Vector3i(2, 2, 2)),
          new BlockOccupancy(new Vector3i(2, 3, 0)),
          new BlockOccupancy(new Vector3i(2, 3, 1)),
          new BlockOccupancy(new Vector3i(2, 3, 2)),
          new BlockOccupancy(new Vector3i(2, 4, 0)),
          new BlockOccupancy(new Vector3i(2, 4, 1)),
          new BlockOccupancy(new Vector3i(2, 4, 2)),
      });
  }

}



[Serialized]
public partial class NyElectricBlastFurnaceItem : WorldObjectItem<NyElectricBlastFurnaceObject>
{
  public override LocString DisplayName { get { return Localizer.DoStr("Electric Blast Furnace"); } }
  public override LocString DisplayDescription { get { return Localizer.DoStr("A superior replacement for the blast furnace that use electric power."); } }

  static NyElectricBlastFurnaceItem()
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
public partial class NyElectricBlastFurnaceRecipe : Recipe
{
  public NyElectricBlastFurnaceRecipe()
  {
      this.Products = new CraftingElement[]
      {
          new CraftingElement<NyElectricBlastFurnaceItem>(),
      };
      this.Ingredients = new CraftingElement[] {
			new CraftingElement<CircuitItem>(1f),
			new CraftingElement<SteelItem>(typeof(IndustrySkill), 40f, IndustrySkill.MultiplicativeStrategy, typeof(IndustryLavishResourcesTalent)),
			new CraftingElement<ScrewsItem>(typeof(IndustrySkill), 40f, IndustrySkill.MultiplicativeStrategy),

		};

      this.ExperienceOnCraft = 7f;

      this.CraftMinutes = CreateCraftTimeValue(typeof(NyElectricBlastFurnaceRecipe), Item.Get<NyElectricBlastFurnaceItem>().UILink(), 10f, typeof(IndustrySkill), typeof(IndustryFocusedSpeedTalent), typeof(IndustryParallelSpeedTalent));
      this.Initialize(Localizer.DoStr("Electric Blast Furnace"), typeof(NyElectricBlastFurnaceRecipe));

      CraftingComponent.AddRecipe(typeof(ElectricMachinistTableObject), this);
  }
}

}