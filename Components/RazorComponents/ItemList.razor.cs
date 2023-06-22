using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Components.RazorComponents;

public partial class ItemList<TItemType>
{
    private IJSObjectReference _jsModule = default!;

    [Parameter]
    public IList<TItemType> Items { get; init; } = new List<TItemType>();

    [Parameter, EditorRequired]
    public required RenderFragment<TItemType> ItemTemplate { get; set; }

    [Parameter]
    public EventCallback<TItemType> DeleteEvent { get; set; }

    [Parameter]
    public EventCallback<TItemType> SelectEvent { get; set; }

    private async Task<bool> ShouldDelete()
    {
        this._jsModule = await this.jsRuntime.InvokeAsync<IJSObjectReference>( "import",
                                                                               "_content/Components/RazorComponents/ItemList.razor.js" )
                                             .ConfigureAwait( false );
        return await this._jsModule.InvokeAsync<bool>( "showConfirm", "Are you sure?" ).ConfigureAwait( false );
    }
}
