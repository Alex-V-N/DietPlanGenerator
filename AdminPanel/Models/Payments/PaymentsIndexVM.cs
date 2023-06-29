using FitCookieAI_ApplicationService.DTOs.Others;
using FitCookieAI_ApplicationService.DTOs.UserRelated;

namespace AdminPanel.Models.Payments
{
    public class PaymentsIndexVM
    {
        public List<PaymentDTO>? Payments { get; set; } = new List<PaymentDTO>();
        public List<UserDTO>? Users { get; set; }
    }
}
