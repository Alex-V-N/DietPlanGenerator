namespace AdminPanel.RestComunication.FitCookieAI
{
    public class FitCookieAI_RequestRouter
    {
        //Payments
        public string Payments_GetAll = "/api/Payments/GetAll";
        public string Payments_GetById = "/api/Payments/GetById?";
        public string Payments_Save = "/api/Payments/Save";
        public string Payments_Charge = "/api/Payments/Charge";
        public string Payments_Delete = "/api/Payments/Delete?";

        //PaymentPlans
        public string PaymentPlans_GetAll = "/api/PaymentPlans/GetAll";
        public string PaymentPlans_GetById = "/api/PaymentPlans/GetById?";
        public string PaymentPlans_Save = "/api/PaymentPlans/Save";
        public string PaymentPlans_Delete = "/api/PaymentPlans/Delete?";

        //PaymentPlanFeatures
        public string PaymentPlanFeatures_GetAll = "/api/PaymentPlanFeatures/GetAll";
        public string PaymentPlanFeatures_GetById = "/api/PaymentPlanFeatures/GetById?";
        public string PaymentPlanFeatures_Save = "/api/PaymentPlanFeatures/Save";
        public string PaymentPlanFeatures_Delete = "/api/PaymentPlanFeatures/Delete?";

        //PaymentPlansToUsers
        public string PaymentPlansToUsers_GetAll = "/api/PaymentPlansToUsers/GetAll";
        public string PaymentPlansToUsers_GetById = "/api/PaymentPlansToUsers/GetById?";
        public string PaymentPlansToUsers_Save = "/api/PaymentPlansToUsers/Save";
        public string PaymentPlansToUsers_Delete = "/api/PaymentPlansToUsers/Delete?";

        //Users
        public string Users_GetAll = "/api/Users/GetAll";
        public string Users_GetById = "/api/Users/GetById?";
        public string Users_Update = "/api/Users/Update";
        public string Users_Delete = "/api/Users/Delete?";

		//Admins
		public string Admins_SignUp = "/api/Admins/SignUp";
		public string Admins_Login = "/api/Admins/Login?";
		public string Admins_Logout = "/api/Admins/Logout?";
		public string Admins_GetAll = "/api/Admins/GetAll";
		public string Admins_GetById = "/api/Admins/GetById?";
		public string Admins_Update = "/api/Admins/Update";
		public string Admins_Delete = "/api/Admins/Delete?";

		//AdminStatuses
		public string AdminStatuses_GetAll = "/api/AdminStatuses/GetAll";
		public string AdminStatuses_GetById = "/api/AdminStatuses/GetById?";
		public string AdminStatuses_Save = "/api/AdminStatuses/Save";
		public string AdminStatuses_Delete = "/api/AdminStatuses/Delete?";
	}
}
