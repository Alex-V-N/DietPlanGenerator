using FitCookieAI_ApplicationService.DTOs.AdminRelated;
using FitCookieAI_Data.Entities.AdminRelated;
using FitCookieAI_Repository.Implementations.AdminRelated;
using FitCookieAI_Repository.Implementations.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCookieAI_ApplicationService.Implementations.AdminRelated
{
	public class AdminsManagementService
	{
		public static async Task<List<AdminDTO>> GetAll()
		{
			using (MyUnitOfWork unitOfWork = new MyUnitOfWork())
			{
				unitOfWork.BeginTransaction();

				AdminsRepository adminsRepo = new AdminsRepository(unitOfWork);
				List<Admin> admins = await adminsRepo.GetAll();

				List<AdminDTO> adminsDTO = new List<AdminDTO>();

				if (admins != null)
				{
					foreach (var item in admins)
					{
						adminsDTO.Add(new AdminDTO
						{
							Id = item.Id,
							Email = item.Email,
							FirstName = item.FirstName,
							LastName = item.LastName,
							Password = item.Password,
							DOB = item.DOB,
							StatusId = item.StatusId,
							Gender = item.Gender,
							ProfilePhotoName = item.ProfilePhotoName
						});
					}

					unitOfWork.Commit();
				}
				else
				{
					unitOfWork.Rollback();
				}
				return adminsDTO;
			}
		}

		public static async Task<AdminDTO> GetById(int id)
		{
			using (MyUnitOfWork unitOfWork = new MyUnitOfWork())
			{
				unitOfWork.BeginTransaction();

				AdminsRepository adminsRepo = new AdminsRepository(unitOfWork);
				AdminDTO adminDTO = new AdminDTO();

				Admin admin = await adminsRepo.GetById(id);

				if (admin != null)
				{
					adminDTO.Id = admin.Id;
					adminDTO.Email = admin.Email;
					adminDTO.Password = admin.Password;
					adminDTO.FirstName = admin.FirstName;
					adminDTO.LastName = admin.LastName;
					adminDTO.DOB = admin.DOB;
					adminDTO.StatusId = admin.StatusId;
					adminDTO.Gender = admin.Gender;
					adminDTO.ProfilePhotoName = admin.ProfilePhotoName;

					unitOfWork.Commit();
				}
				else
				{
					unitOfWork.Rollback();
				}
				return adminDTO;
			}
		}

		public static async Task Save(AdminDTO adminDTO)
		{
			using (MyUnitOfWork unitOfWork = new MyUnitOfWork())
			{
				unitOfWork.BeginTransaction();

				AdminsRepository adminsRepo = new AdminsRepository(unitOfWork);
				Admin admin = new Admin();

				if (adminDTO != null)
				{
					if (adminDTO.Id == 0)
					{
						admin = new Admin
						{
							Email = adminDTO.Email,
							FirstName = adminDTO.FirstName,
							LastName = adminDTO.LastName,
							Password = adminDTO.Password,
							DOB = adminDTO.DOB,
							StatusId = adminDTO.StatusId,
							Gender = adminDTO.Gender,
							ProfilePhotoName = adminDTO.ProfilePhotoName
						};
					}
					else
					{
						admin = new Admin
						{
							Id = adminDTO.Id,
							Email = adminDTO.Email,
							FirstName = adminDTO.FirstName,
							LastName = adminDTO.LastName,
							Password = adminDTO.Password,
							DOB = adminDTO.DOB,
							StatusId = adminDTO.StatusId,
							Gender = adminDTO.Gender,
							ProfilePhotoName = adminDTO.ProfilePhotoName
						};
					}

					await adminsRepo.Save(admin);
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
				AdminsRepository adminsRepo = new AdminsRepository();
				Admin admin = await adminsRepo.GetFirstOrDefault(u => u.Email == email);

				if (admin == null)
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

				AdminsRepository adminsRepo = new AdminsRepository(unitOfWork);
				RefreshAdminTokensRepository refreshAdminTokensRepo = new RefreshAdminTokensRepository(unitOfWork);

				Admin admin = await adminsRepo.GetById(id);

				if (admin != null)
				{
					List<RefreshAdminToken> refreshAdminTokens = await refreshAdminTokensRepo.GetAll(t => t.AdminId == admin.Id);
					foreach (var token in refreshAdminTokens)
					{
						await refreshAdminTokensRepo.Delete(token);
					}

					await adminsRepo.Delete(admin);
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
