using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Resources.Messages
{
    public struct MessageTypes
    {
        public const string TextMesage = "TextMesage";
        public const string ImagesMessage = "ImagesMessage";
        public const string FileMessage = "FileMessage";
        public const string VoiceMessage = "VoiceMessage";
        public const string GifMessage = "GifMessage";

        public static List<string> GetAll()
        {
            return new List<string>()
        {
                TextMesage,
                ImagesMessage,
                FileMessage,
                VoiceMessage,
                GifMessage
            };
        }
    }
}
