using System.Net.Http.Headers;
using System.Text.Json;

namespace PowerToysRunPluginGet.Manager
{
    public class PluginManager
    {
        private readonly HttpClient _client;
        private JsonSerializerOptions _jsonSerializerOptions => new() { PropertyNameCaseInsensitive = true };
        // 獲取指定儲存庫中 PluginStore 資料夾的內容
        private string _pluginStoreUrl = "https://api.github.com/repos/poychang/powertoys-run-plugins/contents/PluginStore";

        public PluginManager()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AppName", "1.0"));
        }

        public async Task<IEnumerable<PluginManifest>> SearchPlugins(string query)
        {
            try
            {
                // PluginStore 資料夾中的所有套件名稱
                var pluginFolders = await GetPluginFolderListAsync(_pluginStoreUrl);
                var pluginManifests = pluginFolders.Select(folder => GetPluginManifestAsync(folder).GetAwaiter().GetResult());
                return pluginManifests;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Message :{0} ", ex.Message);
            }
            return [];
        }

        public async Task<List<string>> GetPluginFolderListAsync(string pluginStoreUrl)
        {
            var response = await _client.GetAsync(pluginStoreUrl);
            response.EnsureSuccessStatusCode();
            var contents = JsonSerializer.Deserialize<List<GitHubContent>>(await response.Content.ReadAsStreamAsync(), _jsonSerializerOptions) ?? [];

            return contents.Where(p => p.Type == "dir").Select(p => p.Name).ToList();
        }

        public async Task<PluginManifest> GetPluginManifestAsync(string pluginFolder)
        {
            var requestUri = $"{_pluginStoreUrl}/{pluginFolder}/manifest.json";
            var response = await _client.GetAsync(requestUri);

            response.EnsureSuccessStatusCode();

            var fileContent = JsonSerializer.Deserialize<GitHubContent>(await response.Content.ReadAsStreamAsync(), _jsonSerializerOptions)?.Content;
            var file = Convert.FromBase64String(fileContent); // 解碼 Base64 編碼的內容

            return JsonSerializer.Deserialize<PluginManifest>(file, _jsonSerializerOptions);
        }
    }

    public class GitHubContent
    {
        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // "file" 或 "dir"
        public string Content { get; set; } = string.Empty;
        public string Encoding { get; set; } = string.Empty;
    }
}
