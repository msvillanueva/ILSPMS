using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ILSPMS.Common
{
    public class EnumerationHelper
    {
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            if (fi != null)
            {
                DescriptionAttribute[] attributes =
                    (DescriptionAttribute[])fi.GetCustomAttributes(
                    typeof(DescriptionAttribute),
                    false);

                if (attributes != null &&
                    attributes.Length > 0)
                    return attributes[0].Description;
                else
                    return value.ToString();
            }
            else
                return value.ToString();
        }

        public static List<GenericData> GetEnumList(Type enumType)
        {
            var list = new List<GenericData>();

            foreach (Enum enumValue in Enum.GetValues(enumType))
            {
                list.Add(new GenericData()
                {
                    ID = Convert.ToInt16(enumValue),
                    Name = GetEnumDescription(enumValue)
                });
            }

            return list;
        }
    }
}
