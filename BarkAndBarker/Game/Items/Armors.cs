﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarkAndBarker.Game.Items
{
    public static class Armors
    {
        public static readonly string ItemPrefix = "DesignDataItem:";

        public static readonly List<string> ItemIDs = new List<string>()
        {
            "Id_Item_AdventurerBoots_0001", "Id_Item_AdventurerBoots_1001", "Id_Item_AdventurerBoots_2001",
            "Id_Item_AdventurerBoots_3001", "Id_Item_AdventurerBoots_4001", "Id_Item_AdventurerBoots_5001",
            "Id_Item_AdventurerBoots_6001", "Id_Item_AdventurerBoots_7001",

            "Id_Item_AdventurerCloak_0001", "Id_Item_AdventurerCloak_1001", "Id_Item_AdventurerCloak_2001",
            "Id_Item_AdventurerCloak_3001", "Id_Item_AdventurerCloak_4001", "Id_Item_AdventurerCloak_5001",
            "Id_Item_AdventurerCloak_6001", "Id_Item_AdventurerCloak_7001",

            "Id_Item_AdventurerTunic_0001", "Id_Item_AdventurerTunic_1001", "Id_Item_AdventurerTunic_2001",
            "Id_Item_AdventurerTunic_3001", "Id_Item_AdventurerTunic_4001", "Id_Item_AdventurerTunic_5001",
            "Id_Item_AdventurerTunic_6001", "Id_Item_AdventurerTunic_7001",
            
            "Id_Item_Armet_0001", "Id_Item_Armet_1001", "Id_Item_Armet_2001", "Id_Item_Armet_3001",
            "Id_Item_Armet_4001", "Id_Item_Armet_5001", "Id_Item_Armet_6001", "Id_Item_Armet_7001",

            "Id_Item_BarbutaHelm_0001",
            "Id_Item_BarbutaHelm_1001",
            "Id_Item_BarbutaHelm_2001",
            "Id_Item_BarbutaHelm_3001",
            "Id_Item_BarbutaHelm_4001",
            "Id_Item_BarbutaHelm_5001",
            "Id_Item_BarbutaHelm_6001",
            "Id_Item_BarbutaHelm_7001",

            "Id_Item_BardicPants_0001",
            "Id_Item_BardicPants_1001",
            "Id_Item_BardicPants_2001",
            "Id_Item_BardicPants_3001",
            "Id_Item_BardicPants_4001",
            "Id_Item_BardicPants_5001",
            "Id_Item_BardicPants_6001",
            "Id_Item_BardicPants_7001",

            "Id_Item_Buckler_0001",
            "Id_Item_Buckler_1001",
            "Id_Item_Buckler_2001",
            "Id_Item_Buckler_3001",
            "Id_Item_Buckler_4001",
            "Id_Item_Buckler_5001",
            "Id_Item_Buckler_6001",
            "Id_Item_Buckler_7001",

            "Id_Item_ChapelDeFer_0001",
            "Id_Item_ChapelDeFer_1001",
            "Id_Item_ChapelDeFer_2001",
            "Id_Item_ChapelDeFer_3001",
            "Id_Item_ChapelDeFer_4001",
            "Id_Item_ChapelDeFer_5001",
            "Id_Item_ChapelDeFer_6001",
            "Id_Item_ChapelDeFer_7001",

            "Id_Item_Chaperon_0001",
            "Id_Item_Chaperon_1001",
            "Id_Item_Chaperon_2001",
            "Id_Item_Chaperon_3001",
            "Id_Item_Chaperon_4001",
            "Id_Item_Chaperon_5001",
            "Id_Item_Chaperon_6001",
            "Id_Item_Chaperon_7001",

            "Id_Item_ClothPants_0001",
            "Id_Item_ClothPants_1001",
            "Id_Item_ClothPants_2001",
            "Id_Item_ClothPants_3001",
            "Id_Item_ClothPants_4001",
            "Id_Item_ClothPants_5001",
            "Id_Item_ClothPants_6001",
            "Id_Item_ClothPants_7001",

            "Id_Item_CobaltChapelDeFer_4001",
            "Id_Item_CobaltFrock_4001",
            "Id_Item_CobaltHat_4001",
            "Id_Item_CobaltHeavyGauntlet_4001",
            "Id_Item_CobaltHood_4001",
            "Id_Item_CobaltLeatherGloves_4001",
            "Id_Item_CobaltLightfootBoots_4001",
            "Id_Item_CobaltPlateBoots_4001",
            "Id_Item_CobaltPlatePants_4001",
            "Id_Item_CobaltRegalGambeson_4001",
            "Id_Item_CobaltTemplarArmor_4001",
            "Id_Item_CobaltTrousers_4001",
            "Id_Item_CobaltVikingHelm_4001",


            "Id_Item_RubysilverAdventurerBoots",
            "Id_Item_RubysilverBarbutaHelm",
            "Id_Item_RubysilverDoublet",
            "Id_Item_RubysilverFineCuirass",
            "Id_Item_RubysilverHeavyGauntlet",
            "Id_Item_RubysilverHood",
            "Id_Item_RubysilverLeatherCap",
            "Id_Item_RubysilverLeatherLeggings",
            "Id_Item_RubysilverMysticVestments",
            "Id_Item_RubysilverPlateBoots",
            "Id_Item_RubysilverPlatePants",
            "Id_Item_RubysilverRawhideGloves",

            "Id_Item_DarkPlateArmor_0001",
            "Id_Item_DarkPlateArmor_1001",
            "Id_Item_DarkPlateArmor_2001",
            "Id_Item_DarkPlateArmor_3001",
            "Id_Item_DarkPlateArmor_4001",
            "Id_Item_DarkPlateArmor_5001",
            "Id_Item_DarkPlateArmor_6001",
            "Id_Item_DarkPlateArmor_7001",

            "Id_Item_Doublet_0001",
            "Id_Item_Doublet_1001",
            "Id_Item_Doublet_2001",
            "Id_Item_Doublet_3001",
            "Id_Item_Doublet_4001",
            "Id_Item_Doublet_5001",
            "Id_Item_Doublet_6001",
            "Id_Item_Doublet_7001",

            "Id_Item_FeatheredHat_0001",
            "Id_Item_FeatheredHat_1001",
            "Id_Item_FeatheredHat_2001",
            "Id_Item_FeatheredHat_3001",
            "Id_Item_FeatheredHat_4001",
            "Id_Item_FeatheredHat_5001",
            "Id_Item_FeatheredHat_6001",
            "Id_Item_FeatheredHat_7001",

            "Id_Item_FineCuirass_0001",
            "Id_Item_FineCuirass_1001",
            "Id_Item_FineCuirass_2001",
            "Id_Item_FineCuirass_3001",
            "Id_Item_FineCuirass_4001",
            "Id_Item_FineCuirass_5001",
            "Id_Item_FineCuirass_6001",
            "Id_Item_FineCuirass_7001",

            "Id_Item_ForestHood_0001",
            "Id_Item_ForestHood_1001",
            "Id_Item_ForestHood_2001",
            "Id_Item_ForestHood_3001",
            "Id_Item_ForestHood_4001",
            "Id_Item_ForestHood_5001",
            "Id_Item_ForestHood_6001",
            "Id_Item_ForestHood_7001",

            "Id_Item_Frock_0001",
            "Id_Item_Frock_1001",
            "Id_Item_Frock_2001",
            "Id_Item_Frock_3001",
            "Id_Item_Frock_4001",
            "Id_Item_Frock_5001",
            "Id_Item_Frock_6001",
            "Id_Item_Frock_7001",

            "Id_Item_Gjermundbu_0001",
            "Id_Item_Gjermundbu_1001",
            "Id_Item_Gjermundbu_2001",
            "Id_Item_Gjermundbu_3001",
            "Id_Item_Gjermundbu_4001",
            "Id_Item_Gjermundbu_5001",
            "Id_Item_Gjermundbu_6001",
            "Id_Item_Gjermundbu_7001",

            "Id_Item_HeavyBoots_0001",
            "Id_Item_HeavyBoots_1001",
            "Id_Item_HeavyBoots_2001",
            "Id_Item_HeavyBoots_3001",
            "Id_Item_HeavyBoots_4001",
            "Id_Item_HeavyBoots_5001",
            "Id_Item_HeavyBoots_6001",
            "Id_Item_HeavyBoots_7001",

            "Id_Item_HeavyGauntlet_0001",
            "Id_Item_HeavyGauntlet_1001",
            "Id_Item_HeavyGauntlet_2001",
            "Id_Item_HeavyGauntlet_3001",
            "Id_Item_HeavyGauntlet_4001",
            "Id_Item_HeavyGauntlet_5001",
            "Id_Item_HeavyGauntlet_6001",
            "Id_Item_HeavyGauntlet_7001",

            "Id_Item_HeavyLeatherLeggings_0001",
            "Id_Item_HeavyLeatherLeggings_1001",
            "Id_Item_HeavyLeatherLeggings_2001",
            "Id_Item_HeavyLeatherLeggings_3001",
            "Id_Item_HeavyLeatherLeggings_4001",
            "Id_Item_HeavyLeatherLeggings_5001",
            "Id_Item_HeavyLeatherLeggings_6001",
            "Id_Item_HeavyLeatherLeggings_7001",

            "Id_Item_Hounskull_0001",
            "Id_Item_Hounskull_1001",
            "Id_Item_Hounskull_2001",
            "Id_Item_Hounskull_3001",
            "Id_Item_Hounskull_4001",
            "Id_Item_Hounskull_5001",
            "Id_Item_Hounskull_6001",
            "Id_Item_Hounskull_7001",

            "Id_Item_KettleHat_0001",
            "Id_Item_KettleHat_1001",
            "Id_Item_KettleHat_2001",
            "Id_Item_KettleHat_3001",
            "Id_Item_KettleHat_4001",
            "Id_Item_KettleHat_5001",
            "Id_Item_KettleHat_6001",
            "Id_Item_KettleHat_7001",

            "Id_Item_LacedTurnshoe_0001",
            "Id_Item_LacedTurnshoe_1001",
            "Id_Item_LacedTurnshoe_2001",
            "Id_Item_LacedTurnshoe_3001",
            "Id_Item_LacedTurnshoe_4001",
            "Id_Item_LacedTurnshoe_5001",
            "Id_Item_LacedTurnshoe_6001",
            "Id_Item_LacedTurnshoe_7001",

            "Id_Item_LeatherBonnet_0001",
            "Id_Item_LeatherBonnet_1001",
            "Id_Item_LeatherBonnet_2001",
            "Id_Item_LeatherBonnet_3001",
            "Id_Item_LeatherBonnet_4001",
            "Id_Item_LeatherBonnet_5001",
            "Id_Item_LeatherBonnet_6001",
            "Id_Item_LeatherBonnet_7001",
            "Id_Item_LeatherCap_0001",
            "Id_Item_LeatherCap_1001",
            "Id_Item_LeatherCap_2001",
            "Id_Item_LeatherCap_3001",
            "Id_Item_LeatherCap_4001",
            "Id_Item_LeatherCap_5001",
            "Id_Item_LeatherCap_6001",
            "Id_Item_LeatherCap_7001",
            "Id_Item_LeatherChausses_0001",
            "Id_Item_LeatherChausses_1001",
            "Id_Item_LeatherChausses_2001",
            "Id_Item_LeatherChausses_3001",
            "Id_Item_LeatherChausses_4001",
            "Id_Item_LeatherChausses_5001",
            "Id_Item_LeatherChausses_6001",
            "Id_Item_LeatherChausses_7001",
            "Id_Item_LeatherGloves_0001",
            "Id_Item_LeatherGloves_1001",
            "Id_Item_LeatherGloves_2001",
            "Id_Item_LeatherGloves_3001",
            "Id_Item_LeatherGloves_4001",
            "Id_Item_LeatherGloves_5001",
            "Id_Item_LeatherGloves_6001",
            "Id_Item_LeatherGloves_7001",
            "Id_Item_LeatherLeggings_0001",
            "Id_Item_LeatherLeggings_1001",
            "Id_Item_LeatherLeggings_2001",
            "Id_Item_LeatherLeggings_3001",
            "Id_Item_LeatherLeggings_4001",
            "Id_Item_LeatherLeggings_5001",
            "Id_Item_LeatherLeggings_6001",
            "Id_Item_LeatherLeggings_7001",
            "Id_Item_LightfootBoots_0001",
            "Id_Item_LightfootBoots_1001",
            "Id_Item_LightfootBoots_2001",
            "Id_Item_LightfootBoots_3001",
            "Id_Item_LightfootBoots_4001",
            "Id_Item_LightfootBoots_5001",
            "Id_Item_LightfootBoots_6001",
            "Id_Item_LightfootBoots_7001",

            "Id_Item_LooseTrousers_0001",
            "Id_Item_LooseTrousers_1001",
            "Id_Item_LooseTrousers_2001",
            "Id_Item_LooseTrousers_3001",
            "Id_Item_LooseTrousers_4001",
            "Id_Item_LooseTrousers_5001",
            "Id_Item_LooseTrousers_6001",
            "Id_Item_LooseTrousers_7001",

            "Id_Item_MarauderOutfit_0001",
            "Id_Item_MarauderOutfit_1001",
            "Id_Item_MarauderOutfit_2001",
            "Id_Item_MarauderOutfit_3001",
            "Id_Item_MarauderOutfit_4001",
            "Id_Item_MarauderOutfit_5001",
            "Id_Item_MarauderOutfit_6001",
            "Id_Item_MarauderOutfit_7001",

            "Id_Item_MysticVestments_0001",
            "Id_Item_MysticVestments_1001",
            "Id_Item_MysticVestments_2001",
            "Id_Item_MysticVestments_3001",
            "Id_Item_MysticVestments_4001",
            "Id_Item_MysticVestments_5001",
            "Id_Item_MysticVestments_6001",
            "Id_Item_MysticVestments_7001",

            "Id_Item_NorthernFullTunic_0001",
            "Id_Item_NorthernFullTunic_1001",
            "Id_Item_NorthernFullTunic_2001",
            "Id_Item_NorthernFullTunic_3001",
            "Id_Item_NorthernFullTunic_4001",
            "Id_Item_NorthernFullTunic_5001",
            "Id_Item_NorthernFullTunic_6001",
            "Id_Item_NorthernFullTunic_7001",


            "Id_Item_OldShoes_0001",
            "Id_Item_OldShoes_1001",
            "Id_Item_OldShoes_2001",
            "Id_Item_OldShoes_3001",
            "Id_Item_OldShoes_4001",
            "Id_Item_OldShoes_5001",
            "Id_Item_OldShoes_6001",
            "Id_Item_OldShoes_7001",
            "Id_Item_OracleRobe_0001",
            "Id_Item_OracleRobe_1001",
            "Id_Item_OracleRobe_2001",
            "Id_Item_OracleRobe_3001",
            "Id_Item_OracleRobe_4001",
            "Id_Item_OracleRobe_5001",
            "Id_Item_OracleRobe_6001",
            "Id_Item_OracleRobe_7001",

            "Id_Item_PaddedLeggings_0001",
            "Id_Item_PaddedLeggings_1001",
            "Id_Item_PaddedLeggings_2001",
            "Id_Item_PaddedLeggings_3001",
            "Id_Item_PaddedLeggings_4001",
            "Id_Item_PaddedLeggings_5001",
            "Id_Item_PaddedLeggings_6001",
            "Id_Item_PaddedLeggings_7001",
            "Id_Item_PaddedTunic_0001",
            "Id_Item_PaddedTunic_1001",
            "Id_Item_PaddedTunic_2001",
            "Id_Item_PaddedTunic_3001",
            "Id_Item_PaddedTunic_4001",
            "Id_Item_PaddedTunic_5001",
            "Id_Item_PaddedTunic_6001",
            "Id_Item_PaddedTunic_7001",

            "Id_Item_PlateBoots_0001",
            "Id_Item_PlateBoots_1001",
            "Id_Item_PlateBoots_2001",
            "Id_Item_PlateBoots_3001",
            "Id_Item_PlateBoots_4001",
            "Id_Item_PlateBoots_5001",
            "Id_Item_PlateBoots_6001",
            "Id_Item_PlateBoots_7001",
            "Id_Item_PlatePants_0001",
            "Id_Item_PlatePants_1001",
            "Id_Item_PlatePants_2001",
            "Id_Item_PlatePants_3001",
            "Id_Item_PlatePants_4001",
            "Id_Item_PlatePants_5001",
            "Id_Item_PlatePants_6001",
            "Id_Item_PlatePants_7001",

            "Id_Item_RadiantCloak_0001",
            "Id_Item_RadiantCloak_1001",
            "Id_Item_RadiantCloak_2001",
            "Id_Item_RadiantCloak_3001",
            "Id_Item_RadiantCloak_4001",
            "Id_Item_RadiantCloak_5001",
            "Id_Item_RadiantCloak_6001",
            "Id_Item_RadiantCloak_7001",

            "Id_Item_RangerHood_0001",
            "Id_Item_RangerHood_1001",
            "Id_Item_RangerHood_2001",
            "Id_Item_RangerHood_3001",
            "Id_Item_RangerHood_4001",
            "Id_Item_RangerHood_5001",
            "Id_Item_RangerHood_6001",
            "Id_Item_RangerHood_7001",

            "Id_Item_RawhideGloves_0001",
            "Id_Item_RawhideGloves_1001",
            "Id_Item_RawhideGloves_2001",
            "Id_Item_RawhideGloves_3001",
            "Id_Item_RawhideGloves_4001",
            "Id_Item_RawhideGloves_5001",
            "Id_Item_RawhideGloves_6001",
            "Id_Item_RawhideGloves_7001",


            "Id_Item_RegalGambeson_0001",
            "Id_Item_RegalGambeson_1001",
            "Id_Item_RegalGambeson_2001",
            "Id_Item_RegalGambeson_3001",
            "Id_Item_RegalGambeson_4001",
            "Id_Item_RegalGambeson_5001",
            "Id_Item_RegalGambeson_6001",
            "Id_Item_RegalGambeson_7001",

            "Id_Item_ReinforcedGloves_0001",
            "Id_Item_ReinforcedGloves_1001",
            "Id_Item_ReinforcedGloves_2001",
            "Id_Item_ReinforcedGloves_3001",
            "Id_Item_ReinforcedGloves_4001",
            "Id_Item_ReinforcedGloves_5001",
            "Id_Item_ReinforcedGloves_6001",
            "Id_Item_ReinforcedGloves_7001",

            "Id_Item_RivetedGloves_0001",
            "Id_Item_RivetedGloves_1001",
            "Id_Item_RivetedGloves_2001",
            "Id_Item_RivetedGloves_3001",
            "Id_Item_RivetedGloves_4001",
            "Id_Item_RivetedGloves_5001",
            "Id_Item_RivetedGloves_6001",
            "Id_Item_RivetedGloves_7001",

            "Id_Item_RogueCowl_0001",
            "Id_Item_RogueCowl_1001",
            "Id_Item_RogueCowl_2001",
            "Id_Item_RogueCowl_3001",
            "Id_Item_RogueCowl_4001",
            "Id_Item_RogueCowl_5001",
            "Id_Item_RogueCowl_6001",
            "Id_Item_RogueCowl_7001",


            "Id_Item_RuggedBoots_0001",
            "Id_Item_RuggedBoots_1001",
            "Id_Item_RuggedBoots_2001",
            "Id_Item_RuggedBoots_3001",
            "Id_Item_RuggedBoots_4001",
            "Id_Item_RuggedBoots_5001",
            "Id_Item_RuggedBoots_6001",
            "Id_Item_RuggedBoots_7001",
            "Id_Item_ShadowHood_0001",
            "Id_Item_ShadowHood_1001",
            "Id_Item_ShadowHood_2001",
            "Id_Item_ShadowHood_3001",
            "Id_Item_ShadowHood_4001",
            "Id_Item_ShadowHood_5001",
            "Id_Item_ShadowHood_6001",
            "Id_Item_ShadowHood_7001",
            "Id_Item_ShadowMask_0001",
            "Id_Item_ShadowMask_1001",
            "Id_Item_ShadowMask_2001",
            "Id_Item_ShadowMask_3001",
            "Id_Item_ShadowMask_4001",
            "Id_Item_ShadowMask_5001",
            "Id_Item_ShadowMask_6001",
            "Id_Item_ShadowMask_7001",

            "Id_Item_SplendidCloak_0001",
            "Id_Item_SplendidCloak_1001",
            "Id_Item_SplendidCloak_2001",
            "Id_Item_SplendidCloak_3001",
            "Id_Item_SplendidCloak_4001",
            "Id_Item_SplendidCloak_5001",
            "Id_Item_SplendidCloak_6001",
            "Id_Item_SplendidCloak_7001",

            "Id_Item_StrawHat_0001",
            "Id_Item_StrawHat_1001",
            "Id_Item_StrawHat_2001",
            "Id_Item_StrawHat_3001",
            "Id_Item_StrawHat_4001",
            "Id_Item_StrawHat_5001",
            "Id_Item_StrawHat_6001",
            "Id_Item_StrawHat_7001",

            "Id_Item_TatteredCloak_0001",
            "Id_Item_TatteredCloak_1001",
            "Id_Item_TatteredCloak_2001",
            "Id_Item_TatteredCloak_3001",
            "Id_Item_TatteredCloak_4001",
            "Id_Item_TatteredCloak_5001",
            "Id_Item_TatteredCloak_6001",
            "Id_Item_TatteredCloak_7001",
            "Id_Item_TemplarArmor_0001",
            "Id_Item_TemplarArmor_1001",
            "Id_Item_TemplarArmor_2001",
            "Id_Item_TemplarArmor_3001",
            "Id_Item_TemplarArmor_4001",
            "Id_Item_TemplarArmor_5001",
            "Id_Item_TemplarArmor_6001",
            "Id_Item_TemplarArmor_7001",

            "Id_Item_TroubadourOutfit_0001",
            "Id_Item_TroubadourOutfit_1001",
            "Id_Item_TroubadourOutfit_2001",
            "Id_Item_TroubadourOutfit_3001",
            "Id_Item_TroubadourOutfit_4001",
            "Id_Item_TroubadourOutfit_5001",
            "Id_Item_TroubadourOutfit_6001",
            "Id_Item_TroubadourOutfit_7001",

            "Id_Item_VikingHelm_0001",
            "Id_Item_VikingHelm_1001",
            "Id_Item_VikingHelm_2001",
            "Id_Item_VikingHelm_3001",
            "Id_Item_VikingHelm_4001",
            "Id_Item_VikingHelm_5001",
            "Id_Item_VikingHelm_6001",
            "Id_Item_VikingHelm_7001",


            "Id_Item_VisoredBarbutaHelm_0001",
            "Id_Item_VisoredBarbutaHelm_1001",
            "Id_Item_VisoredBarbutaHelm_2001",
            "Id_Item_VisoredBarbutaHelm_3001",
            "Id_Item_VisoredBarbutaHelm_4001",
            "Id_Item_VisoredBarbutaHelm_5001",
            "Id_Item_VisoredBarbutaHelm_6001",
            "Id_Item_VisoredBarbutaHelm_7001",
            "Id_Item_WandererAttire_0001",
            "Id_Item_WandererAttire_1001",
            "Id_Item_WandererAttire_2001",
            "Id_Item_WandererAttire_3001",
            "Id_Item_WandererAttire_4001",
            "Id_Item_WandererAttire_5001",
            "Id_Item_WandererAttire_6001",
            "Id_Item_WandererAttire_7001",
            "Id_Item_WardenOutfit_0001",
            "Id_Item_WardenOutfit_1001",
            "Id_Item_WardenOutfit_2001",
            "Id_Item_WardenOutfit_3001",
            "Id_Item_WardenOutfit_4001",
            "Id_Item_WardenOutfit_5001",
            "Id_Item_WardenOutfit_6001",
            "Id_Item_WardenOutfit_7001",

            "Id_Item_WizardHat_0001",
            "Id_Item_WizardHat_1001",
            "Id_Item_WizardHat_2001",
            "Id_Item_WizardHat_3001",
            "Id_Item_WizardHat_4001",
            "Id_Item_WizardHat_5001",
            "Id_Item_WizardHat_6001",
            "Id_Item_WizardHat_7001",

            "Id_Item_WizardShoes_0001",
            "Id_Item_WizardShoes_1001",
            "Id_Item_WizardShoes_2001",
            "Id_Item_WizardShoes_3001",
            "Id_Item_WizardShoes_4001",
            "Id_Item_WizardShoes_5001",
            "Id_Item_WizardShoes_6001",
            "Id_Item_WizardShoes_7001",
        };
    }
}
