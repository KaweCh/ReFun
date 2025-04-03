using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiReFund.Application.DTOs.Auth;
using XiaomiReFund.Application.DTOs.Callback;
using XiaomiReFund.Application.DTOs.Inquiry;
using XiaomiReFund.Application.DTOs.PaymentType;
using XiaomiReFund.Application.DTOs.Refund;
using XiaomiReFund.Domain.Entities;

namespace XiaomiReFund.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Auth mappings
            CreateMap<AuthenticateRequest, rmsAPI_ClientSignOn>()
                .ForMember(dest => dest.ClientUserName, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.ClientPasswordHash, opt => opt.Ignore());

            // PaymentType mappings
            CreateMap<rms_PaymentType, PaymentTypeResponse>();

            // Refund mappings
            CreateMap<CreateRefundRequest, rms_OrderRefund>()
                .ForMember(dest => dest.RefundID, opt => opt.Ignore())
                .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(src => DateTime.Parse(src.TransactionDate)))
                .ForMember(dest => dest.TxnStatus, opt => opt.MapFrom(src => 0)) // Default to Processing
                .ForMember(dest => dest.CreateDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateDate, opt => opt.Ignore());

            CreateMap<rms_OrderRefund, RefundDto>()
                .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(src => src.TransactionDate.ToString("yyyy-MM-dd")));

            CreateMap<rms_OrderRefund, RefundStatusDto>()
                .ForMember(dest => dest.StatusDescription, opt => opt.MapFrom(src => src.Status.StatusDescription))
                .ForMember(dest => dest.PaymentName, opt => opt.MapFrom(src => src.PaymentTypeEntity.PaymentName));

            // Inquiry mappings
            CreateMap<InquireRefundStatusRequest, rms_OrderRefund>()
                .ForMember(dest => dest.RefundID, opt => opt.Ignore())
                .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(src => DateTime.Parse(src.TransactionDate)))
                .ForMember(dest => dest.TxnStatus, opt => opt.Ignore())
                .ForMember(dest => dest.ClientID, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreateDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateDate, opt => opt.Ignore());

            // Callback mappings
            CreateMap<rms_OrderRefund, SendCallbackRequest>()
                .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(src => src.TransactionDate.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => GetStatusText(src.TxnStatus)))
                .ForMember(dest => dest.Msg, opt => opt.MapFrom(src => src.Status.StatusDescription));

            CreateMap<rms_OrderRefund, EnqueueCallbackRequest>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => GetStatusText(src.TxnStatus)))
                .ForMember(dest => dest.StatusMessage, opt => opt.MapFrom(src => src.Status.StatusDescription))
                .ForMember(dest => dest.RetryCount, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.ScheduledTime, opt => opt.MapFrom(src => DateTime.Now));
        }

        private static string GetStatusText(byte statusCode)
        {
            return statusCode switch
            {
                1 => "Approved",
                2 => "Rejected",
                _ => "Processing"
            };
        }
    }
}