using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace BlazorFormsTestRound.Components
{
    public class PropertyRowBase : ComponentBase
    {
        public PropertyRowBase()
        {
            DisplayedFullPropertyPaths = new List<string>();
        }
        [Parameter]
        public PropertyInfoAtLevelNode PropertyInfoAtLevel { get; set; }

        [Parameter]
        public int Depth { get; set; }

        [Parameter]
        public List<string> DisplayedFullPropertyPaths { get; set; }

        [Inject]
        protected IJSRuntime JsRunTime { get; set; }

        protected void toggleExpandButton(MouseEventArgs e, string buttonId)
        {
            JsRunTime.InvokeVoidAsync("toggleExpandButton", buttonId);
        }

    }
}
