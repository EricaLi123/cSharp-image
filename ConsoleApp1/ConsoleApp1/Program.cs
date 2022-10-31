using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.IO;
using Svg;
using ThumbImage;

//Test.Test.test_getThumbSizeOfImage();
Test.Test.test_loadImage();


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
            //小数
            caseList.Add(new ThumbSizeCaseItem
            {
                inputOriginalSize = new ImageSize { width = 735, height = 385 },
                inputContainerSize = inputContainerSize,
                output = new ImageSize { width = 100, height = 52 },
            });


            Console.WriteLine("============ GetThumbImageSize ==============");
            var wrongCase = caseList.Where(i =>
            {
                var output =ThumbImage.ThumbImage.GetThumbImageSize(i.inputOriginalSize, i.inputContainerSize);

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

            imgUrls.ForEach(i =>
            {
                var index = imgUrls.IndexOf(i);

                var fileName = index + "_" + i.type;
                var path = dire + fileName;

                var thumb = ThumbImage.ThumbImage.getThumb(i.url);

                if (thumb != null)
                {

                    //thumb.Save(path + "." + thumb.RawFormat);
                    thumb.Save(path  ,ImageFormat.Jpeg);
                    Console.WriteLine(index + '.' + i.type + "  => " + thumb.RawFormat + "       " + new ImageSize(thumb));
                }
                else
                {
                    Console.WriteLine("==============================================failed: " + i.url + " " + i.url);
                }
            });
        }

    }

}