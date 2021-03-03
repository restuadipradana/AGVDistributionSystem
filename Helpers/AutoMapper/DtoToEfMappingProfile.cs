using AutoMapper;
using AGVDistributionSystem.DTO;
using AGVDistributionSystem.Models;

namespace AGVDistributionSystem.Helpers.AutoMapper
{
    public class DtoToEfMappingProfile : Profile
    {
        public DtoToEfMappingProfile() 
        {
            CreateMap<ProcessStatusDTO, ProcessStatus>();
            CreateMap<RolesDTO, Roles>();
            CreateMap<RunningPODTO, RunningPO>();
            CreateMap<UserRoleDTO, UserRole>();
            CreateMap<V_PO2DTO, V_PO2>();
            CreateMap<VW_MES_OrgDTO, VW_MES_Org>();
            CreateMap<VW_UserAccDTO, VW_UserAcc>();
        }
    }
}