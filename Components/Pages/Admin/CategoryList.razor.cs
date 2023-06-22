using Data.Models.Models;

namespace Components.Pages.Admin;

public partial class CategoryList
{
    private List<Category> Items { get; set; } = new List<Category>();

    public Category Item { get; set; } = new Category();

    protected override async Task OnInitializedAsync()
    {
        this.Items = (await this._api.GetCategoriesAsync().ConfigureAwait( false )) ?? new List<Category>();
        await base.OnInitializedAsync().ConfigureAwait( false );
    }

    private async Task Delete( Category category )
    {
        await this._api.DeleteCategoryAsync( category.Id! ).ConfigureAwait( false );
        _ = this.Items.Remove( category );
    }

    private async Task Save()
    {
        _ = await this._api.SaveCategoryAsync( this.Item ).ConfigureAwait( false );
        if( this.Items.Contains( this.Item ) == false )
        {
            this.Items.Add( this.Item );
        }

        this.Item = new Category();
    }

    private Task Select( Category category )
    {
        this.Item = category;
        return Task.CompletedTask;
    }
}
