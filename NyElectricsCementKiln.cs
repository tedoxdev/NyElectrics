

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Eco.Core.Items;
using Eco.Gameplay.Blocks;
using Eco.Gameplay.Components;
using Eco.Gameplay.Components.Auth;
using Eco.Gameplay.DynamicValues;
using Eco.Gameplay.Economy;
using Eco.Gameplay.Housing;
using Eco.Gameplay.Interactions;
using Eco.Gameplay.Items;
using Eco.Gameplay.Modules;
using Eco.Gameplay.Minimap;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Players;
using Eco.Gameplay.Property;
using Eco.Gameplay.Skills;
using Eco.Gameplay.Systems.TextLinks;
using Eco.Gameplay.Pipes.LiquidComponents;
using Eco.Gameplay.Pipes.Gases;
using Eco.Gameplay.Systems.Tooltip;
using Eco.Shared;
using Eco.Shared.Math;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using Eco.Shared.Utils;
using Eco.Shared.View;
using Eco.Shared.Items;
using Eco.Gameplay.Pipes;
using Eco.World.Blocks;
using Eco.Gameplay.Housing.PropertyValues;
using static Eco.Gameplay.Housing.PropertyValues.HomeFurnishingValue;
using Eco.Mods.TechTree;



namespace NyElectrics
{

    [Serialized]
    [RequireComponent(typeof(AirPollutionComponent))]
    [RequireComponent(typeof(ChimneyComponent))]
    [RequireComponent(typeof(OnOffComponent))]
    [RequireComponent(typeof(LiquidProducerComponent))]
    [RequireComponent(typeof(AttachmentComponent))]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(MinimapComponent))]
    [RequireComponent(typeof(LinkComponent))]
    [RequireComponent(typeof(CraftingComponent))]
    [RequireComponent(typeof(PowerGridComponent))]
    [RequireComponent(typeof(PowerConsumptionComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(SolidAttachedSurfaceRequirementComponent))]
    [RequireComponent(typeof(PluginModulesComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireRoomContainment]
    [RequireRoomVolume(45)]
    [RequireRoomMaterialTier(1.8f, typeof(MasonryLavishReqTalent), typeof(MasonryFrugalReqTalent))]
    public partial class NyElectricCementKilnObject : WorldObject, IRepresentsItem
    {
        public override LocString DisplayName { get { return Localizer.DoStr("Electric Cement Kiln"); } }

        public virtual Type RepresentedItemType { get { return typeof(NyElectricCementKilnItem); } }

        protected override void Initialize()
        {

            this.GetComponent<MinimapComponent>().Initialize(Localizer.DoStr("Crafting"));
            this.GetComponent<PowerGridComponent>().Initialize(10, new ElectricPower());
            this.GetComponent<PowerConsumptionComponent>().Initialize(3000f);
            this.GetComponent<HousingComponent>().HomeValue = NyElectricCementKilnItem.homeValue;

            this.GetComponent<LiquidProducerComponent>().Setup(typeof(SmogItem), (int)(0.8 * 1000f), this.GetOccupancyType(BlockOccupancyType.ChimneyOut));
        }

        public override void Destroy()
        {
            base.Destroy();
        }

        static NyElectricCementKilnObject()
        {
            WorldObject.AddOccupancy<NyElectricCementKilnObject>(new List<BlockOccupancy>(){new BlockOccupancy(new Vector3i(0, 1, 0), typeof(PipeSlotBlock), new Quaternion(-0.7071068f, 0f, 0f, 0.7071068f), BlockOccupancyType.ChimneyOut),new BlockOccupancy(new Vector3i(0, 0, 0)),new BlockOccupancy(new Vector3i(1, 0, 0)),new BlockOccupancy(new Vector3i(1, 1, 0)), new BlockOccupancy(new Vector3i(2, 0, 0)),new BlockOccupancy(new Vector3i(2, 1, 0)),new BlockOccupancy(new Vector3i(3, 0, 0)),new BlockOccupancy(new Vector3i(3, 1, 0)),});
                
            /* Linked Recipes */
            CraftingComponent.AddRecipe(typeof(NyElectricCementKilnObject), RecipeFamily.Get(typeof (ReinforcedConcreteRecipe)));
            CraftingComponent.AddRecipe(typeof(NyElectricCementKilnObject), RecipeFamily.Get(typeof (CementRecipe)));
            CraftingComponent.AddRecipe(typeof(NyElectricCementKilnObject), RecipeFamily.Get(typeof (MasonryAdvancedUpgradeRecipe)));
        }

    }


    [Serialized]
    [LocDisplayName("Electric Cement Kiln")]
    [Ecopedia("Work Stations", "Craft Tables", createAsSubPage: true, display: InPageTooltip.DynamicTooltip)]
    [LiquidProducer(typeof(SmogItem), 1)]
    [AllowPluginModules(Tags = new[] { "AdvancedUpgrade" }, ItemTypes = new[] { typeof(MasonryAdvancedUpgradeItem) })]
    public partial class NyElectricCementKilnItem : WorldObjectItem<NyElectricCementKilnObject>, IPersistentData
    {
        public override LocString DisplayDescription { get { return Localizer.DoStr("A superior replacement for the cement kiln that use electric power."); } }

        static NyElectricCementKilnItem()
        {

        }

        public override DirectionAxisFlags RequiresSurfaceOnSides { get;} = 0 | DirectionAxisFlags.Down;

        public override HomeFurnishingValue HomeValue => homeValue;
        public static readonly HomeFurnishingValue homeValue = new HomeFurnishingValue()
        {
            Category                 = RoomCategory.Industrial,
            TypeForRoomLimit         = Localizer.DoStr(""),
        };

        [Tooltip(7)] private LocString PowerConsumptionTooltip => Localizer.Do($"Consumes: {Text.Info(3000)}w of {new HeatPower().Name} power from fuel");
        [Serialized, TooltipChildren] public object PersistentData { get; set; }
    }

    [RequiresSkill(typeof(IndustrySkill), 0)]
    public class NyElectricCementKilnRecipe : RecipeFamily
    {
        public NyElectricCementKilnRecipe()
        {
            this.Recipes = new List<Recipe>
            {
                new Recipe(
                    "Electric Cement Kiln",
                    Localizer.DoStr("Electric Cement Kiln"),
                    new IngredientElement[]
                    {
                        new IngredientElement(typeof(AdvancedCircuitItem), 2, typeof(IndustrySkill)),
                        new IngredientElement(typeof(SteelPlateItem), 24, typeof(IndustrySkill)),
                        new IngredientElement(typeof(SteelPipeItem), 16, typeof(IndustrySkill)),
                        new IngredientElement(typeof(BrickItem), 30, typeof(IndustrySkill)),
                        new IngredientElement(typeof(SteelBarItem), 10, typeof(IndustrySkill)),
                    },
                    new CraftingElement<NyElectricCementKilnItem>()
                )
            };


            this.ExperienceOnCraft = 7f;
            this.LaborInCalories = CreateLaborInCaloriesValue(1000, typeof(IndustrySkill));
            this.CraftMinutes = CreateCraftTimeValue(typeof(NyElectricCementKilnRecipe), 20, typeof(IndustrySkill));
            this.Initialize(Localizer.DoStr("Electric Cement Kiln"), typeof(NyElectricCementKilnRecipe));

            CraftingComponent.AddRecipe(typeof(ElectricMachinistTableObject), this);
        }
    }



}
