using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IsMatch.StoriesDownloader.Provider
{
    public class StoriesProviders
    {
        public static StoriesProviders Instance
        {
            get
            {
                return Holder.providers;
            }
        }

        public List<IStoriesProvider> Providers
        {
            get; set;
        } = new List<IStoriesProvider>();


        public void AddMusicProvider(IStoriesProvider provider)
        {
            Providers.Add(provider);
            type2Provider.Add(provider.ProviderName, provider);
        }

        Dictionary<string, IStoriesProvider> type2Provider = new Dictionary<string, IStoriesProvider>();

        public string ProviderName => "StoriesProviders";


        static class Holder
        {
            public static StoriesProviders providers = Load();

            /// <summary>
            /// 从当前Assembly加载
            /// </summary>
            /// <returns></returns>
            private static StoriesProviders Load()
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                List<Type> hostTypes = new List<Type>();

                foreach (var type in assembly.GetExportedTypes())
                {
                    if (type.Name == "StoriesProviders")
                    {
                        continue;
                    }
                    //确定type为类并且继承自(实现)IMyInstance
                    if (type.IsClass && typeof(IStoriesProvider).IsAssignableFrom(type) && !type.IsAbstract)
                        hostTypes.Add(type);
                }

                StoriesProviders storyProviders = new StoriesProviders();
                foreach (var type in hostTypes)
                {
                    IStoriesProvider instance = (IStoriesProvider)Activator.CreateInstance(type);
                    storyProviders.AddMusicProvider(instance);
                }

                return storyProviders;
            }
        }
    }
}
