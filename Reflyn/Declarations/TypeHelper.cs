using System;
using System.Reflection;
using System.Xml.Serialization;

namespace Reflyn.Declarations
{
    public sealed class TypeHelper
    {
        internal TypeHelper()
        {
        }

        public static ConstructorInfo GetConstructor(Type type, params Type[] args)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            ConstructorInfo constructor = type.GetConstructor(args);
            if (constructor == null)
            {
                throw new ArgumentNullException("constructor for " + type.FullName + " not found");
            }
            return constructor;
        }

        public static ConstructorInfo GetDefaultConstructor(Type type)
        {
            if ((object)type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
            if ((object)constructor == null)
            {
                throw new ArgumentNullException("no default constructor for " + type.FullName);
            }
            return constructor;
        }

        public static bool IsXmlNullable(FieldInfo field)
        {
            if (field.FieldType.IsValueType)
            {
                return false;
            }
            if (!HasCustomAttribute(field, typeof(XmlElementAttribute)))
            {
                return true;
            }
            XmlElementAttribute xmlElementAttribute = (XmlElementAttribute)GetFirstCustomAttribute(field, typeof(XmlElementAttribute));
            return xmlElementAttribute.IsNullable;
        }

        public static bool HasCustomAttribute(Type type, Type customAttributeType)
        {
            if ((object)type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if ((object)customAttributeType == null)
            {
                throw new ArgumentNullException(nameof(customAttributeType));
            }
            return type.GetCustomAttributes(customAttributeType, inherit: true).Length != 0;
        }

        public static bool HasCustomAttribute(PropertyInfo prop, Type customAttributeType)
        {
            if ((object)prop == null)
            {
                throw new ArgumentNullException(nameof(prop));
            }
            if ((object)customAttributeType == null)
            {
                throw new ArgumentNullException(nameof(customAttributeType));
            }
            return prop.GetCustomAttributes(customAttributeType, inherit: true).Length != 0;
        }

        public static bool HasCustomAttribute(FieldInfo field, Type customAttributeType)
        {
            if ((object)field == null)
            {
                throw new ArgumentNullException(nameof(field));
            }
            if ((object)customAttributeType == null)
            {
                throw new ArgumentNullException(nameof(customAttributeType));
            }
            return field.GetCustomAttributes(customAttributeType, inherit: true).Length != 0;
        }

        public static object GetFirstCustomAttribute(Type type, Type customAttributeType)
        {
            if ((object)type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if ((object)customAttributeType == null)
            {
                throw new ArgumentNullException(nameof(customAttributeType));
            }
            object[] customAttributes = type.GetCustomAttributes(customAttributeType, inherit: true);
            if (customAttributes.Length == 0)
            {
                throw new ArgumentException("type does not have custom attribute");
            }
            return customAttributes[0];
        }

        public static object GetFirstCustomAttribute(PropertyInfo prop, Type customAttributeType)
        {
            if ((object)prop == null)
            {
                throw new ArgumentNullException(nameof(prop));
            }
            if ((object)customAttributeType == null)
            {
                throw new ArgumentNullException(nameof(customAttributeType));
            }
            object[] customAttributes = prop.GetCustomAttributes(customAttributeType, inherit: true);
            if (customAttributes.Length == 0)
            {
                throw new ArgumentException("type does not have custom attribute");
            }
            return customAttributes[0];
        }

        public static object GetFirstCustomAttribute(FieldInfo field, Type customAttributeType)
        {
            if ((object)field == null)
            {
                throw new ArgumentNullException(nameof(field));
            }
            if ((object)customAttributeType == null)
            {
                throw new ArgumentNullException(nameof(customAttributeType));
            }
            object[] customAttributes = field.GetCustomAttributes(customAttributeType, inherit: true);
            if (customAttributes.Length == 0)
            {
                throw new ArgumentException("type does not have custom attribute");
            }
            return customAttributes[0];
        }
    }
}
