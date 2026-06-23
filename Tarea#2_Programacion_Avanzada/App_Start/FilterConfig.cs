using System.Web;
using System.Web.Mvc;

namespace Tarea_2_Programacion_Avanzada
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
