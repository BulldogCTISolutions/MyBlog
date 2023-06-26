using Data.Models.Interfaces;
using Data.Models.Models;

namespace MyBlog.Tests;

public sealed class BlogApiMock : IBlogApi
{
    public Task DeleteBlogPostAsync( string id ) => Task.CompletedTask;

    public Task DeleteCategoryAsync( string id ) => Task.CompletedTask;

    public Task DeleteTagAsync( string id ) => Task.CompletedTask;

    public async Task<BlogPost?> GetBlogPostAsync( string id )
    {
        BlogPost blogPost = new BlogPost()
        {
            Id = id,
            Text = $"This is blog post # {id}",
            Title = $"BlogPost {id}",
            PublishDate = DateTime.Now,
            Category = await this.GetCategoryAsync( "1" ).ConfigureAwait( false )
        };
        blogPost.Tags.Add( (await this.GetTagAsync( "1" ).ConfigureAwait( false ))! );
        blogPost.Tags.Add( (await this.GetTagAsync( "2" ).ConfigureAwait( false ))! );
        return blogPost;
    }

    public Task<int> GetBlogPostCountAsync()
    {
        return Task.FromResult( 10 );
    }

    public async Task<List<BlogPost>?> GetBlogPostsAsync( int numberOfPosts, int startIndex )
    {
        List<BlogPost> list = new List<BlogPost>();
        for( int a = 0; a < numberOfPosts; ++a )
        {
            list.Add( (await this.GetBlogPostAsync( $"{a}" ).ConfigureAwait( false ))! );
        }
        return list;
    }

    public async Task<List<Category>?> GetCategoriesAsync()
    {
        List<Category> list = new List<Category>();
        for( int a = 0; a < 10; ++a )
        {
            list.Add( (await this.GetCategoryAsync( $"{a}" ).ConfigureAwait( false ))! );
        }
        return list;
    }

    public Task<Category?> GetCategoryAsync( string id )
    {
        return Task.FromResult( new Category() { Id = id, Name = $"Category {id}" } )!;
    }

    public Task<Tag?> GetTagAsync( string id )
    {
        return Task.FromResult( new Tag() { Id = id, Name = $"Tag {id}" } )!;
    }

    public async Task<List<Tag>?> GetTagsAsync()
    {
        List<Tag> list = new List<Tag>();
        for( int a = 0; a < 10; ++a )
        {
            list.Add( (await this.GetTagAsync( $"{a}" ).ConfigureAwait( false ))! );
        }
        return list;
    }

    public Task InvalidateCacheAsync() => Task.CompletedTask;

    public Task<BlogPost?> SaveBlogPostAsync( BlogPost item ) => Task.FromResult( item )!;

    public Task<Category?> SaveCategoryAsync( Category item ) => Task.FromResult( item )!;

    public Task<Tag?> SaveTagAsync( Tag item ) => Task.FromResult( item )!;
}
