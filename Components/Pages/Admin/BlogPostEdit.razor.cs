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

                this.UpdateHTML();
            }
        }

        this.Categories = (await this._api.GetCategoriesAsync().ConfigureAwait( false )) ?? new List<Category>();
        this.Tags = (await this._api.GetTagsAsync().ConfigureAwait( false )) ?? new List<Tag>();
        base.OnParametersSet();
    }

    protected void UpdateHTML()
    {
        this.markDownAsHTML = Markdig.Markdown.ToHtml( this.Post.Text, this._pipeline );
    }

    private bool hasTag( Tag tag )
    {
        return this.Post.Tags.Contains( tag );
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
