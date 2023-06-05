using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Entities;

public class Customer
{
    [Key]
    public int CustomerId { get; set; }
    public string Fname { get; set; }
    public string Lname { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    [Phone]
    public string PhoneNumber { get; set; }

}