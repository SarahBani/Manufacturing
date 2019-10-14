using Core.DomainModel;
using Microsoft.Extensions.Configuration;
using System;

namespace Core.DomainServices
{
    public static class Utility
    {

        public static T ConvertToEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }

        public static string GetConnectionString(IConfiguration config)
        {
            return config.GetConnectionString(Constant.AppSetting_DefaultConnection);
        }

    }
}
