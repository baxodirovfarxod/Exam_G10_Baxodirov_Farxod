namespace UserContacts.Dal.Entities;

public class User
{
    public long UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }

    public long UserRoleId { get; set; }
    public UserRole UserRole { get; set; }

    public List<Contact> Contacts { get; set; }
    public ICollection<RefreshToken> RefreshTokens { get; set; }
}
