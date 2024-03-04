using BusinessObject.Model;
using CatCoffeePlatformAPI.Protos;
using DTO.BookingDTO;
using Grpc.Core;
using Microsoft.OData.Edm;
using NuGet.Packaging;
using Repository.Interface;
using static System.Reflection.Metadata.BlobBuilder;
using Booking = CatCoffeePlatformAPI.Protos.Booking;

namespace CatCoffeePlatformAPI.Service;

public class BookingCRUDService : BookingCRUD.BookingCRUDBase
{
    private readonly IBookingRepo _bookingRepo;
    public BookingCRUDService(IBookingRepo bookingRepo)
    {
        _bookingRepo = bookingRepo;
    }

    public override Task<Bookings> SelectAll(Empty requestData, ServerCallContext context)
    {
        Bookings responseData = new Bookings();
        var query = _bookingRepo
            .Get(0, 0, null, null, null, null, null, null, null, null, null)
            .Result
            .Payload?
            .Select(b => new Booking()
            {
                BookingId = b.BookingId ?? 0,
                BookingDate = b.BookingDate?.ToString("yyyy-MM-dd HH:mm:ss") ?? "None",
                Date = b.Date.ToString("yyyy-MM-dd") ?? "None",
                Slots = b.Slots ?? 0,
                TotalMoney = double.Parse(b.TotalMoney.ToString() ?? "0"),
                PaymentDate = b.PaymentDate?.ToString("yyyy-MM-dd") ?? "None" ,
                Status = b.Status ?? 0,
                AreaId = b.AreaId ?? 0,
                TimeFrameId = b.TimeFrameId ?? 0,
                UserId = b.UserId.ToString() ?? "None",
                CoffeeShopId = b.CoffeeShopId ?? 0
            });
        responseData.Items.AddRange(query);
        return Task.FromResult(responseData);
    }
}
