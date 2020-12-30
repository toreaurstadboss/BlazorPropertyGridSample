using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace BlazorFormsTestRound.Components
{
    public class PropertyGridBase : ComponentBase
    {

        [Parameter] public object DataContext { get; set; }

        public Dictionary<string, PropertyInfoAtLevelNode> Props { get; set; }


        public PropertyGridBase()
        {
            Props = new Dictionary<string, PropertyInfoAtLevelNode>();
        }


        protected override void OnParametersSet()
        {
            Props.Clear();
            if (DataContext == null)
                return;
          
            Props["ROOT"] = MapPropertiesOfDataContext(string.Empty, DataContext, null);

            StateHasChanged();
        }

        private bool IsNestedProperty(PropertyInfo pi) =>
            pi.PropertyType.IsClass && pi.PropertyType.Namespace != "System";

        private PropertyInfoAtLevelNode MapPropertiesOfDataContext(string propertyPath, object parentObject,
            PropertyInfo currentProp)
        {
            if (parentObject == null)
                return null;

            var publicProperties = parentObject.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var propertyNode = new PropertyInfoAtLevelNode
            {
                PropertyName = currentProp?.Name ?? "ROOT",
                PropertyValue = parentObject,
                PropertyType = parentObject.GetType(),
                FullPropertyPath = TrimFullPropertyPath($"{propertyPath}.{currentProp?.Name}") ?? "ROOT",
                IsClass = parentObject.GetType().IsClass && parentObject.GetType().Namespace != "System"
            };

            foreach (var p in publicProperties)
            {
                var propertyValue = p.GetValue(parentObject, null); 

                if (!IsNestedProperty(p))
                {
                    propertyNode.SubProperties.Add(p.Name, new PropertyInfoAtLevelNode
                        {
                            IsClass = false,
                            FullPropertyPath = TrimFullPropertyPath($"{propertyPath}.{p.Name}"),
                            PropertyName = p.Name,
                            PropertyValue = propertyValue,
                            PropertyType = p.PropertyType
                            //note - SubProperties are default empty if not nested property of course.
                        }
                    );
                }
                else
                {
                    //we need to add the sub property but recurse also call to fetch the nested properties 
                    propertyNode.SubProperties.Add(p.Name, new PropertyInfoAtLevelNode
                        {
                            IsClass = true,
                            FullPropertyPath = propertyPath + p.Name,
                            PropertyName = p.Name,
                            PropertyValue = MapPropertiesOfDataContext(TrimFullPropertyPath($"{propertyPath}.{p.Name}"), propertyValue, p),
                            PropertyType = p.PropertyType
                            //note - SubProperties are default empty if not nested property of course.
                        }
                    );
                }
            }

            return propertyNode;
        }

        private string TrimFullPropertyPath(string fullpropertypath)
        {
            if (string.IsNullOrEmpty(fullpropertypath))
                return fullpropertypath;
            return fullpropertypath.TrimStart('.').TrimEnd('.');
        }

    }

}