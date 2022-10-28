using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.IO;
using Svg;

//Main.Img.test();
Test.Test.test_loadImage();

class ImageSize{
    public int width { get; set; }
    public int height { get; set; }


    public override bool Equals(object obj)
    {
        //
        // See the full list of guidelines at
        //   http://go.microsoft.com/fwlink/?LinkID=85237
        // and also the guidance for operator== at
        //   http://go.microsoft.com/fwlink/?LinkId=85238
        //

        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        return (obj as ImageSize).width == this.width && (obj as ImageSize).height == this.height;
    }
    
}
namespace Main
{


    class Img
    {


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
            if (originalSize.width / originalSize.height > containerSize.width / containerSize.height)
            {
                width = containerSize.width;
                height = width * originalSize.height / originalSize.width;
            }
            else
            {
                height = containerSize.height;
                width = height * originalSize.width / originalSize.height;
            }
            return new ImageSize { width = width, height = height };
        }

        public static Stream readStream(string url)
        {
            var webReq = WebRequest.Create(url);
            var webres = webReq.GetResponse();
            return  webres.GetResponseStream();
        }

        public static Image loadLocalImage(string path)
        {
           return  Image.FromFile(path);

        }

        public static Image loadImage(string url)
        {
            var stream = readStream(url);
            var img1 = Image.FromStream(stream);

            return img1;

        }

        public static SvgDocument loadSvg(string url)
        {
            var stream = readStream(url);
            return SvgDocument.Open<SvgDocument>(stream);
        }

        //public static Image loadWebP(string url)
        //{
        //    return;
        //}

        public static Image convertType(SvgDocument svg)
        {
            return svg.Draw();
        }

        public static Image getThumb(Image image, ImageSize size)
        {
            return image.GetThumbnailImage(size.width, size.height, () => false, IntPtr.Zero);
        }
    }

}

namespace Test
{

    class ThumbSizeCaseItem
    {
        public ImageSize inputOriginalSize { get; set; }
        public ImageSize inputContainerSize { get; set; }
        public ImageSize output { get; set; }
    }


    class LoadImageCaseItem
    {
        public string url { get; set; }
        public string type{ get; set; }
    }

    class Test
    {


        public static void test_getThumbSizeOfImage()
        {
            var caseList = new List<ThumbSizeCaseItem>();

            var inputContainerSize = new ImageSize { width = 100, height = 200 };
            //等宽,等高 => 不变
            caseList.Add(new ThumbSizeCaseItem
            {
                inputOriginalSize = new ImageSize { width = 100, height = 200 },
                inputContainerSize = inputContainerSize,
                output = new ImageSize { width = 100, height = 200 },
            });
            //等宽, 原图高小 => 不变 
            caseList.Add(new ThumbSizeCaseItem
            {
                inputOriginalSize = new ImageSize { width = 100, height = 100 },
                inputContainerSize = inputContainerSize,
                output = new ImageSize { width = 100, height = 100 },
            });
            //等宽, 原图高大 => 缩
            caseList.Add(new ThumbSizeCaseItem
            {
                inputOriginalSize = new ImageSize { width = 100, height = 400 },
                inputContainerSize = inputContainerSize,
                output = new ImageSize { width = 50, height = 200 },
            });

            //原图宽小,等高 => 不变
            caseList.Add(new ThumbSizeCaseItem
            {
                inputOriginalSize = new ImageSize { width = 50, height = 200 },
                inputContainerSize = inputContainerSize,
                output = new ImageSize { width = 50, height = 200 },
            });
            //原图宽小,原图高小 => 不变
            caseList.Add(new ThumbSizeCaseItem
            {
                inputOriginalSize = new ImageSize { width = 50, height = 100 },
                inputContainerSize = inputContainerSize,
                output = new ImageSize { width = 50, height = 100 },
            });
            //原图宽小, 原图高大 => 缩
            caseList.Add(new ThumbSizeCaseItem
            {
                inputOriginalSize = new ImageSize { width = 50, height = 400 },
                inputContainerSize = inputContainerSize,
                output = new ImageSize { width = 25, height = 200 },
            });

            //原图宽大, 等高 => 缩
            caseList.Add(new ThumbSizeCaseItem
            {
                inputOriginalSize = new ImageSize { width = 200, height = 200 },
                inputContainerSize = inputContainerSize,
                output = new ImageSize { width = 100, height = 100 },
            });
            //原图宽大, 原图高小 => 缩
            caseList.Add(new ThumbSizeCaseItem
            {
                inputOriginalSize = new ImageSize { width = 200, height = 100 },
                inputContainerSize = inputContainerSize,
                output = new ImageSize { width = 100, height = 50 },
            });
            //原图宽大, 原图高大 => 缩
            caseList.Add(new ThumbSizeCaseItem
            {
                inputOriginalSize = new ImageSize { width = 200, height = 400 },
                inputContainerSize = inputContainerSize,
                output = new ImageSize { width = 100, height = 200 },
            });


            Console.WriteLine("============ GetThumbImageSize ==============");
            var wrongCase = caseList.Where(i =>
            {
                var output = Main.Img.GetThumbImageSize(i.inputOriginalSize, i.inputContainerSize);

                var pass = output.Equals(i.output);

                if (!pass)
                {

                }
                Console.WriteLine("GetThumbImageSize : CASE is " + (pass ? "pass" : "fail"));
                return !pass;
            });

            if (wrongCase.Count() == 0)
            {
                Console.WriteLine("============ GetThumbImageSize ======= PASS=======");
            }
            else
            {
                Console.WriteLine("============ GetThumbImageSize ======= FAIL:  " + wrongCase.Count() + "=======");
            }
        }


        public static void test_loadImage()
        {
            var imgUrls = new[]{
                new {
                    type= "jpg",
                    url= "https://scpic.chinaz.net/files/pic/pic9/201311/apic2098.jpg",},
              new {
                    type="png",
                    url="https://img.zcool.cn/community/015d885d9c1beaa8012060bea1b658.png@2o.png",},
               new                {
                    type="png",
                    url="https://lkemu.blob.core.windows.net/container-4149/task/v3/26ade7a5-ebd1-42f5-a2e1-7b2e57976383/download.png",
                }, new                {
                    type="svg",
                    url="https://i-teacher.leyserkids.jp/img/alert.svg",
                },
                new                {
                    type="svg",
                    url="https://lkemu.blob.core.windows.net/container-4149/task/v3/31abacd2-0549-4182-afa3-e1eae343a82a/ingress.svg",
                },
                new {
                    type="jfif",
                    url="https://lkemu.blob.core.windows.net/container-4149/task/v3/c4d00e54-5a12-4b21-9018-c61356556e75/01e8fa5965991ba8012193a3195e5a.jfif" ,},
                new                {
                    type="gif",
                    //name="",
                    url="https://lkemu.blob.core.windows.net/container-4149/task/v3/77f3533c-89f7-400c-925a-1db542b44ffc/R-C.gif",
                }, 
                new {
                    type="gif",
                    url="https://ts1.cn.mm.bing.net/th/id/R-C.80a92d5722e8f4513c711e01b75c7473?rik=viBX%2fKU01HGFrg&riu=http%3a%2f%2fimg95.699pic.com%2fphoto%2f40158%2f1890.gif_wh860.gif&ehk=bC1DuUggkKrUnIpMm1Ekb1DMGN9zMJ3LslfHC8cZXEc%3d&risl=&pid=ImgRaw&r=0",},
                new
                {
                    type="webp",
                    url="https://lkemu.blob.core.windows.net/container-4149/task/v3/e8bc4a65-6428-46ed-b561-71c3efbdb4bc/0004.webp",
                },
                new
                {
                    type="bmp",
                    url="https://lkemu.blob.core.windows.net/container-4149/task/v3/856d320b-4866-41e4-bfac-e4e69da8328c/3.bmp",
                },
                new
                {
                    type="png",
                    url="https://lkemu.blob.core.windows.net/container-4149/task/v3/c466a415-143f-4060-91c5-694398620f63/实为png的jpg.jpg",
                }
                }.ToList();

            var dire = "C:\\Users\\ericali\\Downloads\\Img\\";

            Directory.GetFiles(dire).ToList().ForEach(i =>
            {
                File.Delete(i);
            });

            var time = DateTime.Now;

            var fileNamePre = String.Join('-', new[] {
                    time.Hour.ToString().PadLeft(2,'0'),
                    time.Minute.ToString().PadLeft(2,'0'),
                    time.Second.ToString().PadLeft(2,'0'),
                    time.Millisecond.ToString().PadLeft(3,'0'),  });
            imgUrls.ForEach(i =>
            {
                var index = imgUrls.IndexOf(i);

                var fileName = fileNamePre + "_" + index +"_"+i.type;
                var path = dire + fileName;
                try
                {
                    var res = Main.Img.loadImage(i.url);
                    res.Save(path+"."+res.RawFormat);

                    Console.WriteLine("LoadImage : " + index + '.' + i.type+"  => "+ res.RawFormat);
                    return;                   
                } catch (Exception e)
                {
                }

                try
                {
                    var res = Main.Img.convertType(Main.Img.loadSvg(i.url));
                    res.Save(path + "." + res.RawFormat);

                    //var img = Main.Img.loadLocalImage(dire + "svg__" + fileName+ ".jpg");
                    Console.WriteLine("LoadSvg: " + index + '.' + i.type + " =>  " + res.RawFormat);
                    return;
                }
                catch (Exception e)
                { 
                }


                Console.WriteLine("==============================================failed: " + index + i.type);
            });
        }
    }

}