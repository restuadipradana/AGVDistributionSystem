using AutoMapper;
using AGVDistributionSystem.Models;
using AGVDistributionSystem.DTO;

namespace AGVDistributionSystem.Helpers.AutoMapper
{
    public class EfToDtoMappingProfile : Profile
    {
        public EfToDtoMappingProfile()
        {
            CreateMap<ProcessStatus, ProcessStatusDTO>();
            CreateMap<Roles, RolesDTO>();
            CreateMap<RunningPO, RunningPODTO>();
            CreateMap<UserRole, UserRoleDTO>();
            CreateMap<V_PO2, V_PO2DTO>();
            CreateMap<VW_MES_Org, VW_MES_OrgDTO>();
            CreateMap<VW_UserAcc, VW_UserAccDTO>();
        }
    }
}