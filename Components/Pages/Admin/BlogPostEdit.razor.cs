using Components.RazorComponents;

using Data.Models.Models;

using Markdig;

using Microsoft.AspNetCore.Components;

namespace Components.Pages.Admin;

public partial class BlogPostEdit
{
    [Parameter]
    public string? Id { get; set; }
    private BlogPost Post { get; set; } = new BlogPost();
    private List<Category> Categories { get; set; } = new List<Category>();
    private List<Tag> Tags { get; set; } = new List<Tag>();

    private string? selectedCategory;
    private string markDownAsHTML { get; set; } = string.Empty;
    private BlogNavigationLock? NavigationLock { get; set; }
    private MarkdownPipeline _pipeline = default!;

    protected override Task OnInitializedAsync()
    {
        this._pipeline = new MarkdownPipelineBuilder().UseEmojiAndSmiley().Build();
        return base.OnInitializedAsync();
    }

    protected override async Task OnAfterRenderAsync( bool firstRender )
    {
        if( firstRender && string.IsNullOrEmpty( this.Id ) )
        {
            BlogPost? saved = await this._storage.GetAsync<BlogPost>( "EditCurrentPost" ).ConfigureAwait( false );
            if( saved is not null )
            {
                this.Post = saved;
                _ = this.InvokeAsync( () => this.StateHasChanged() );
            }
        }
        await base.OnAfterRenderAsync( firstRender ).ConfigureAwait( false );
    }

    protected override async Task OnParametersSetAsync()
    {
        if( this.Id is not null )
        {
            BlogPost? p = await this._api.GetBlogPostAsync( this.Id ).ConfigureAwait( false );
            if( p is not null )
            {
                this.Post = p;
                if( this.Post.Category is not null )
                {
                    this.selectedCategory = this.Post.Category.Id;
                }

                await this.UpdateHTMLAsync().ConfigureAwait( false );
            }
        }

        this.Categories = (await this._api.GetCategoriesAsync().ConfigureAwait( false )) ?? new List<Category>();
        this.Tags = (await this._api.GetTagsAsync().ConfigureAwait( false )) ?? new List<Tag>();
        base.OnParametersSet();
    }

    protected async Task UpdateHTMLAsync()
    {
        if( string.IsNullOrEmpty( this.Post.Text ) == false )
        {
            await this._notificationService.SendNotification( this.Post ).ConfigureAwait( false );
            this.markDownAsHTML = Markdown.ToHtml( this.Post.Text, this._pipeline );
            if( string.IsNullOrEmpty( this.Post.Id ) )
            {
                await this._storage.SetAsync( "EditCurrentPost", this.Post ).ConfigureAwait( false );
            }
        }
    }

    private async Task SavePost()
    {
        if( (string.IsNullOrEmpty( this.selectedCategory ) == false) && (this.Categories is not null) )
        {
            Category? category = this.Categories.FirstOrDefault( c => c.Id == this.selectedCategory );
            if( category is not null )
            {
                this.Post.Category = category;
            }
        }

        _ = await this._api.SaveBlogPostAsync( this.Post ).ConfigureAwait( false );
        this.NavigationLock?.CurrentEditContext.MarkAsUnmodified();
        this._manager.NavigateTo( "/admin/blogposts" );
    }
}
