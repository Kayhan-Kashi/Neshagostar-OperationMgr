using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neshagostar.WebUI.Services.ObjectFlatter
{
    public static class Flatter
    {
        public static object Flatten(object paramObj)
        {

            Type type = paramObj.GetType();

            foreach(var propertyInfo in type.GetProperties())
            {
                var newType = propertyInfo.PropertyType;
                if (!propertyInfo.PropertyType.IsValueType && !(propertyInfo.PropertyType.Name=="Guid")) {
                    type.GetProperty(propertyInfo.Name).SetValue(paramObj, null);
                }
            }

            return paramObj;


        }
    }
}