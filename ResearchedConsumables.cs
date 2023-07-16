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
		static Dictionary<int, int> NeededSacrificeCounts => CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId;
		public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
			return entity.consumable || entity.useAmmo > 0;
		}
		public override bool ConsumeItem(Item item, Player player) {
			if (ResearchedConsumablesConfig.Instance.ConsumeItem && player.whoAmI == Main.myPlayer && ((item.createTile == -1 && item.createWall == -1) || ResearchedConsumablesConfig.Instance.Tiles) && HasResearched(item.type, player)) {
				return false;
			}
			return true;
		}
		public override bool CanBeConsumedAsAmmo(Item ammo, Item weapon, Player player) {
			if (ResearchedConsumablesConfig.Instance.CanBeConsumedAsAmmo && player.whoAmI == Main.myPlayer && HasResearched(ammo.type, player)) {
				return false;
			}
			return true;
		}
		public override bool NeedsAmmo(Item item, Player player) {
			if (ResearchedConsumablesConfig.Instance.NeedsAmmo && player.whoAmI == Main.myPlayer && HasResearched(item.useAmmo, player)) {
				return false;
			}
			return true;
		}
		static bool HasResearched(int type, Player player) => NeededSacrificeCounts.TryGetValue(type, out int needed) && player.creativeTracker.ItemSacrifices.GetSacrificeCount(type) >= needed;
	}
	public class ResearchedConsumablesConfig : ModConfig {
		public static ResearchedConsumablesConfig Instance;
		public override ConfigScope Mode => ConfigScope.ServerSide;

		[DefaultValue(true)]
		public bool ConsumeItem = true;

		[DefaultValue(true)]
		public bool CanBeConsumedAsAmmo = true;

		[DefaultValue(true)]
		public bool NeedsAmmo = true;

		[DefaultValue(true)]
		public bool Tiles = true;
	}
}