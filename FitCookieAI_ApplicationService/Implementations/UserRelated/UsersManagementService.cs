using FitCookieAI_ApplicationService.DTOs.UserRelated;
using FitCookieAI_Data.Entities.UserRelated;
using FitCookieAI_Repository.Implementations.UserRelated;
using FitCookieAI_Repository.Implementations.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitCookieAI_Repository.Implementations.Others;
using FitCookieAI_Data.Entities.Others;

namespace FitCookieAI_ApplicationService.Implementations.UserRelated
{
	public class UsersManagementService
	{
		public static async Task<List<UserDTO>> GetAll()
		{
			using (MyUnitOfWork unitOfWork = new MyUnitOfWork())
			{
				unitOfWork.BeginTransaction();

				UsersRepository UsersRepo = new UsersRepository(unitOfWork);
				List<User> Users = await UsersRepo.GetAll();

				List<UserDTO> UsersDTO = new List<UserDTO>();

				if (Users != null)
				{
					foreach (var item in Users)
					{
						UsersDTO.Add(new UserDTO
						{
							Id = item.Id,
							Email = item.Email,
							FirstName = item.FirstName,
							LastName = item.LastName,
							Password = item.Password,
							DOB = item.DOB,
							Gender = item.Gender
						});
					}

					unitOfWork.Commit();
				}
				else
				{
					unitOfWork.Rollback();
				}
				return UsersDTO;
			}
		}

		public static async Task<UserDTO> GetById(int id)
		{
			using (MyUnitOfWork unitOfWork = new MyUnitOfWork())
			{
				unitOfWork.BeginTransaction();

				UsersRepository UsersRepo = new UsersRepository(unitOfWork);
				UserDTO UserDTO = new UserDTO();

				User User = await UsersRepo.GetById(id);

				if (User != null)
				{
					UserDTO.Id = User.Id;
					UserDTO.Email = User.Email;
					UserDTO.Password = User.Password;
					UserDTO.FirstName = User.FirstName;
					UserDTO.LastName = User.LastName;
					UserDTO.DOB = User.DOB;
					UserDTO.Gender = User.Gender;

					unitOfWork.Commit();
				}
				else
				{
					unitOfWork.Rollback();
				}
				return UserDTO;
			}
		}

		public static async Task Save(UserDTO UserDTO)
		{
			using (MyUnitOfWork unitOfWork = new MyUnitOfWork())
			{
				unitOfWork.BeginTransaction();

				UsersRepository UsersRepo = new UsersRepository(unitOfWork);
				User User = new User();

				if (UserDTO != null)
				{
					if (UserDTO.Id == 0)
					{
						User = new User
						{
							Email = UserDTO.Email,
							FirstName = UserDTO.FirstName,
							LastName = UserDTO.LastName,
							Password = UserDTO.Password,
							DOB = UserDTO.DOB,
							Gender = UserDTO.Gender,
						};
					}
					else
					{
						User = new User
						{
							Id = UserDTO.Id,
							Email = UserDTO.Email,
							FirstName = UserDTO.FirstName,
							LastName = UserDTO.LastName,
							Password = UserDTO.Password,
							DOB = UserDTO.DOB,
							Gender = UserDTO.Gender,
						};
					}

					await UsersRepo.Save(User);
					unitOfWork.Commit();
				}
				else
				{
					unitOfWork.Rollback();
				}
			}
		}

		public static async Task<bool> VerifyEmail(string email)
		{
			bool exists = true;

			using (MyUnitOfWork unitOfWork = new MyUnitOfWork())
			{
				unitOfWork.BeginTransaction();
				UsersRepository UsersRepo = new UsersRepository();
				User User = await UsersRepo.GetFirstOrDefault(u => u.Email == email);

				if (User == null)
				{
					exists = false;
				}

				unitOfWork.Commit();
				return exists;
			}
		}

		public static async Task Delete(int id)
		{
			using (MyUnitOfWork unitOfWork = new MyUnitOfWork())
			{
				unitOfWork.BeginTransaction();

				UsersRepository UsersRepo = new UsersRepository(unitOfWork);
				RefreshUserTokensRepository refreshUserTokensRepo = new RefreshUserTokensRepository(unitOfWork);
				PaymentPlansToUsersRepository paymentPlansToUsersRepo = new PaymentPlansToUsersRepository(unitOfWork);

				User User = await UsersRepo.GetById(id);

				if (User != null)
				{
					List<RefreshUserToken> refreshUserTokens = await refreshUserTokensRepo.GetAll(t => t.UserId == User.Id);
					foreach (var token in refreshUserTokens)
					{
						await refreshUserTokensRepo.Delete(token);
					}

					List<PaymentPlanToUser> plansToUsers = await paymentPlansToUsersRepo.GetAll(p => p.UserId == id);
					foreach (var planToUser in plansToUsers)
					{
						await paymentPlansToUsersRepo.Delete(planToUser);
					}

					await UsersRepo.Delete(User);
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
