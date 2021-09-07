using ImageMagick;
using System.Security.Cryptography;
using System.Text;

namespace BlizzardWind.App.Common.Tools
{
    public class FileEncryptTool
    {
        /// <summary>
        /// 加密文件
        /// </summary>
        /// <param name="inPath">待加密文件的路径</param>
        /// <param name="outPath">加密之后文件的说出路径</param>
        /// <param name="key">加密密钥 - 16字节</param>
        public static void EncryptFile(string inPath, string outPath, string key)
        {
            if (string.IsNullOrEmpty(key) || key.Trim().Length != 16)
                throw new Exception("加密密钥不符合规范");
            if (!File.Exists(inPath))
                throw new Exception("待加密文件不存在");
            using (FileStream fs = new FileStream(inPath, FileMode.Open, FileAccess.Read))
            {
                using (var AES = Aes.Create())
                {
                    AES.IV = Encoding.UTF8.GetBytes(key);
                    AES.Key = Encoding.UTF8.GetBytes(key);
                    AES.Mode = CipherMode.CBC;
                    AES.Padding = PaddingMode.PKCS7;

                    using (var memStream = new MemoryStream())
                    {
                        CryptoStream cryptoStream = new CryptoStream(memStream, AES.CreateEncryptor(),
                            CryptoStreamMode.Write);
                        fs.CopyTo(cryptoStream);
                        cryptoStream.FlushFinalBlock();
                        File.WriteAllBytes(outPath, memStream.ToArray());
                    }
                }
            }
        }

        /// <summary>
        /// 压缩，加密封面图片
        /// </summary>
        /// <param name="inPath">待加密文件的路径</param>
        /// <param name="outPath">加密之后文件的说出路径</param>
        /// <param name="key">加密密钥 - 16字节</param>
        public static void EncryptCoverFile(string inPath, string outPath, string key)
        {
            if (string.IsNullOrEmpty(key) || key.Trim().Length != 16)
                throw new Exception("加密密钥不符合规范");
            if (!File.Exists(inPath))
                throw new Exception("待加密文件不存在");
            var image = new MagickImage(inPath);
            int pixel = 512;
            var min = image.Width < image.Height ? image.Width : image.Height;
            if (min <= pixel)
            {
                EncryptFile(inPath, outPath, key);
                return;
            }
            var multiple = min / pixel;
            var size = new MagickGeometry(image.Width / multiple, image.Height / multiple);
            image.Resize(size);
            using (MemoryStream fsm = new MemoryStream())
            {
                image.Write(fsm);
                fsm.Flush();
                fsm.Position = 0;
                using (var AES = Aes.Create())
                {
                    AES.IV = Encoding.UTF8.GetBytes(key);
                    AES.Key = Encoding.UTF8.GetBytes(key);
                    AES.Mode = CipherMode.CBC;
                    AES.Padding = PaddingMode.PKCS7;

                    using (var memStream = new MemoryStream())
                    {
                        CryptoStream cryptoStream = new CryptoStream(memStream, AES.CreateEncryptor(),
                            CryptoStreamMode.Write);
                        fsm.CopyTo(cryptoStream);
                        cryptoStream.FlushFinalBlock();
                        File.WriteAllBytes(outPath, memStream.ToArray());
                    }
                }
            }
        }


        /// <summary>
        /// 解密文件
        /// </summary>
        /// <param name="inPath">待解密文件的路径</param>
        /// <param name="outPath">解密之后文件的存储路径</param>
        /// <param name="key">密钥 - 16字节</param>
        public static void DecryptFile(string inPath, string outPath, string key)
        {
            if (string.IsNullOrEmpty(key) || key.Trim().Length != 16)
                throw new Exception("解密密钥不符合规范");
            if (!File.Exists(inPath))
                throw new Exception("待解密文件不存在");
            using (FileStream fs = new FileStream(inPath, FileMode.Open, FileAccess.Read))
            {
                using (var AES = Aes.Create())
                {
                    AES.IV = Encoding.UTF8.GetBytes(key);
                    AES.Key = Encoding.UTF8.GetBytes(key);
                    AES.Mode = CipherMode.CBC;
                    AES.Padding = PaddingMode.PKCS7;

                    using (var memStream = new MemoryStream())
                    {
                        CryptoStream cryptoStream = new CryptoStream(memStream, AES.CreateDecryptor(),
                            CryptoStreamMode.Write);

                        fs.CopyTo(cryptoStream);
                        cryptoStream.FlushFinalBlock();
                        File.WriteAllBytes(outPath, memStream.ToArray());
                    }
                }
            }
        }

        /// <summary>
        /// 解密文件
        /// </summary>
        /// <param name="inPath">待解密文件的路径</param>
        /// <param name="outPath">解密之后文件的存储路径</param>
        /// <param name="key">密钥 - 16字节</param>
        public static byte[] GetDecryptFileBytes(string inPath, string key)
        {
            if (string.IsNullOrEmpty(key) || key.Trim().Length != 16)
                throw new Exception("解密密钥不符合规范");
            if (!File.Exists(inPath))
                throw new Exception("待解密文件不存在");
            using (FileStream fs = new FileStream(inPath, FileMode.Open, FileAccess.Read))
            {
                using (var AES = Aes.Create())
                {
                    AES.IV = Encoding.UTF8.GetBytes(key);
                    AES.Key = Encoding.UTF8.GetBytes(key);
                    AES.Mode = CipherMode.CBC;
                    AES.Padding = PaddingMode.PKCS7;

                    using (MemoryStream memStream = new MemoryStream())
                    {
                        CryptoStream cryptoStream = new CryptoStream(memStream, AES.CreateDecryptor(),
                                CryptoStreamMode.Write);

                        fs.CopyTo(cryptoStream);
                        cryptoStream.FlushFinalBlock();
                        return memStream.ToArray();
                    }

                }
            }
        }

        /// <summary>
        /// 获取密钥
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GuidToKey(Guid id)
        {
            return id.ToString().Trim().Substring(0, 16);
        }
    }
}
