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
	public class AdminStatusesManagementService
	{
		public static async Task<List<AdminStatusDTO>> GetAll()
		{
			using (MyUnitOfWork unitOfWork = new MyUnitOfWork())
			{
				unitOfWork.BeginTransaction();

				AdminStatusesRepository adminStatusesRepo = new AdminStatusesRepository(unitOfWork);
				List<AdminStatus> adminStatuses = await adminStatusesRepo.GetAll();

				List<AdminStatusDTO> adminStatusesDTO = new List<AdminStatusDTO>();

				if (adminStatuses != null)
				{
					foreach (var item in adminStatuses)
					{
						adminStatusesDTO.Add(new AdminStatusDTO
						{
							Id = item.Id,
							Name = item.Name
						});
					}

					unitOfWork.Commit();
				}
				else
				{
					unitOfWork.Rollback();
				}
				return adminStatusesDTO;
			}
		}

		public static async Task<AdminStatusDTO> GetById(int id)
		{
			using (MyUnitOfWork unitOfWork = new MyUnitOfWork())
			{
				unitOfWork.BeginTransaction();

				AdminStatusesRepository adminStatusesRepo = new AdminStatusesRepository(unitOfWork);
				AdminStatusDTO adminStatusDTO = new AdminStatusDTO();

				AdminStatus adminStatus = await adminStatusesRepo.GetById(id);

				if (adminStatus != null)
				{
					adminStatusDTO.Id = adminStatus.Id;
					adminStatusDTO.Name = adminStatus.Name;

					unitOfWork.Commit();
				}
				else
				{
					unitOfWork.Rollback();
				}
				return adminStatusDTO;
			}
		}

		public static async Task Save(AdminStatusDTO adminStatusDTO)
		{
			using (MyUnitOfWork unitOfWork = new MyUnitOfWork())
			{
				unitOfWork.BeginTransaction();

				AdminStatusesRepository adminStatusesRepo = new AdminStatusesRepository(unitOfWork);
				AdminStatus adminStatus = new AdminStatus();

				if (adminStatusDTO != null)
				{
					if (adminStatusDTO.Id == 0)
					{
						adminStatus = new AdminStatus
						{
							Name = adminStatusDTO.Name
						};
					}
					else
					{
						adminStatus = new AdminStatus
						{
							Id = adminStatusDTO.Id,
							Name = adminStatusDTO.Name
						};
					}

					await adminStatusesRepo.Save(adminStatus);
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

				AdminStatusesRepository adminStatusesRepo = new AdminStatusesRepository(unitOfWork);

				AdminStatus adminStatus = await adminStatusesRepo.GetById(id);

				if (adminStatus != null)
				{
					
					await adminStatusesRepo.Delete(adminStatus);
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
