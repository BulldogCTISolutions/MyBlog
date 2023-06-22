using Data.Models.Models;

namespace Components.Pages.Admin;

public partial class TagList
{
    private List<Tag> Items { get; set; } = new List<Tag>();
    public Tag Item { get; set; } = new Tag();

    protected override async Task OnInitializedAsync()
    {
        this.Items = (await this._api.GetTagsAsync().ConfigureAwait( false )) ?? new List<Tag>();
        await base.OnInitializedAsync().ConfigureAwait( false );
    }

    private async Task Delete( Tag tag )
    {
        await this._api.DeleteTagAsync( tag.Id! ).ConfigureAwait( false );
        _ = this.Items.Remove( tag );
    }

    private async Task Save()
    {
        _ = await this._api.SaveTagAsync( this.Item ).ConfigureAwait( false );
        if( this.Items.Contains( this.Item ) == false )
        {
            this.Items.Add( this.Item );
        }
        this.Item = new Tag();
    }

    private Task Select( Tag tag )
    {
        this.Item = tag;
        return Task.CompletedTask;
    }
}
