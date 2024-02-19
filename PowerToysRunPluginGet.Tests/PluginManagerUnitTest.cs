using PowerToysRunPluginGet.Manager;
using System.Text.Json;

namespace PowerToysRunPluginGet.Tests
{
    [TestClass]
    public class PluginManagerUnitTest
    {
        [TestMethod]
        public async Task PluginManager_GetPluginFolderListAsync()
        {
            var pluginStoreUrl = "https://api.github.com/repos/poychang/powertoys-run-plugins/contents/PluginStore";

            var manager = new PluginManager();
            var result = await manager.GetPluginFolderListAsync(pluginStoreUrl);

            Console.WriteLine(JsonSerializer.Serialize(result));

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task PluginManager_GetPluginManifestAsync()
        {
            const string pluginFolder = "Starter";
            var manager = new PluginManager();
            var result = await manager.GetPluginManifestAsync(pluginFolder);

            Console.WriteLine(JsonSerializer.Serialize(result));

            Assert.IsNotNull(result);
        }
    }
}
