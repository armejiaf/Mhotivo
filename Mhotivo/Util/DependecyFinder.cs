using System.Web.Mvc;

namespace Mhotivo.Util
{
    
    public class DependecyFinder<T>
    {
        public T GetDependency()
        {
            return ((T)DependencyResolver.Current.GetService(typeof(T)));
        }
    }
}