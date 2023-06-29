namespace AdminPanel.RestComunication.FitCookieAI
{
    public class FitCookieAI_RequestBuilder
    {
		private readonly FitCookieAI_RequestRouter _router = new FitCookieAI_RequestRouter();

		//Payments
		public string DeletePaymentsByIdRequestBuilder(string uri, int id)
		{
			return uri + _router.Payments_Delete + $"id={id}";
		}
		public string GetAllPaymentsRequestBuilder(string uri)
		{
			return uri + _router.Payments_GetAll;
		}
		public string SavePaymentsRequestBuilder(string uri)
		{
			return uri + _router.Payments_Save;
		}
		public string ChargePaymentsRequestBuilder(string uri)
		{
			return uri + _router.Payments_Charge;
		}
		public string GetPaymentsByIdRequestBuilder(string uri, int id)
		{
			return uri + _router.Payments_GetById + $"id={id}";
		}

		//PaymentPlans
		public string DeletePaymentPlansByIdRequestBuilder(string uri, int id)
		{
			return uri + _router.PaymentPlans_Delete + $"id={id}";
		}
		public string GetAllPaymentPlansRequestBuilder(string uri)
		{
			return uri + _router.PaymentPlans_GetAll;
		}
		public string SavePaymentPlansRequestBuilder(string uri)
		{
			return uri + _router.PaymentPlans_Save;
		}
		public string GetPaymentPlansByIdRequestBuilder(string uri, int id)
		{
			return uri + _router.PaymentPlans_GetById + $"id={id}";
		}

		//PaymentPlanFeatures
		public string DeletePaymentPlanFeaturesByIdRequestBuilder(string uri, int id)
		{
			return uri + _router.PaymentPlanFeatures_Delete + $"id={id}";
		}
		public string GetAllPaymentPlanFeaturesRequestBuilder(string uri)
		{
			return uri + _router.PaymentPlanFeatures_GetAll;
		}
		public string SavePaymentPlanFeaturesRequestBuilder(string uri)
		{
			return uri + _router.PaymentPlanFeatures_Save;
		}
		public string GetPaymentPlanFeaturesByIdRequestBuilder(string uri, int id)
		{
			return uri + _router.PaymentPlanFeatures_GetById + $"id={id}";
		}

		//PaymentPlansToUsers
		public string DeletePaymentPlansToUsersByIdRequestBuilder(string uri, int id)
		{
			return uri + _router.PaymentPlansToUsers_Delete + $"id={id}";
		}
		public string GetAllPaymentPlansToUsersRequestBuilder(string uri)
		{
			return uri + _router.PaymentPlansToUsers_GetAll;
		}
		public string SavePaymentPlansToUsersRequestBuilder(string uri)
		{
			return uri + _router.PaymentPlansToUsers_Save;
		}
		public string GetPaymentPlansToUsersByIdRequestBuilder(string uri, int id)
		{
			return uri + _router.PaymentPlansToUsers_GetById + $"id={id}";
		}

		//Users
		public string GetUsersByIdRequestBuilder(string uri, int id)
		{
			return uri + _router.Users_GetById + $"id={id}";
		}
		public string DeleteUsersByIdRequestBuilder(string uri, int id)
		{
			return uri + _router.Users_Delete + $"id={id}";
		}
		public string UpdateUsersRequestBuilder(string uri)
		{
			return uri + _router.Users_Update;
		}
		public string GetAllUsersRequestBuilder(string uri)
		{
			return uri + _router.Users_GetAll;
		}

		//Admins
		public string LoginRequestBuilder(string uri, string email, string password)
		{
			return uri + _router.Admins_Login + $"email={email}&password={password}";
		}
		public string SignUpRequestBuilder(string uri)
		{
			return uri + _router.Admins_SignUp;
		}
		public string LogoutRequestBuilder(string uri, int userId)
		{
			return uri + _router.Admins_Logout + $"userId={userId}";
		}
		public string GetAdminsByIdRequestBuilder(string uri, int id)
		{
			return uri + _router.Admins_GetById + $"id={id}";
		}

		public string DeleteAdminsByIdRequestBuilder(string uri, int id)
		{
			return uri + _router.Admins_Delete + $"id={id}";
		}

		public string UpdateAdminsRequestBuilder(string uri)
		{
			return uri + _router.Admins_Update;
		}

		public string GetAllAdminsRequestBuilder(string uri)
		{
			return uri + _router.Admins_GetAll;
		}

		//AdminStatuses
		public string DeleteAdminStatusesByIdRequestBuilder(string uri, int id)
		{
			return uri + _router.AdminStatuses_Delete + $"id={id}";
		}
		public string GetAllAdminStatusesRequestBuilder(string uri)
		{
			return uri + _router.AdminStatuses_GetAll;
		}
		public string SaveAdminStatusesRequestBuilder(string uri)
		{
			return uri + _router.AdminStatuses_Save;
		}
		public string GetAdminStatusesByIdRequestBuilder(string uri, int id)
		{
			return uri + _router.AdminStatuses_GetById + $"id={id}";
		}
	}
}
