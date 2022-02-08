
using Leclair.Stardew.BetterCrafting;
using Leclair.Stardew.Common.Events;

using StardewModdingAPI.Events;

namespace Leclair.Stardew.BCSpaceCore {
	class ModEntry : ModSubscriber {

		[Subscriber]
		private void OnGameLaunched(object sender, GameLaunchedEventArgs e) {
			Log("Hi.", StardewModdingAPI.LogLevel.Info);

			var api = Helper.ModRegistry.GetApi<IAPI>("leclair.bettercrafting");
			api.v1.AddRecipeProvider(new SpaceCoreProvider());
		}

	}
}
