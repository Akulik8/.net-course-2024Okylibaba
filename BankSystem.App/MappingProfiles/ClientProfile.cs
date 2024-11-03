using AutoMapper;
using System;
using System.Linq;
using System.Collections.Generic;
using BankSystem.App.DTOs;
using BankSystem.Domain.Models;

public class ClientProfile : Profile
{
    public ClientProfile()
    {
        CreateMap<Client, ClientDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.Name} {src.Surname}"))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
            .ForMember(dest => dest.PasNumber, opt => opt.MapFrom(src => src.Passport))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.Bonus, opt => opt.MapFrom(src => src.Bonus));

        CreateMap<ClientDto, Client>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName.Split().First()))
            .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.FullName.Split().Last()))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Passport, opt => opt.MapFrom(src => src.PasNumber))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.Bonus, opt => opt.MapFrom(src => src.Bonus ?? 0));
        ;
    }
}
