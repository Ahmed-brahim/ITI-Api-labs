using lab2._1.Dtos;
using lab2._1.Models;
using AutoMapper;

namespace lab2._1.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Student, StudentDto>()
                .ForMember(dest => dest.FullName,
                           opt => opt.MapFrom(src => $"{src.StFname} {src.StLname}"))
                .ForMember(dest => dest.DepartmentName,
                           opt => opt.MapFrom(src => src.Dept!.DeptName))
                .ForMember(dest => dest.SupervisorName,
                           opt => opt.MapFrom(src =>
                                src.StSuperNavigation != null
                                ? $"{src.StSuperNavigation.StFname} {src.StSuperNavigation.StLname}"
                                : null));

            CreateMap<InputStudentDto, Student>()
                .ForMember(dest => dest.StFname, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.StLname, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.StAddress, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.StAge, opt => opt.MapFrom(src => src.Age))
                .ForMember(dest => dest.DeptId, opt => opt.MapFrom(src => src.DepartmentId))
                .ForMember(dest => dest.StSuper, opt => opt.MapFrom(src => src.SupervisorId));
            CreateMap<InputDepartmentDto, Department>()
                .ForMember(dest => dest.DeptName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.DeptLocation, opt => opt.MapFrom(src => src.Location))
                .ForMember(dest => dest.DeptDesc, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.DeptManager, opt => opt.MapFrom(src => src.ManagerId));

        } 
    }
}
