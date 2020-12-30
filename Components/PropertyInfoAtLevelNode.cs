using System;
using System.Collections.Generic;

namespace BlazorFormsTestRound.Components
{

    /// <summary>
    /// Node class for hierarchical structure of property info for an object of given object graph structure.
    /// </summary>
    public class PropertyInfoAtLevelNode
    {
        public PropertyInfoAtLevelNode()
        {
            SubProperties = new Dictionary<string, PropertyInfoAtLevelNode>();
        }

        public string PropertyName { get; set; }
        public object PropertyValue { get; set; }
        public Type PropertyType { get; set; }
        public Dictionary<string, PropertyInfoAtLevelNode> SubProperties { get; private set; }
        public string FullPropertyPath { get; set; }
        public bool IsClass { get; set; }
    }

}