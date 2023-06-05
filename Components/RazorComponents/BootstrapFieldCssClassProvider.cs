using Microsoft.AspNetCore.Components.Forms;

namespace Components.RazorComponents;

public sealed class BootstrapFieldCssClassProvider : FieldCssClassProvider
{
    public override string GetFieldCssClass( EditContext editContext, in FieldIdentifier fieldIdentifier )
    {
        if( editContext is null )
        {
            throw new ArgumentNullException( nameof( editContext ) );
        }
        bool isValid = !editContext.GetValidationMessages( fieldIdentifier ).Any();
        bool isModified = editContext.IsModified( fieldIdentifier );
#pragma warning disable IDE0072 // Add missing cases
        return (isModified, isValid) switch
        {
            (true, true ) => "form-control modified is-valid",
            (true, false ) => "form-control modified is-invalid",
            (false, true ) => "form-control",
            (false, false ) => "form-control"
        };
#pragma warning restore IDE0072 // Add missing cases
    }
}
