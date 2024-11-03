using AutoMapper;
using BankSystem.App.DTOs;
using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.MappingProfiles
{
    public class EmployeeProfile: Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.Name} {src.Surname}"))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
            .ForMember(dest => dest.PasNumber, opt => opt.MapFrom(src => src.Passport))
            .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
            .ForMember(dest => dest.Salary, opt => opt.MapFrom(src => src.Salary))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            ;

            CreateMap<EmployeeDto, Employee>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName.Split().First()))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.FullName.Split().Last()))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Passport, opt => opt.MapFrom(src => src.PasNumber))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                .ForMember(dest => dest.Salary, opt => opt.MapFrom(src => src.Salary))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Bonus, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.Contract, opt => opt.MapFrom(src => "N/A"))
                .ForMember(dest => dest.DateStartWork, opt => opt.MapFrom(src => DateOnly.FromDateTime(DateTime.Today)));
        }
    }
}
