using Portfolio.Domain;

namespace Portfolio.Application.DTOs;

public class HomeFeedDto
{
    public MY_PROFILE? Profile { get; set; }
    public EXPERIENCE? CurrentRole { get; set; }
    public IList<EXPERIENCE> CompanyLogos { get; set; } = new List<EXPERIENCE>();
    public IList<BLOG_POSTS> FeaturedPosts { get; set; } = new List<BLOG_POSTS>();
}
