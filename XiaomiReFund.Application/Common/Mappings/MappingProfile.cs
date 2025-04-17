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
using XiaomiReFund.Domain.Constants;
using XiaomiReFund.Domain.Entities;

namespace XiaomiReFund.Application.Common.Mappings
{
    /// <summary>
    /// โปรไฟล์การแมปข้อมูลสำหรับการแปลงระหว่างเอนทิตีและดีทีโอ
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // การแมปข้อมูลสำหรับการรับรองตัวตน
            // แปลงคำขอรับรองตัวตนไปเป็นข้อมูลลูกค้า โดยข้าม password hash
            CreateMap<AuthenticateRequest, rmsAPI_ClientSignOn>()
                .ForMember(dest => dest.ClientUserName, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.ClientPasswordHash, opt => opt.Ignore());

            // การแมปประเภทการชำระเงิน
            // แปลงเอนทิตีประเภทการชำระเงินเป็นการตอบกลับประเภทการชำระเงิน
            CreateMap<rms_PaymentType, PaymentTypeResponse>();

            // การแมปข้อมูลการคืนเงิน
            // แปลงคำขอสร้างการคืนเงินไปเป็นเอนทิตีการคืนเงิน
            CreateMap<CreateRefundRequest, rms_OrderRefund>()
                .ForMember(dest => dest.RefundID, opt => opt.Ignore()) // ข้ามการแมป ID การคืนเงิน
                .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(src => DateTime.Parse(src.TransactionDate))) // แปลงวันที่เป็นวัตถุ DateTime
                .ForMember(dest => dest.TxnStatus, opt => opt.MapFrom(src => RefundConstants.TransactionStatus.Processing)) // ตั้งค่าสถานะเริ่มต้นเป็นกำลังดำเนินการ
                .ForMember(dest => dest.CreateDate, opt => opt.Ignore()) // ข้ามวันที่สร้าง
                .ForMember(dest => dest.UpdateDate, opt => opt.Ignore()); // ข้ามวันที่อัปเดท

            // แปลงเอนทิตีการคืนเงินเป็นดีทีโอการคืนเงิน
            CreateMap<rms_OrderRefund, RefundDto>()
                .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(src => src.TransactionDate.ToString("yyyy-MM-dd"))); // แปลงวันที่เป็นสตริง

            // แปลงเอนทิตีการคืนเงินเป็นดีทีโอสถานะการคืนเงิน
            CreateMap<rms_OrderRefund, RefundStatusDto>()
                .ForMember(dest => dest.StatusDescription, opt => opt.MapFrom(src => src.Status.StatusDescription)) // แมปคำอธิบายสถานะ
                .ForMember(dest => dest.PaymentName, opt => opt.MapFrom(src => src.PaymentTypeEntity.PaymentName)); // แมปชื่อการชำระเงิน

            // การแมปข้อมูลการสอบถาม
            // แปลงคำขอสถานะการคืนเงินเป็นเอนทิตีการคืนเงิน
            CreateMap<InquireRefundStatusRequest, rms_OrderRefund>()
                .ForMember(dest => dest.RefundID, opt => opt.Ignore()) // ข้ามการแมป ID การคืนเงิน
                .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(src => DateTime.Parse(src.TransactionDate))) // แปลงวันที่เป็นวัตถุ DateTime
                .ForMember(dest => dest.TxnStatus, opt => opt.Ignore()) // ข้ามสถานะธุรกรรม
                .ForMember(dest => dest.ClientID, opt => opt.Ignore()) // ข้ามรหัสลูกค้า
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore()) // ข้ามผู้แก้ไข
                .ForMember(dest => dest.CreateDate, opt => opt.Ignore()) // ข้ามวันที่สร้าง
                .ForMember(dest => dest.UpdateDate, opt => opt.Ignore()); // ข้ามวันที่อัปเดท

            // การแมปข้อมูลการเรียกกลับ
            // แปลงเอนทิตีการคืนเงินเป็นคำขอส่งการเรียกกลับ
            CreateMap<rms_OrderRefund, SendCallbackRequest>()
                .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(src => src.TransactionDate.ToString("yyyy-MM-dd"))) // แปลงวันที่เป็นสตริง
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => GetStatusText(src.TxnStatus))) // แปลงรหัสสถานะเป็นข้อความ
                .ForMember(dest => dest.Msg, opt => opt.MapFrom(src => src.Status.StatusDescription)); // แมปข้อความสถานะ

            // แปลงเอนทิตีการคืนเงินเป็นคำขอคิวการเรียกกลับ
            CreateMap<rms_OrderRefund, EnqueueCallbackRequest>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => GetStatusText(src.TxnStatus))) // แปลงรหัสสถานะเป็นข้อความ
                .ForMember(dest => dest.StatusMessage, opt => opt.MapFrom(src => src.Status.StatusDescription)) // แมปข้อความสถานะ
                .ForMember(dest => dest.RetryCount, opt => opt.MapFrom(src => 0)) // ตั้งค่าจำนวนการลองใหม่เป็น 0
                .ForMember(dest => dest.ScheduledTime, opt => opt.MapFrom(src => DateTime.Now)); // ตั้งเวลาที่กำหนดเป็นเวลาปัจจุบัน
        }

        /// <summary>
        /// แปลงรหัสสถานะเป็นข้อความสถานะ
        /// </summary>
        /// <param name="statusCode">รหัสสถานะธุรกรรม</param>
        /// <returns>ข้อความสถานะ</returns>
        private static string GetStatusText(byte statusCode)
        {
            // แปลงรหัสสถานะเป็นข้อความที่เข้าใจง่าย โดยใช้ค่าคงที่จาก RefundConstants
            return statusCode switch
            {
                RefundConstants.TransactionStatus.Approved => RefundConstants.CallbackStatus.Approved,
                RefundConstants.TransactionStatus.Rejected => RefundConstants.CallbackStatus.Rejected,
                _ => RefundConstants.CallbackStatus.Processing // สถานะเริ่มต้นหรือกำลังดำเนินการ
            };
        }
    }
}