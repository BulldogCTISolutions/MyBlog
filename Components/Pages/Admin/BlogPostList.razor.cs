using Data.Models.Models;

using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace Components.Pages.Admin;

public partial class BlogPostList
{
    public int TotalBlogposts { get; set; }

    private async ValueTask<ItemsProviderResult<BlogPost>> LoadPosts( ItemsProviderRequest request )
    {
        if( this.TotalBlogposts == 0 )
        {
            this.TotalBlogposts = await this._api.GetBlogPostCountAsync().ConfigureAwait( false );
        }

        int numBlogPosts = Math.Min( request.Count, this.TotalBlogposts - request.StartIndex );
        List<BlogPost> posts = (await this._api.GetBlogPostsAsync( numBlogPosts, request.StartIndex ).ConfigureAwait( false )) ?? new List<BlogPost>();
        return new ItemsProviderResult<BlogPost>( posts, this.TotalBlogposts );
    }
}
