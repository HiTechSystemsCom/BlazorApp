using BlazorApp.Data.Entities;

namespace BlazorApp.Data.WebAPI;

public partial class RefreshToken
{
    public int TokenId { get; set; }
    public int UserId { get; set; }
    public string Token { get; set; }
    public DateTime ExpiryDate { get; set; }

    public virtual User User { get; set; }
}
