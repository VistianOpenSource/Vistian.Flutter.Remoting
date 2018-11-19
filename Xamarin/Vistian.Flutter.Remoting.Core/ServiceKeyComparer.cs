using System;
using System.Collections.Generic;

namespace Vistian.Flutter.Remoting
{

    public class ServiceKeyComparer:IEqualityComparer<ServiceKey>
    {
        public bool Equals(ServiceKey type1, ServiceKey type2)
        {
            return type1 != null || type2 != null && type2 != null && type1 != null && type1.Value == type2.Value ? true : false;
        }

        public int GetHashCode(ServiceKey type)
        {
            return type.Value.GetHashCode();
        }
    }
}
