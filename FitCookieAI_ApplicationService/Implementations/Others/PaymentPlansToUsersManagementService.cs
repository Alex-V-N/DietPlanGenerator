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
	public class PaymentPlansToUsersManagementService
	{
		public static async Task<List<PaymentPlanToUserDTO>> GetAll()
		{
			using (MyUnitOfWork unitOfWork = new MyUnitOfWork())
			{
				unitOfWork.BeginTransaction();

				PaymentPlansToUsersRepository paymentPlansToUsersRepo = new PaymentPlansToUsersRepository(unitOfWork);
				List<PaymentPlanToUser> paymentPlansToUsers = await paymentPlansToUsersRepo.GetAll();

				List<PaymentPlanToUserDTO> paymentPlansToUsersDTO = new List<PaymentPlanToUserDTO>();

				if (paymentPlansToUsers != null)
				{
					foreach (var item in paymentPlansToUsers)
					{
						paymentPlansToUsersDTO.Add(new PaymentPlanToUserDTO
						{
							Id = item.Id,
							PaymentPlanId = item.PaymentPlanId,
							UserId = item.UserId
						});
					}

					unitOfWork.Commit();
				}
				else
				{
					unitOfWork.Rollback();
				}
				return paymentPlansToUsersDTO;
			}
		}

		public static async Task<PaymentPlanToUserDTO> GetById(int id)
		{
			using (MyUnitOfWork unitOfWork = new MyUnitOfWork())
			{
				unitOfWork.BeginTransaction();

				PaymentPlansToUsersRepository paymentPlansToUsersRepo = new PaymentPlansToUsersRepository(unitOfWork);
				PaymentPlanToUserDTO PaymentPlanToUserDTO = new PaymentPlanToUserDTO();

				PaymentPlanToUser paymentPlanToUser = await paymentPlansToUsersRepo.GetById(id);

				if (paymentPlanToUser != null)
				{
					PaymentPlanToUserDTO.Id = paymentPlanToUser.Id;
					PaymentPlanToUserDTO.PaymentPlanId = paymentPlanToUser.PaymentPlanId;
					PaymentPlanToUserDTO.UserId = paymentPlanToUser.UserId;

					unitOfWork.Commit();
				}
				else
				{
					unitOfWork.Rollback();
				}
				return PaymentPlanToUserDTO;
			}
		}

		public static async Task Save(PaymentPlanToUserDTO PaymentPlanToUserDTO)
		{
			using (MyUnitOfWork unitOfWork = new MyUnitOfWork())
			{
				unitOfWork.BeginTransaction();

				PaymentPlansToUsersRepository paymentPlansToUsersRepo = new PaymentPlansToUsersRepository(unitOfWork);
				PaymentPlanToUser paymentPlanToUser = new PaymentPlanToUser();

				if (PaymentPlanToUserDTO != null)
				{
					if (PaymentPlanToUserDTO.Id == 0)
					{
						paymentPlanToUser = new PaymentPlanToUser
						{
							PaymentPlanId = PaymentPlanToUserDTO.PaymentPlanId,
							UserId= PaymentPlanToUserDTO.UserId
						};
					}
					else
					{
						paymentPlanToUser = new PaymentPlanToUser
						{
							Id = PaymentPlanToUserDTO.Id,
							PaymentPlanId = PaymentPlanToUserDTO.PaymentPlanId,
							UserId = PaymentPlanToUserDTO.UserId
						};
					}

					await paymentPlansToUsersRepo.Save(paymentPlanToUser);
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

				PaymentPlansToUsersRepository paymentPlansToUsersRepo = new PaymentPlansToUsersRepository(unitOfWork);

				PaymentPlanToUser paymentPlanToUser = await paymentPlansToUsersRepo.GetById(id);

				if (paymentPlanToUser != null)
				{

					await paymentPlansToUsersRepo.Delete(paymentPlanToUser);
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
