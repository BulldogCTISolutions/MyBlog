using System.Text.Json;

using Data.Models.Interfaces;
using Data.Models.Models;

using Microsoft.Extensions.Options;

namespace Data;

public sealed class BlogApiJsonDirectAccess : IBlogApi
{
    private readonly BlogApiJsonDirectAccessSetting _settings;
    private List<BlogPost>? _blogPosts;
    private List<Category>? _categories;
    private List<Tag>? _tags;

    public BlogApiJsonDirectAccess( IOptions<BlogApiJsonDirectAccessSetting> option )
    {
        if( option is null )
        {
            throw new ArgumentNullException( nameof( option ) );
        }
        this._settings = option.Value;
        if( Directory.Exists( this._settings.DataPath ) == false )
        {
            _ = Directory.CreateDirectory( this._settings.DataPath );
        }
        if( Directory.Exists( $@"{this._settings.DataPath}\{this._settings.BlogPostsFolder}" ) == false )
        {
            _ = Directory.CreateDirectory( $@"{this._settings.DataPath}\{this._settings.BlogPostsFolder}" );
        }
        if( Directory.Exists( $@"{this._settings.DataPath}\{this._settings.CategoriesFolder}" ) == false )
        {
            _ = Directory.CreateDirectory( $@"{this._settings.DataPath}\{this._settings.CategoriesFolder}" );
        }
        if( Directory.Exists( $@"{this._settings.DataPath}\{this._settings.TagsFolder}" ) == false )
        {
            _ = Directory.CreateDirectory( $@"{this._settings.DataPath}\{this._settings.TagsFolder}" );
        }
    }

    public async Task<int> GetBlogPostCountAsync()
    {
        await this.LoadBlogPostsAsync().ConfigureAwait( false );
        return this._blogPosts is null ? 0 : this._blogPosts.Count;
    }

    public async Task<List<BlogPost>?> GetBlogPostsAsync( int numberOfPosts, int startIndex )
    {
        await this.LoadBlogPostsAsync().ConfigureAwait( false );
        return this._blogPosts ?? new List<BlogPost>();
    }

    public async Task<List<Category>?> GetCategoriesAsync()
    {
        await this.LoadCategoriesAsync().ConfigureAwait( false );
        return this._categories ?? new List<Category>();
    }

    public async Task<List<Tag>?> GetTagsAsync()
    {
        await this.LoadTagsAsync().ConfigureAwait( false );
        return this._tags ?? new List<Tag>();
    }

    public async Task<BlogPost?> GetBlogPostAsync( string id )
    {
        await this.LoadBlogPostsAsync().ConfigureAwait( false );
        return this._blogPosts is null
            ? throw new ArgumentNullException( id, "Blog posts not found" )
            : this._blogPosts.FirstOrDefault( b => b.Id == id );
    }

    public async Task<Category?> GetCategoryAsync( string id )
    {
        await this.LoadCategoriesAsync().ConfigureAwait( false );
        return this._categories is null
            ? throw new ArgumentNullException( id, "Categories not found" )
            : this._categories.FirstOrDefault( b => b.Id == id );
    }

    public async Task<Tag?> GetTagAsync( string id )
    {
        await this.LoadTagsAsync().ConfigureAwait( false );
        return this._tags is null
            ? throw new ArgumentNullException( id, "Tags not found" )
            : this._tags.FirstOrDefault( b => b.Id == id );
    }

    public async Task<BlogPost?> SaveBlogPostAsync( BlogPost item )
    {
        if( item is null )
        {
            throw new ArgumentNullException( nameof( item ) );
        }
        item.Id ??= Guid.NewGuid().ToString();
        await this.SaveAsync( this._blogPosts, this._settings.BlogPostsFolder, $"{item.Id}.json", item ).ConfigureAwait( false );
        return item;
    }

    public async Task<Category?> SaveCategoryAsync( Category item )
    {
        if( item is null )
        {
            throw new ArgumentNullException( nameof( item ) );
        }
        item.Id ??= Guid.NewGuid().ToString();
        await this.SaveAsync( this._categories, this._settings.CategoriesFolder, $"{item.Id}.json", item ).ConfigureAwait( false );
        return item;
    }

    public async Task<Tag?> SaveTagAsync( Tag item )
    {
        if( item is null )
        {
            throw new ArgumentNullException( nameof( item ) );
        }
        item.Id ??= Guid.NewGuid().ToString();
        await this.SaveAsync( this._tags, this._settings.TagsFolder, $"{item.Id}.json", item ).ConfigureAwait( false );
        return item;
    }

    public Task DeleteBlogPostAsync( string id )
    {
        this.DeleteAsync( this._settings.BlogPostsFolder, id );
        if( this._blogPosts is null )
        {
            BlogPost? item = this._blogPosts!.FirstOrDefault( b => b.Id == id );
            if( item is not null )
            {
                _ = this._blogPosts!.Remove( item );
            }
        }
        return Task.CompletedTask;
    }

    public Task DeleteCategoryAsync( string id )
    {
        this.DeleteAsync( this._settings.CategoriesFolder, id );
        if( this._categories is null )
        {
            Category? item = this._categories!.FirstOrDefault( b => b.Id == id );
            if( item is not null )
            {
                _ = this._categories!.Remove( item );
            }
        }
        return Task.CompletedTask;
    }

    public Task DeleteTagAsync( string id )
    {
        this.DeleteAsync( this._settings.TagsFolder, id );
        if( this._tags is null )
        {
            Tag? item = this._tags!.FirstOrDefault( b => b.Id == id );
            if( item is not null )
            {
                _ = this._tags!.Remove( item );
            }
        }
        return Task.CompletedTask;
    }

    public Task InvalidateCacheAsync()
    {
        this._blogPosts = null;
        this._categories = null;
        this._tags = null;
        return Task.CompletedTask;
    }

    private async Task SaveAsync<T>( List<T>? list, string folder, string filename, T item )
    {
        string? filePath = $@"{this._settings.DataPath}\{folder}\{filename}";
        await File.WriteAllTextAsync( filePath, JsonSerializer.Serialize( item ) ).ConfigureAwait( false );
        list ??= new List<T>();
        if( list.Contains( item ) == false )
        {
            list.Add( item );
        }
    }

    private void DeleteAsync( string folder, string id )
    {
        string? filePath = $@"{this._settings.DataPath}\{folder}\{id}.json";
        try
        {
            File.Delete( filePath );
        }
        catch( ArgumentException )
        {
            // No worries about being unable to delete at this time.
        }
    }

    private Task LoadBlogPostsAsync()
    {
        this.Load( ref this._blogPosts, this._settings.BlogPostsFolder );
        return Task.CompletedTask;
    }

    private Task LoadTagsAsync()
    {
        this.Load( ref this._tags, this._settings.TagsFolder );
        return Task.CompletedTask;
    }

    private Task LoadCategoriesAsync()
    {
        this.Load( ref this._categories, this._settings.CategoriesFolder );
        return Task.CompletedTask;
    }

    private void Load<T>( ref List<T>? list, string folder )
    {
        if( list is null )
        {
            list = new List<T>();
            string? fullPath = $@"{this._settings.DataPath}\{folder}";
            foreach( string f in Directory.GetFiles( fullPath ) )
            {
                string? json = File.ReadAllText( f );
                T? bp = JsonSerializer.Deserialize<T>( json );
                if( bp is not null )
                {
                    list.Add( bp );
                }
            }
        }
    }
}
