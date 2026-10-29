
namespace PostsTags.Core.Entities;

public class Post
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Body { get; set; } = string.Empty;

    public string CoverImagePath { get; set; } = string.Empty;

    public bool IsPinned { get; set; }

    public string UserId { get; set; } = string.Empty;

    public AppUser User { get; set; } = null!;

    public List<PostTag> PostTags { get; set; } = new();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? DeletedAt { get; set; }
}
