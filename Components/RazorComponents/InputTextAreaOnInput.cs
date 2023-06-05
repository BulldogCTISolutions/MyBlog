using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Components.Rendering;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.AspNetCore.Components.Forms;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class InputTextAreaOnInput : InputBase<string?>
{
    protected override void BuildRenderTree( RenderTreeBuilder builder )
    {
        if( builder is null )
        {
            throw new ArgumentNullException( nameof( builder ) );
        }
        builder.OpenElement( 0, "textarea" );
        builder.AddMultipleAttributes( 1, this.AdditionalAttributes );
        builder.AddAttribute( 2, "class", this.CssClass );
        builder.AddAttribute( 3, "value", BindConverter.FormatValue( this.CurrentValue ) );
        builder.AddAttribute( 4, "oninput", EventCallback.Factory.CreateBinder<string?>( this, __value => this.CurrentValueAsString = __value, this.CurrentValueAsString ) );
        builder.CloseElement();
    }

    protected override bool TryParseValueFromString( string? value, out string? result, [NotNullWhen( false )] out string? validationErrorMessage )
    {
        result = value;
        validationErrorMessage = null;
        return true;
    }
}
