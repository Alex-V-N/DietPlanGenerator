using FitCookieAI_ApplicationService.DTOs.Others;
using FitCookieAI_Data.Entities.Others;
using FitCookieAI_Repository.Implementations.Base;
using FitCookieAI_Repository.Implementations.Others;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCookieAI_ApplicationService.Implementations.Others
{
	public class PaymentPlansManagementService
	{
		public static async Task<List<PaymentPlanDTO>> GetAll()
		{
			using (MyUnitOfWork unitOfWork = new MyUnitOfWork())
			{
				unitOfWork.BeginTransaction();

				PaymentPlansRepository paymentPlansRepo = new PaymentPlansRepository(unitOfWork);
				List<PaymentPlan> paymentPlans = await paymentPlansRepo.GetAll();

				List<PaymentPlanDTO> paymentsDTO = new List<PaymentPlanDTO>();

				if (paymentPlans != null)
				{
					foreach (var item in paymentPlans)
					{
						paymentsDTO.Add(new PaymentPlanDTO
						{
							Id = item.Id,
							Title = item.Title,
							Description = item.Description,
							PricePerMonth = item.PricePerMonth
						});
					}

					unitOfWork.Commit();
				}
				else
				{
					unitOfWork.Rollback();
				}
				return paymentsDTO;
			}
		}

		public static async Task<PaymentPlanDTO> GetById(int id)
		{
			using (MyUnitOfWork unitOfWork = new MyUnitOfWork())
			{
				unitOfWork.BeginTransaction();

				PaymentPlansRepository paymentPlansRepo = new PaymentPlansRepository(unitOfWork);
				PaymentPlanDTO paymentPlanDTO = new PaymentPlanDTO();

				PaymentPlan paymentPlan = await paymentPlansRepo.GetById(id);

				if (paymentPlan != null)
				{
					paymentPlanDTO.Id = paymentPlan.Id;
					paymentPlanDTO.Title = paymentPlan.Title;
					paymentPlanDTO.Description = paymentPlan.Description;
					paymentPlanDTO.PricePerMonth = paymentPlan.PricePerMonth;

					unitOfWork.Commit();
				}
				else
				{
					unitOfWork.Rollback();
				}
				return paymentPlanDTO;
			}
		}

		public static async Task Save(PaymentPlanDTO paymentPlanDTO)
		{
			using (MyUnitOfWork unitOfWork = new MyUnitOfWork())
			{
				unitOfWork.BeginTransaction();

				PaymentPlansRepository paymentPlansRepo = new PaymentPlansRepository(unitOfWork);
				PaymentPlan paymentPlan = new PaymentPlan();

				if (paymentPlanDTO != null)
				{
					if (paymentPlanDTO.Id == 0)
					{
						paymentPlan = new PaymentPlan
						{
							Title = paymentPlanDTO.Title,
							Description = paymentPlanDTO.Description,
							PricePerMonth = paymentPlanDTO.PricePerMonth,
						};
					}
					else
					{
						paymentPlan = new PaymentPlan
						{
							Id = paymentPlanDTO.Id,
							Title = paymentPlanDTO.Title,
							Description = paymentPlanDTO.Description,
							PricePerMonth = paymentPlanDTO.PricePerMonth,
						};
					}

					await paymentPlansRepo.Save(paymentPlan);
					unitOfWork.Commit();
				}
				else
				{
					unitOfWork.Rollback();
				}
			}
		}

		public static async Task Delete(int id)
		{
			using (MyUnitOfWork unitOfWork = new MyUnitOfWork())
			{
				unitOfWork.BeginTransaction();

				PaymentPlansRepository paymentPlansRepo = new PaymentPlansRepository(unitOfWork);
				PaymentPlanFeaturesRepository paymentPlanFeaturesRepo = new PaymentPlanFeaturesRepository(unitOfWork);
				PaymentPlansToUsersRepository paymentPlansToUsersRepo = new PaymentPlansToUsersRepository(unitOfWork);

				PaymentPlan paymentPlan = await paymentPlansRepo.GetById(id);

				if (paymentPlan != null)
				{
					List<PaymentPlanFeature> features = await paymentPlanFeaturesRepo.GetAll(f => f.PaymentPlanId == id);
					foreach (var feature in features)
					{
						await paymentPlanFeaturesRepo.Delete(feature);
					}

					List<PaymentPlanToUser> plansToUsers = await paymentPlansToUsersRepo.GetAll(p => p.PaymentPlanId == id);
					foreach (var planToUser in plansToUsers)
					{
						await paymentPlansToUsersRepo.Delete(planToUser);
					}

					await paymentPlansRepo.Delete(paymentPlan);
					unitOfWork.Commit();
				}
				else
				{
					unitOfWork.Rollback();
				}
			}
		}
	}
}
