using FitCookieAI_ApplicationService.DTOs.Others;

namespace AdminPanel.Models.PaymentPlans
{
    public class PaymentPlansIndexVM
    {
        public List<PaymentPlanDTO>? PaymentPlans { get; set; }
        public List<PaymentPlanFeatureDTO>? PaymentPlanFeatures { get; set;}
    }
}
