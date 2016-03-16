using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKAnalyzer
{
    public class RepositoryFactory
    {
        public enum Repositories { Vk, Facebook, Default };

        public static IRepository CreateRepository(Repositories type = Repositories.Vk)
        {
            switch (type)
            {
                case Repositories.Vk: return VkRepository.Instance;
                case Repositories.Facebook: return  FacebookRepository.Instance;
                default:
                  return new VkRepository();
            }
        }
    }
}
