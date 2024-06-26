namespace abcAPI.Models.DTOs;

using System.ComponentModel.DataAnnotations;
public class PaymentDto
{
    [Range(0.01, double.MaxValue, ErrorMessage = "The value must be greater than 0.")]
    public decimal Amount { get; set; }

    public int ContractId { get; set; }
}