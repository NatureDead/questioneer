using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using questioneer.Core.Entities.Events;

namespace questioneer.Core.Entities
{
    public abstract class YamlFile
    {
        private IConfiguration _configuration;

        protected abstract string ResourceName { get; }

        public event YamlFileChangedHandler Changed;

        protected YamlFile()
        {
        }

        public string this[string key]
        {
            get { return _configuration[key]; }
            set { _configuration[key] = value; }
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

            _configuration = configurationBuilder.Build();

            var reloadToken = _configuration.GetReloadToken();
            reloadToken.RegisterChangeCallback(x => OnChanged(), null);

            OnChanged();
        }

        private void CheckForFile(string basePath, string resourceName)
        {
            var path = Path.Combine(basePath, resourceName);
            if (File.Exists(path)) return;

            using var stream = Reflections.GetManifestResourceStream(resourceName);
            using var fileStream = new FileStream(path, FileMode.CreateNew);

            stream.CopyTo(fileStream);

            throw new InvalidOperationException($"{resourceName} was created.");
        }

        protected virtual void OnChanged()
        {
            Changed?.Invoke(this);
        }
    }
}