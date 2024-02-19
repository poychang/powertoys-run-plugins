namespace PowerToysRunPluginGet.Manager
{
    public class PluginManifest
    {
        public string ManifestVersion { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public string ID { get; set; } = string.Empty;
        public string PackageName { get; set; } = string.Empty;
        public string Repository { get; set; } = string.Empty;
        public Developer Developer { get; set; } = new();
        public Name Name { get; set; } = new();
        public Description Description { get; set; } = new();
        public Architecture Architecture { get; set; } = new();
    }

    public class Developer
    {
        public string Name { get; set; } = string.Empty;
        public string WebsiteUrl { get; set; } = string.Empty;
        public string PrivacyUrl { get; set; } = string.Empty;
        public string TermsOfUseUrl { get; set; } = string.Empty;
    }

    public class Name
    {
        public string Short { get; set; } = string.Empty;
        public string Full { get; set; } = string.Empty;
    }

    public class Description
    {
        public string Short { get; set; } = string.Empty;
        public string Full { get; set; } = string.Empty;
    }

    public class Architecture
    {
        public ArmInfo Arm { get; set; } = new();
        public X64Info X64 { get; set; } = new();

        public class ArmInfo
        {
            public string Filename { get; set; } = string.Empty;
            public string Filetype { get; set; } = string.Empty;
        }

        public class X64Info
        {
            public string Filename { get; set; } = string.Empty;
            public string Filetype { get; set; } = string.Empty;
        }
    }
}
