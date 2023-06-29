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
	public class PaymentPlanFeaturesManagementService
	{
		public static async Task<List<PaymentPlanFeatureDTO>> GetAll()
		{
			using (MyUnitOfWork unitOfWork = new MyUnitOfWork())
			{
				unitOfWork.BeginTransaction();

				PaymentPlanFeaturesRepository paymentPlanFeaturesRepo = new PaymentPlanFeaturesRepository(unitOfWork);
				List<PaymentPlanFeature> paymentPlanFeatrures = await paymentPlanFeaturesRepo.GetAll();

				List<PaymentPlanFeatureDTO> paymentPlanFeaturesDTO = new List<PaymentPlanFeatureDTO>();

				if (paymentPlanFeatrures != null)
				{
					foreach (var item in paymentPlanFeatrures)
					{
						paymentPlanFeaturesDTO.Add(new PaymentPlanFeatureDTO
						{
							Id = item.Id,
							Title = item.Title,
							PaymentPlanId = item.PaymentPlanId
						});
					}

					unitOfWork.Commit();
				}
				else
				{
					unitOfWork.Rollback();
				}
				return paymentPlanFeaturesDTO;
			}
		}

		public static async Task<PaymentPlanFeatureDTO> GetById(int id)
		{
			using (MyUnitOfWork unitOfWork = new MyUnitOfWork())
			{
				unitOfWork.BeginTransaction();

				PaymentPlanFeaturesRepository paymentPlanFeaturesRepo = new PaymentPlanFeaturesRepository(unitOfWork);
				PaymentPlanFeatureDTO PaymentPlanFeatureDTO = new PaymentPlanFeatureDTO();

				PaymentPlanFeature paymentPlanFeature = await paymentPlanFeaturesRepo.GetById(id);

				if (paymentPlanFeature != null)
				{
					PaymentPlanFeatureDTO.Id = paymentPlanFeature.Id;
					PaymentPlanFeatureDTO.Title = paymentPlanFeature.Title;
					PaymentPlanFeatureDTO.PaymentPlanId = paymentPlanFeature.PaymentPlanId;

					unitOfWork.Commit();
				}
				else
				{
					unitOfWork.Rollback();
				}
				return PaymentPlanFeatureDTO;
			}
		}

		public static async Task Save(PaymentPlanFeatureDTO PaymentPlanFeatureDTO)
		{
			using (MyUnitOfWork unitOfWork = new MyUnitOfWork())
			{
				unitOfWork.BeginTransaction();

				PaymentPlanFeaturesRepository paymentPlanFeaturesRepo = new PaymentPlanFeaturesRepository(unitOfWork);
				PaymentPlanFeature paymentPlanFeature = new PaymentPlanFeature();

				if (PaymentPlanFeatureDTO != null)
				{
					if (PaymentPlanFeatureDTO.Id == 0)
					{
						paymentPlanFeature = new PaymentPlanFeature
						{
							Title = PaymentPlanFeatureDTO.Title,
							PaymentPlanId = paymentPlanFeature.PaymentPlanId
						};
					}
					else
					{
						paymentPlanFeature = new PaymentPlanFeature
						{
							Id = PaymentPlanFeatureDTO.Id,
							Title = PaymentPlanFeatureDTO.Title,
							PaymentPlanId = paymentPlanFeature.PaymentPlanId
						};
					}

					await paymentPlanFeaturesRepo.Save(paymentPlanFeature);
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

				PaymentPlanFeaturesRepository paymentPlanFeaturesRepo = new PaymentPlanFeaturesRepository(unitOfWork);

				PaymentPlanFeature paymentPlanFeature = await paymentPlanFeaturesRepo.GetById(id);

				if (paymentPlanFeature != null)
				{

					await paymentPlanFeaturesRepo.Delete(paymentPlanFeature);
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
