using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ReactGraph.Internals
{
    public class NodeInfo
    {
        private readonly Action reevaluateValue;

        public NodeInfo(object instance, PropertyInfo propertyInfo, MemberExpression localPropertyExpression, Action reevaluateValue)
        {
            this.reevaluateValue = reevaluateValue;
            PropertyInfo = propertyInfo;
            LocalPropertyExpression = localPropertyExpression;
            Instance = instance;
        }

        public object Instance { get; private set; }

        public PropertyInfo PropertyInfo { get; private set; }

        public MemberExpression LocalPropertyExpression { get; private set; }

        public void ReevalValue()
        {
            if (reevaluateValue != null)
                reevaluateValue();
        }

        private bool Equals(NodeInfo other)
        {
            return Equals(Instance, other.Instance) && Equals(PropertyInfo, other.PropertyInfo);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((NodeInfo)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Instance != null ? Instance.GetHashCode() : 0) * 397) ^ (PropertyInfo != null ? PropertyInfo.GetHashCode() : 0);
            }
        }

        public static bool operator ==(NodeInfo left, NodeInfo right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(NodeInfo left, NodeInfo right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return PropertyInfo.Name;
        }

        /*
         * target: 
         *  - instance,
         *  - property info (instance + property info = key)
         *  - get delegate, 
         *  - set delegate
         * ReEval expression
         *  - type: static method
         * Dependents
         */
    }
}