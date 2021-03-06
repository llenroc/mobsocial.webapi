﻿using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ImageProcessor;

namespace Nop.Plugin.WebApi.MobSocial.Services
{
    public class MobSocialImageProcessor : IMobSocialImageProcessor
    {
        public void WriteBytesToImage(byte[] imageBytes, string filePath, ImageFormat imageFormat)
        {
            using (var mStream = new MemoryStream(imageBytes))
            {
                var image = Image.FromStream(mStream);
                image.Save(filePath, imageFormat);
            }
        }

        public byte[] ResizeImage(byte[] imageBytes, int width, int height)
        {
            var size = new Size(width, height);
            using (var inStream = new MemoryStream(imageBytes))
            using (var outStream = new MemoryStream())
            using (var imageFactory = new ImageFactory(preserveExifData: true))
            {
                imageFactory.Load(inStream)
                    .Resize(size)
                    .Save(outStream);
                return outStream.ToArray();
            }
        }

        public void WriteResizedImage(byte[] imageBytes, int width, int height, string filePath, ImageFormat imageFormat)
        {
            //first resize image
            var resizedBytes = ResizeImage(imageBytes, width, height);

            //write the image to file system
            WriteBytesToImage(resizedBytes, filePath, imageFormat);
        }

        public void WriteResizedImage(string sourceFile, string destinationFile, int width, int height, ImageFormat imageFormat)
        {
            if (!File.Exists(sourceFile))
            {
                return;
            }

            //read bytes from image 
            var imageBytes = File.ReadAllBytes(sourceFile);

            //write the resized image
            WriteResizedImage(imageBytes, width, height, destinationFile, imageFormat);
        }

        public byte[] CropImage(byte[] imageBytes, int left, int top, int width, int height)
        {
            var rectangle = new Rectangle(left, top, width, height);
            using (var inStream = new MemoryStream(imageBytes))
            using (var outStream = new MemoryStream())
            using (var imageFactory = new ImageFactory(preserveExifData: true))
            {
                imageFactory.Load(inStream)
                    .Crop(rectangle)
                    .Save(outStream);
                return outStream.ToArray();
            }
        }

        public void WriteCroppedImage(byte[] imageBytes, int left, int top, int width, int height, string filePath,
            ImageFormat imageFormat)
        {
            var croppedBytes = CropImage(imageBytes, left, top, width, height);
            WriteBytesToImage(croppedBytes, filePath, imageFormat);
        }

        public void WriteCroppedImage(string sourceFile, string destinationFile, int left, int top, int width, int height,
            string filePath, ImageFormat imageFormat)
        {
            if (!File.Exists(sourceFile))
            {
                return;
            }

            //read bytes from image 
            var imageBytes = File.ReadAllBytes(sourceFile);
            WriteCroppedImage(imageBytes, left, top, width, height, filePath, imageFormat);
        }
    }
}