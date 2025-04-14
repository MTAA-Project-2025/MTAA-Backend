using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Resources.Images
{
    public class ImagesSizes
    {
        public static Dictionary<ImageSavingTypes, ICollection<ImagesSize>> Sizes { get; set; } = new Dictionary<ImageSavingTypes, ICollection<ImagesSize>>()
        {
            {
                ImageSavingTypes.UserAvatar,new List<ImagesSize>()
                {
                    new ImagesSize()
                    {
                        Width=100,
                        Height=100,
                        Type=ImageSizeType.Small
                    },
                    new ImagesSize()
                    {
                        Width=300,
                        Height=300,
                        Type=ImageSizeType.Middle
                    }
                }
            },
            {
                ImageSavingTypes.ChannelImage,new List<ImagesSize>()
                {
                    new ImagesSize()
                    {
                        Width=100,
                        Height=100,
                        Type=ImageSizeType.Small
                    },
                    new ImagesSize()
                    {
                        Width=300,
                        Height=300,
                        Type=ImageSizeType.Middle
                    },
                    new ImagesSize()
                    {
                        Width=500,
                        Height = 500,
                        Type=ImageSizeType.Large
                    }
                }
            },
            {
                ImageSavingTypes.PostImage,new List<ImagesSize>()
                {
                    new ImagesSize()
                    {
                        Width=300,
                        Height=300,
                        Type=ImageSizeType.Small
                    },
                    new ImagesSize()
                    {
                        Width=500,
                        Type=ImageSizeType.Middle
                    },
                    new ImagesSize()
                    {
                        Width=800,
                        Type=ImageSizeType.Large
                    }
                }
            }
        };
    }
}
