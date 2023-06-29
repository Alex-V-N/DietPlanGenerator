using FitCookieAI_ApplicationService.DTOs.AdminRelated;
using FitCookieAI_ApplicationService.DTOs.Others;
using FitCookieAI_Data.Entities.AdminRelated;
using FitCookieAI_Data.Entities.Others;
using FitCookieAI_Repository.Implementations.AdminRelated;
using FitCookieAI_Repository.Implementations.Base;
using FitCookieAI_Repository.Implementations.Others;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCookieAI_ApplicationService.Implementations.Others
{
	public class PaymentsManagementService
	{
		public static async Task<List<PaymentDTO>> GetAll()
		{
			using (MyUnitOfWork unitOfWork = new MyUnitOfWork())
			{
				unitOfWork.BeginTransaction();

				PaymentsRepository paymentsRepo = new PaymentsRepository(unitOfWork);
				List<Payment> payments = await paymentsRepo.GetAll();

				List<PaymentDTO> paymentsDTO = new List<PaymentDTO>();

				if (payments != null)
				{
					foreach (var item in payments)
					{
						paymentsDTO.Add(new PaymentDTO
						{
							Id = item.Id,
							Amount = item.Amount,
							Currency = item.Currency,
							Description = item.Description,
							Source = item.Source,
							UserId = item.UserId
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

		public static async Task<PaymentDTO> GetById(int id)
		{
			using (MyUnitOfWork unitOfWork = new MyUnitOfWork())
			{
				unitOfWork.BeginTransaction();

				PaymentsRepository paymentsRepo = new PaymentsRepository(unitOfWork);
				PaymentDTO paymentDTO = new PaymentDTO();

				Payment payment = await paymentsRepo.GetById(id);

				if (payment != null)
				{
					paymentDTO.Id = payment.Id;
					paymentDTO.Amount = payment.Amount;
					paymentDTO.Currency = payment.Currency;
					paymentDTO.Description = payment.Description;
					paymentDTO.Source = payment.Source;
					paymentDTO.UserId = payment.UserId;

					unitOfWork.Commit();
				}
				else
				{
					unitOfWork.Rollback();
				}
				return paymentDTO;
			}
		}

		public static async Task Save(PaymentDTO paymentDTO)
		{
			using (MyUnitOfWork unitOfWork = new MyUnitOfWork())
			{
				unitOfWork.BeginTransaction();

				PaymentsRepository paymentsRepo = new PaymentsRepository(unitOfWork);
				Payment payment = new Payment();

				if (paymentDTO != null)
				{
					if (paymentDTO.Id == 0)
					{
						payment = new Payment
						{
							Amount = paymentDTO.Amount,
							Currency = paymentDTO.Currency,
							Description = paymentDTO.Description,
							Source = paymentDTO.Source,
							UserId = paymentDTO.UserId
						};
					}
					else
					{
						payment = new Payment
						{
							Id = paymentDTO.Id,
							Amount = paymentDTO.Amount,
							Currency = paymentDTO.Currency,
							Description = paymentDTO.Description,
							Source = paymentDTO.Source,
							UserId = paymentDTO.UserId
						};
					}

					await paymentsRepo.Save(payment);
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

				PaymentsRepository paymentsRepo = new PaymentsRepository(unitOfWork);

				Payment payment = await paymentsRepo.GetById(id);

				if (payment != null)
				{

					await paymentsRepo.Delete(payment);
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
