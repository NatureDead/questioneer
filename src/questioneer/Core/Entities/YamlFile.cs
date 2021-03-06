﻿using System.IO;
using Microsoft.Extensions.Configuration;
using questioneer.Core.Entities.Events;

namespace questioneer.Core.Entities
{
    public abstract class YamlFile
    {
        protected abstract int NewestVersion { get; }
        protected abstract string ResourceName { get; }

        protected IConfiguration Configuration { get; private set; }

        public int Version { get; private set; }

        public event YamlFileVersionMismatchHandler VersionMismatch;
        public event YamlFileChangedHandler Changed;

        protected YamlFile()
        {
        }

        public static T Get<T>() where T : YamlFile, new()
        {
            var yamlFile = new T();
            yamlFile.Load(yamlFile.ResourceName);
            return yamlFile;
        }

        private void Load(string resourceName)
        {
            var basePath = Reflections.GetBasePath();
            CheckForFile(basePath, resourceName);

            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddYamlFile(resourceName, false, true);

            Configuration = configurationBuilder.Build();

            var reloadToken = Configuration.GetReloadToken();
            reloadToken.RegisterChangeCallback(x => OnChangedInternal(), null);

            OnChanged();
        }

        private void CheckForFile(string basePath, string resourceName)
        {
            var path = Path.Combine(basePath, resourceName);
            if (File.Exists(path)) return;

            using var stream = Reflections.GetManifestResourceStream(resourceName);
            using var fileStream = new FileStream(path, FileMode.CreateNew);

            stream.CopyTo(fileStream);
        }

        private void OnChangedInternal()
        {
            OnChanged();
            Changed?.Invoke(this);
        }

        protected virtual void OnChanged()
        {
            Version = GetVersion();
        }

        private int GetVersion()
        {
            var versionValue = Configuration["version"];
            var version = int.Parse(versionValue);

            if (version != NewestVersion)
                VersionMismatch?.Invoke(version, NewestVersion);

            return version;
        }
    }
}