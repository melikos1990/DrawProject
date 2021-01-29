using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MultipartDataMediaFormatter;
using MultipartDataMediaFormatter.Infrastructure;
using SMARTII.Domain.Types;

namespace SMARTII.Assist.Converter
{
    public class MutipartFormDataConverter : FormMultipartEncodedMediaTypeFormatter
    {
        public MutipartFormDataConverter(MultipartFormatterSettings settings = null) : base(settings)
        {
        }

        public override bool CanReadType(Type type)
        {
            return base.CanReadType(type.TryGetConcreteType());
        }

        public override bool CanWriteType(Type type)
        {
            return base.CanWriteType(type.TryGetConcreteType());
        }

        public override MediaTypeFormatter GetPerRequestFormatterInstance(Type type, HttpRequestMessage request, MediaTypeHeaderValue mediaType)
        {
            return base.GetPerRequestFormatterInstance(type.TryGetConcreteType(), request, mediaType);
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            return base.ReadFromStreamAsync(type.TryGetConcreteType(), readStream, content, formatterLogger);
        }

        public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
        {
            base.SetDefaultContentHeaders(type.TryGetConcreteType(), headers, mediaType);
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            return base.WriteToStreamAsync(type.TryGetConcreteType(), value, writeStream, content, transportContext);
        }
    }
}