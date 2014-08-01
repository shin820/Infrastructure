using System;

namespace Infrastructure.Common.Extension
{
    /// <summary>
    /// Type扩展
    /// </summary>
    public static class TypeExtension
    {
        /// <summary>
        /// 判断子类是否可以赋值给基类，支持开放泛型
        /// </summary>
        /// <param name="extendType">子类型</param>
        /// <param name="baseType">基类型</param>
        /// <returns>是否可以赋值</returns>
        public static bool IsAssignableTo(this Type extendType, Type baseType)
        {
            while (!baseType.IsAssignableFrom(extendType))
            {
                if (extendType == null)
                {
                    return false;
                }

                if (extendType == typeof(object))
                {
                    return false;
                }

                if (extendType.IsGenericType && !extendType.IsGenericTypeDefinition)
                {
                    extendType = extendType.GetGenericTypeDefinition();
                }
                else
                {
                    extendType = extendType.BaseType;
                }
            }

            return true;
        }
    }
}
