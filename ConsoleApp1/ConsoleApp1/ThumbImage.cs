using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.IO;
using Svg;




namespace ThumbImage
{
    class ImageSize
    {
        public ImageSize()
        {

        }
        public ImageSize(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
        public ImageSize(Image img)
        {
            width = img.Width;
            height = img.Height;
        }

        public int width { get; set; }
        public int height { get; set; }

        public override bool Equals(object obj)
        {

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return (obj as ImageSize).width == this.width && (obj as ImageSize).height == this.height;
        }

        public override string ToString()
        {
            return width + "*" + height;
        }
    }

    class ThumbImage
    {
        private ImageSize _defaultThumbSize = new ImageSize { width = 300, height = 300 };
        public ImageSize DefaultThumbSize { get => _defaultThumbSize; }

        public static ImageSize GetThumbImageSize(ImageSize originalSize, ImageSize containerSize)
        {
            // 不需要缩
            if (originalSize.width <= containerSize.width && originalSize.height < containerSize.height)
            {
                return originalSize;
            }

            // 等比例缩小
            var width = 0;
            var height = 0;
            if (1.0 * originalSize.width / originalSize.height > 1.0 * containerSize.width / containerSize.height)
            {
                width = containerSize.width;
                height = width * originalSize.height / originalSize.width;
            }
            else
            {
                height = containerSize.height;
                width = height * originalSize.width / originalSize.height;
            }
            return new ImageSize(width, height);
        }

        private static Stream readStream(string url)
        {
            var webReq = WebRequest.Create(url);
            var webres = webReq.GetResponse();
            return webres.GetResponseStream();
        }


        private static Image loadNormalImage(string url)
        {
            var stream = readStream(url);
            return Image.FromStream(stream);
        }

        private static SvgDocument loadSvg(string url)
        {
            var stream = readStream(url);
            return SvgDocument.Open<SvgDocument>(stream);
        }

        //TODO
        private static Image loadWebP(string url)
        {
            return null;
        }


        public static Image? loadImage(string url)
        {
            try
            {
                var res = loadNormalImage(url);

                Console.WriteLine("[normal Image]");
                return res;
            }
            catch (Exception e)
            {
            }

            try
            {
                var res = loadSvg(url).Draw();
                Console.WriteLine("[svg]");
                return res;
            }
            catch (Exception e)
            {
            }


            return null;
        }

        public static Image loadLocalImage(string path)
        {
            return Image.FromFile(path);
        }

        public static Image? getThumb(string url)
        {
            var img = loadImage(url);
            if (img == null)
            {
                return null;
            }
            var thumbSize = GetThumbImageSize(new ImageSize(img), new ImageSize(300, 300));
            return img.GetThumbnailImage(img.Width, img.Height, () => false, IntPtr.Zero); ;
        }
    }
}
