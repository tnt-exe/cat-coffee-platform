using DTO.BookingDTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Common.CustomJsonConverter
{
    internal class BookingResponseDTOConverter : JsonConverter
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(BookingResponseDTO);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if(!(value is BookingResponseDTO bookingResponse))
            {
                throw new JsonSerializationException("Expected BookingResponseDTO object value.");
            }

            var booking = new
            {
                BookingId = bookingResponse.BookingId,
                BookingDate = bookingResponse.BookingDate,
                Date = bookingResponse.Date.ToString("yyyy-MM-dd"),
                Slots = bookingResponse.Slots,
                TotalMoney = bookingResponse.TotalMoney,
                PaymentDate = bookingResponse.PaymentDate,
                Status = bookingResponse.Status,
                AreaId = bookingResponse.AreaId,
                Area = bookingResponse.Area,
                TimeFrameId = bookingResponse.TimeFrameId,
                TimeFrame = bookingResponse.TimeFrame,
                UserId = bookingResponse.UserId,
                User = bookingResponse.User,
                CoffeeShopId = bookingResponse.CoffeeShopId,
                CoffeeShop = bookingResponse.CoffeeShop,
            };

            writer.WriteValue(booking);
        }
    }
}
