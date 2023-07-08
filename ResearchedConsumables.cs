using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using System.Collections.Generic;
using Terraria.ModLoader.Config;
using System.ComponentModel;

namespace ResearchedConsumables
{
	public class ResearchedConsumables : Mod {}
	public class ResearchedGlobalItem : GlobalItem {
		static Dictionary<int, int> neededSacrificeCounts => CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId;
		public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
			return entity.consumable || entity.useAmmo > 0;
		}
		public override bool ConsumeItem(Item item, Player player) {
			if (OriginConfig.Instance.ConsumeItem && player.whoAmI == Main.myPlayer && neededSacrificeCounts.TryGetValue(item.type, out int needed) && Main.LocalPlayerCreativeTracker.ItemSacrifices.GetSacrificeCount(item.type) >= needed) {
				return false;
			}
			return true;
		}
		public override bool CanBeConsumedAsAmmo(Item ammo, Item weapon, Player player) {
			if (OriginConfig.Instance.CanBeConsumedAsAmmo && player.whoAmI == Main.myPlayer && neededSacrificeCounts.TryGetValue(ammo.type, out int needed) && Main.LocalPlayerCreativeTracker.ItemSacrifices.GetSacrificeCount(ammo.type) >= needed) {
				return false;
			}
			return true;
		}
		public override bool NeedsAmmo(Item item, Player player) {
			if (OriginConfig.Instance.NeedsAmmo && player.whoAmI == Main.myPlayer && neededSacrificeCounts.TryGetValue(item.useAmmo, out int needed) && Main.LocalPlayerCreativeTracker.ItemSacrifices.GetSacrificeCount(item.useAmmo) >= needed) {
				return false;
			}
			return true;
		}
	}
	[Label("Settings")]
	public class OriginConfig : ModConfig {
		public static OriginConfig Instance;
		public override ConfigScope Mode => ConfigScope.ServerSide;

		[Label("Affect ConsumeItem")]
		[DefaultValue(true)]
		public bool ConsumeItem = true;

		[Label("Affect CanBeConsumedAsAmmo")]
		[DefaultValue(true)]
		public bool CanBeConsumedAsAmmo = true;

		[Label("Affect NeedsAmmo")]
		[DefaultValue(true)]
		public bool NeedsAmmo = true;
	}
}